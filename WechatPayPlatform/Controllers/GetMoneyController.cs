using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WechatPayPlatform.Models;

namespace WechatPayPlatform.Controllers
{
    public class GetMoneyController : Controller
    {
        //
        // GET: /GetMoney/

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult PayForMachineIndex(string code)
        {
            var help = new PayInfoController();
            var openid = help.GetOpenidByCode(code);

            ViewBag.openid = openid;
            ViewBag.appid = System.Configuration.ConfigurationManager.AppSettings["appid"];
            ViewBag.isJsDebug = System.Configuration.ConfigurationManager.AppSettings["IsJsDebug"];
            return View();
        }

        [HttpPost]
        public ActionResult PayForMachineIndex(int mid, string openid)
        {
            var db = new ModelContext();
            var m = db.MachineSet.Find(mid);

            Response.Redirect(System.Configuration.ConfigurationManager.AppSettings["baseurl"] + "/GetMoney/Index?openid=" + openid + "&mname" + m.Name + "&mid" + mid);
            return null;
        }



        [HttpGet]
        public ActionResult Pay(string billnumber)
        {
            var db = new ModelContext();
            Bill bill = null;

            bill = db.ComeBillSet.Include("User").FirstOrDefault(item => item.innerNumber == billnumber);
            if (bill.Status != ComeBillStatus.ToPay)
            {
                return RedirectToAction("IndexWithOpenid", "BillInfo", new { openid = bill.User.OpenId });
            }
            if (bill.Count > bill.User.Balance)
            {
                string url = string.Format(
                "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect",
                System.Configuration.ConfigurationManager.AppSettings["appid"],
                System.Configuration.ConfigurationManager.AppSettings["baseurl"] + "/Pay/Index",
                "cbill;您此次消费" + bill.Count + "元，还需充值" + (bill.Count - bill.User.Balance) + "元;" + bill.innerNumber);

                Response.Redirect(url);

            }

            return View(bill);
        }

        [HttpPost]
        public ActionResult Pay(FormCollection values)
        {
            var billnumber = values["billnumber"];

            var db = new ModelContext();
            ComeBill bill = db.ComeBillSet.Include("User").FirstOrDefault(item => item.innerNumber == billnumber);

            bill.User.Balance -= bill.Count;
            bill.Status = ComeBillStatus.Finish;
            bill.IsSuccess = true;
            bill.PayDate = DateTime.Now;
            db.SaveChanges();

            return RedirectToAction("IndexWithOpenid", "BillInfo", new { openid = bill.User.OpenId });
        }

        [HttpGet]
        public ActionResult Jump(string url, string appid)
        {
            return View(new
            {
                url = url,
                appid = appid
            });
        }
    }
}
