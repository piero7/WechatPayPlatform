using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WechatPayPlatform.Controllers
{
    public class PayController : Controller
    {
        //
        // GET: /Pay/

        [HttpGet]
        public ActionResult Index(string wStr, string from, string param, string state)
        {
            ViewBag.appid = System.Configuration.ConfigurationManager.AppSettings["appid"];
            ViewBag.isJsDebug = System.Configuration.ConfigurationManager.AppSettings["IsJsDebug"];

            if (string.IsNullOrEmpty(state))
            {
                ViewBag.wstr = wStr;
                ViewBag.from = from;
                ViewBag.param = param;
            }
            else
            {
                var ret = state.Split(new char[] { ';' });
                ViewBag.wstr = ret[1];
                ViewBag.from = ret[0];
                ViewBag.param = ret[2];
            }

            //var s = Helper.GetJsApiSignature(System.Configuration.ConfigurationManager.AppSettings["payurl"], "2nDgiWM7gCxhL8v0", "1420774989");

            // ViewBag.Message = s;


            return View();
        }

        [HttpPost]
        public ActionResult Index(FormCollection valuse)
        {
            var from = valuse["from"];
            var param = valuse["param"];

            if (from == "cbill")
            {
                return RedirectToAction("Pay", "GetMoney", new { billnumber = param });
            }
            else
            {
                string aurl = System.Configuration.ConfigurationManager.AppSettings["baseurl"] + "/BillInfo/Index";

                string url = string.Format(
               "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state={2}#wechat_redirect",
               System.Configuration.ConfigurationManager.AppSettings["appid"],
               System.Configuration.ConfigurationManager.AppSettings["baseurl"] + "/BillInfo/Index",
               "");
                Response.Redirect(url);

            }
            return null;
        }

        

    }
}
