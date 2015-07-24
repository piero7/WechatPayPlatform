using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WechatPayPlatform.Models;

namespace WechatPayPlatform.Controllers
{
    public class ShopHelpController : ApiController
    {
        [HttpGet]
        public IEnumerable<object> GetShopList(double x, double y)
        {
            var maxDis = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["shopDis"]);
            var db = new ModelContext();

            List<object> retList = new List<object>();
            foreach (var item in db.ShopSet.ToList())
            {
                double dis = Location.GetDistance(x, y, item.LocationX ?? 0, item.LocationY ?? 0);
                if (dis <= maxDis)
                {
                    retList.Add(new
                    {
                        dis = Math.Round(dis, 2),
                        id = item.ShopId,
                        name = item.Name,
                        img = item.ImgPath,
                        score = Convert.ToInt32(item.Score.GetAverage()),
                        price = item.Minimum,
                    });
                }
            }
            return retList;
        }

        [HttpGet]
        public int AddScore(int shopid, int score)
        {
            var db = new ModelContext();
            var slog = new ScoreLog { s1 = score, s2 = score, s3 = score, CreateDate = DateTime.Now };
            var shop = db.ShopSet.Include("Score").First(item => item.ShopId == shopid);
            slog.ScoreId = shop.ScoreId;

            shop.Score.AddScore(score, score, score);

            db.ScoreLogSet.Add(slog);
            db.SaveChanges();

            return Convert.ToInt32(shop.Score.GetAverage());
        }
    }
}
