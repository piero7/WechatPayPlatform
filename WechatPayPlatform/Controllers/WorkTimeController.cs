using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WechatPayPlatform.Models;

namespace WechatPayPlatform.Controllers
{
    public class WorkTimeController : Controller
    {
        //
        // GET: /WorkTime/

        [HttpGet]
        public ActionResult Index(int adminid)
        {
            return View();
        }


        public ActionResult Index(FormCollection values)
        {
            var adminid = int.Parse(values["adminid"]);
            var content = values["content"];

            var db = new ModelContext();
            var station = db.StationSet.FirstOrDefault(item => (item.AdministratorId ?? 0) == adminid);
            var workTime = db.WorkTimeSet.FirstOrDefault(item => item.StationId == station.StationId);
            workTime.LastEditTime = DateTime.Now;
            workTime.Content = content;
            db.SaveChanges();

            return View();
        }
    }
}
