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

            string openid = "";
            if (code == null)
            {
                openid = "oKCRrs9abChCGga9FWDRNER-xcTQ";
            }
            else
            {

                var help = new PayInfoController();
                openid = help.GetOpenidByCode(code);
            }

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
            ComeBill bill = null;

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
            int s1 = int.Parse(values["s1"]);
            int s2 = int.Parse(values["s2"]);
            int s3 = int.Parse(values["s3"]);

            int comeScid = int.Parse(System.Configuration.ConfigurationManager.AppSettings["comebillScoreId"]);

            var db = new ModelContext();

            ComeBill bill = db.ComeBillSet.Include("User").FirstOrDefault(item => item.innerNumber == billnumber);
            //Score
            var comeSc = db.ScoreSet.Find(comeScid);
            comeSc.AddScore(s1, s2, s3);
            db.ScoreLogSet.Add(new ScoreLog
            {
                BillNumber = bill.BillId,
                CreateDate = DateTime.Now,
                s1 = s1,
                s2 = s2,
                s3 = s3,
                ScoreId = comeScid,
                Userid = bill.UserId,
            });


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
