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
        public ActionResult Pay(string billnumber)
        {
            var db = new ModelContext();
            Bill bill = null;

            bill = db.ComeBillSet.Include("User").FirstOrDefault(item => item.innerNumber == billnumber);
            if (bill.Count > bill.User.Balance)
            {
                return RedirectToAction("Jump", new
                {
                    url =
                        (object)string.Format(
                        "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base#wechat_redirect",
                        System.Configuration.ConfigurationManager.AppSettings["appid"],
                        string.Format(
                        System.Configuration.ConfigurationManager.AppSettings["baseurl"] + "/Pay/Index")
                        )
                });
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
            db.SaveChanges();

            return RedirectToAction("IndexWithOpenid", "BillInfo", (object)bill.User.OpenId);
        }

        [HttpGet]
        public ActionResult Jump(string url)
        {
            return View((object)url);
        }

    }
}
