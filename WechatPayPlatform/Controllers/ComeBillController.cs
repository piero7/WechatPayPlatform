using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WechatPayPlatform.Controllers
{
    public class ComeBillController : Controller
    {
        //
        // GET: /ComeBill/

        public ActionResult Index(string openid)
        {
            return View();
        }

    }
}
