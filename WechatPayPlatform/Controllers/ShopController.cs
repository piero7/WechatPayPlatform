using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WechatPayPlatform.Controllers
{
    public class ShopController : Controller
    {
        //
        // GET: /Default1/

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
        getfileName: string filename = DateTime.Now.GetHashCode().ToString();

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

    }
}
