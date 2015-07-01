using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WechatPayPlatform.Models;

namespace WechatPayPlatform.Controllers
{
    public class ComeBillController : Controller
    {
        //
        // GET: /ComeBill/

        public ActionResult Index(string code)
        {
            var helper = new PayInfoController();
            //if (code == null)
            //{
            //    return View((object)"testtest");
            //}
            var openid = helper.GetOpenidByCode(code);

            return View((object)openid);
        }

        [HttpGet]
        public ActionResult Success()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Fail()
        {
            return View();
        }

        //public ActionResult Index(string openid, string station, string carnumber, string endtime, string phone, string desc)
        //{
        //    return View("Seccess");
        //}
        [HttpPost]
        public ActionResult Index(FormCollection values)
        {
            string location = values["station"];
            string carnumber = values["carnumber"];
            string time = values["endtime"];
            string phone = values["phone"];
            string desc = values["desc"];
            string openid = values["openid"];

            if (string.IsNullOrEmpty(openid) || openid.Contains("errcode"))
            {
                return RedirectToAction("Fail");
            }
            DateTime starttime = DateTime.Today;
            if (time.Contains("明天"))
            {
                if (time.Split(new string[] { "明天" }, StringSplitOptions.None).Count() == 3)
                {
                    starttime = starttime.AddDays(1);
                }
                time = time.Replace("明天", "");
            }
            starttime = starttime.AddHours(Convert.ToInt32(time.Replace(":00", "").Split(new char[] { '-' })[0]));
            var db = new ModelContext();

            //用户信息
            var user = db.WechatUserSet.FirstOrDefault(item => item.OpenId == openid);
            if (user == null)
            {
                user = new WechatUser
                {
                    OpenId = openid,
                    SubscribeTime = DateTime.Now,
                    Balance = 0,
                    subscribe = true,
                    PhoneNumber = phone,
                };
                db.WechatUserSet.Add(user);
                db.SaveChanges();

                Helper.GetUserInfo(user);

            }
            if (user.PhoneNumber != phone)
            {
                user.PhoneNumber = phone;
            }

            //车辆信息
            var car = db.CarInfoSet.FirstOrDefault(item => item.CarNumber == carnumber
                && item.LocationDescribe == location
                && item.Describe == desc
                && item.UserId == user.UserId);
            if (car == null)
            {
                car = new CarInfo
                {
                    CreateDate = DateTime.Now,
                    CarNumber = carnumber,
                    Describe = desc,
                    LocationDescribe = location,
                    UserId = user.UserId,
                };
                db.CarInfoSet.Add(car);
            }
            car.LastUseDate = DateTime.Now;
            db.SaveChanges();

            //订单号
            string billnumner = "c" + SocketController.GetHexString(DateTime.Now.Year - 2015) + SocketController.GetHexString(DateTime.Now.Month) + DateTime.Now.ToString("ddHHmmss") + user.OpenId.Substring(openid.Length - 6, 5);

            var adminid = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["workerid"]);

            //noctice worker
            Helper.SendWorkMessage(adminid, location, car, desc, time);

            var bill = new ComeBill
                        {
                            Address = location,
                            PhoneNumber = phone,
                            CreateDate = DateTime.Now,
                            Describe = desc,
                            StartTime = starttime,
                            EndTime = starttime.AddHours(1),
                            CarNumber = carnumber,
                            Status = ComeBillStatus.ToConfirm,
                            innerNumber = billnumner,
                            UserId = user.UserId,
                            AdminId = adminid,
                            Count = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["billPrice"])
                        };
            //if (openid == "testtest")
            //{
            //    bill.Remarks = "This is a test bill !";
            //}

            db.ComeBillSet.Add(bill);
            db.SaveChanges();

            return RedirectToAction("IndexWithOpenid", "BillInfo", new { openid = openid });
        }

        [HttpGet]
        public ActionResult ConfirmBill(int adminId, string billNumber)
        {
            var db = new ModelContext();
            Administrator admin = db.AdminSet.Find(adminId);
            var bill = db.ComeBillSet.FirstOrDefault(item => item.innerNumber == billNumber);
            ViewData.Add("adminid", adminId);

            return View(bill);
        }

        [HttpPost]
        public ActionResult ConfirmBill(FormCollection valuse)
        {
            var adminid = Convert.ToInt32(valuse["adminid"]);
            var billnumber = valuse["billnumber"];

            var db = new ModelContext();
            var bill = db.ComeBillSet.FirstOrDefault(item => item.innerNumber == billnumber);
            bill.Status = ComeBillStatus.Working;
            db.SaveChanges();

            return RedirectToAction("WorkerBillInfo", "BillInfo", new { adminid = adminid });
        }

        [HttpPost]
        public ActionResult FinishWork(FormCollection values)
        {
            var adminid = values["adminid"];
            var billnumber = values["billnumber"];

            var db = new ModelContext();
            var bill = db.ComeBillSet.Include("User").FirstOrDefault(item => item.innerNumber == billnumber);
            bill.Status = ComeBillStatus.ToPay;
            bill.FinishTime = DateTime.Now;
            db.SaveChanges();

            //TODO Notice user
            Helper.SendFinishCleanMsg(bill);

            return RedirectToAction("WorkerBillInfo", "BillInfo", new { adminid = adminid });
        }

        [HttpPost]
        public ActionResult CancelBill(FormCollection valuse)
        {
            var adminid = Convert.ToInt32(valuse["adminid"]);
            var billnumber = valuse["billnumber"];


            var db = new ModelContext();
            var bill = db.ComeBillSet.FirstOrDefault(item => item.innerNumber == billnumber);
            bill.Status = ComeBillStatus.Cancel;
            bill.FinishTime = DateTime.Now;

            db.SaveChanges();
            return RedirectToAction("WorkerBillInfo", "BillInfo", new { adminid = adminid });
        }

    }
}
