using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;
using WechatPayPlatform.Models;

namespace WechatPayPlatform.Controllers
{
    public class ComeHelpController : ApiController
    {
        [HttpGet]
        public IEnumerable<object> GetStationList()
        {
            var db = new Models.ModelContext();
            return db.StationSet.Select(item => new
            {
                name = item.Name,
                id = item.StationId
            }).AsEnumerable();
        }

        [HttpGet]
        public object GetUserInfo(string openid)
        {
            var db = new Models.ModelContext();
            // var helper = new PayInfoController();
            //var openid = helper.GetOpenidByCode(code);
            var user = db.WechatUserSet.FirstOrDefault(item => item.OpenId == openid);
            if (user == null)
            {
                return new { openid = openid };
            }
            var car = db.CarInfoSet.Where(item => item.UserId == user.UserId).OrderByDescending(item => item.LastUseDate).FirstOrDefault();
            if (car == null)
            {
                return new
                {
                    openid = openid,
                    phone = user.PhoneNumber
                };
            }
            return new
            {
                openid = openid,
                phone = user.PhoneNumber,
                sid = car.LocationDescribe,
                car = car.CarNumber,
                describe = car.Describe
            };
        }


        //[HttpPost]
        //public string AddComeOrder(string openid, string location, string carnumber, DateTime endtime, string phone, string desc, string station)
        //{
        //    return "";
        //}

        [HttpPost]
        public string GetJsSign([FromBody]string url, string str, string time)
        {
            var sign = Helper.GetJsApiSignature(url, str, time);
            return sign;
        }

        /// <summary>
        /// 获取附近小区
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Neighborhood> GetVillageList(double? x, double? y)
        {
            var db = new ModelContext();
            var dis = int.Parse(System.Configuration.ConfigurationManager.AppSettings["villageDis"]);
            var ret = new List<Neighborhood>();
            foreach (var item in db.NeighborhoodSet)
            {
                if (Location.GetDistance(x ?? 0, y ?? 0, item.LocationX ?? 0, item.LocationY ?? 0) <= dis)
                {
                    ret.Add(item);
                }
            }
            return ret;
        }

        [HttpGet]
        public IEnumerable<int> GetFreeHour(int villageId)
        {
            int[] ret = new int[2];

            var db = new ModelContext();
            var village = db.NeighborhoodSet.Include("Station").First(item => item.NeighborhoodId == villageId);

            ret[0] = village.Station.StartWorkHour ?? -1;
            ret[1] = village.Station.EndWorkHour ?? -1;

            return ret;
        }
    }
}
