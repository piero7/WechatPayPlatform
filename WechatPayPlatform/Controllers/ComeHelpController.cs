using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;

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


        [HttpPost]
        public string AddComeOrder(string openid, string location, string carnumber, DateTime endtime, string phone, string desc, string station)
        {
            return "";
        }

        [HttpPost]
        public string GetJsSign([FromBody]string url, string str, string time)
        {
            var sign = Helper.GetJsApiSignature(url, str, time);
            return sign;
        }
    }
}
