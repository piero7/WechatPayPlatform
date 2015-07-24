using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WechatPayPlatform.Models;

namespace WechatPayPlatform.Controllers
{
    public class ShopController : Controller
    {
        //
        // GET: /Default1/

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddInfo()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddInfo(FormCollection values)
        {
            var db = new ModelContext();
            var shop = new Shop
            {
                Address = values["address"],
                Name = values["name"],
                LocationX = Double.Parse(values["x"]),
                LocationY = double.Parse(values["y"]),
                Phone = values["phone"],
                Minimum = double.Parse(values["minimun"]),
                ShopHours = values["startt"] + ":00-" + values["endt"] + ":00",
                ImgPath = values["filename"],
            };
            db.ShopSet.Add(shop);

            var score = new Score
            {
                CountA = 1,
                CountB = 1,
                CountC = 1,
                ScoreA = 3,
                ScoreB = 3,
                ScoreC = 3
            };
            db.ScoreSet.Add(score);

            db.SaveChanges();
            shop.ScoreId = score.ScoreId;
            db.SaveChanges();


            return View();
        }

        public ActionResult Upload(HttpPostedFileBase Filedata)
        {
            // 如果没有上传文件
            if (Filedata == null ||
                string.IsNullOrEmpty(Filedata.FileName) ||
                Filedata.ContentLength == 0)
            {
                return this.HttpNotFound();
            }

            // 保存到 ~/upload 文件夹中，名称不变
            var extName = System.IO.Path.GetExtension(Filedata.FileName);
        getfileName: string filename = DateTime.Now.GetHashCode().ToString() + '.' + extName;

            //string filename = System.IO.Path.GetFileName(Filedata.FileName);
            string virtualPath = string.Format("~/upload/{0}", filename);

            // 转换成绝对路径
            string path = this.Server.MapPath(virtualPath);

            if (System.IO.File.Exists(path))
            {
                goto getfileName;
            }

            Filedata.SaveAs(path);
            return this.Json(new { fn = filename });
        }

        [HttpGet]
        public ActionResult ShopDetail(int shopid)
        {
            var db = new ModelContext();
            var shop = db.ShopSet.Include("Score").FirstOrDefault(item => item.ShopId == shopid);

            return View(shop);
        }

    }
}
