using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WechatPayPlatform.Models;

namespace WechatPayPlatform.Controllers
{
    public class BillInfoController : Controller
    {
        //
        // GET: /BillInfo/

        public ActionResult Index(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                //TODO: code 为空

                //debug
                // return RedirectToAction("IndexWithOpenid", "BillInfo", (object)"testtest");
            }
            var helper = new PayInfoController();
            var openid = helper.GetOpenidByCode(code);

            return RedirectToAction("IndexWithOpenid", "BillInfo", new { openid = openid });

        }

        public ActionResult IndexWithOpenid(string openid)
        {
            if (string.IsNullOrEmpty(openid))
            {
                return RedirectToAction("Fail", "ComeBill");
            }
            var db = new ModelContext();
            var user = db.WechatUserSet.FirstOrDefault(item => item.OpenId == openid);
            if (user == null)
            {
                //TODO: 没找到user 

                //debug
                // user = db.WechatUserSet.FirstOrDefault(item => item.OpenId == "testtest");
                user = new WechatUser
                {
                    OpenId = openid,
                    SubscribeTime = DateTime.Now,
                    Balance = 0,
                    subscribe = true,
                    Sex = 0
                };
                db.WechatUserSet.Add(user);
                db.SaveChanges();

                Helper.GetUserInfo(user);
            }

            var dic = new Dictionary<string, IEnumerable<Bill>>();
            if (!db.ComeBillSet.Any(item => item.UserId == user.UserId))
            {
                dic.Add("come", new List<ComeBill>());
            }
            else
            {
                dic.Add("come", db.ComeBillSet.Where(item => item.UserId == user.UserId).OrderByDescending(item => item.CreateDate).AsEnumerable());
            }

            if (!db.MachineBillSet.Any(item => item.UserId == user.UserId))
            {
                dic.Add("machine", new List<MachineBill>());
            }
            else
            {
                dic.Add("machine", db.MachineBillSet.Where(item => item.UserId == user.UserId).OrderByDescending(item => item.CreateDate).AsEnumerable());
            }

            if (!db.ShopBillSet.Any(item => item.UserId == user.UserId))
            {
                dic.Add("shop", new List<ShopBill>());
            }
            else
            {
                dic.Add("shop", db.ShopBillSet.Where(item => item.UserId == user.UserId).OrderByDescending(item => item.CreateDate).AsEnumerable());
            }

            //dic.Add("recharge", db.RechargeBillSet);
            //return View("BillInfo/Inedx", dic);
            return View(dic);
        }

        [HttpGet]
        public ActionResult WorkerBillInfo(int? adminid)
        {
            var db = new ModelContext();
            var dic = new Dictionary<string, IEnumerable<ComeBill>>();
       
            //TODO: check adminid


            dic.Add("confirm", db.ComeBillSet.Where(item => item.AdminId == adminid
                && item.Status == ComeBillStatus.ToConfirm)
                .OrderByDescending(item => item.CreateDate)
                .AsEnumerable());
            dic.Add("working", db.ComeBillSet.Where(item => item.AdminId == adminid
                && item.Status == ComeBillStatus.Working)
                .OrderByDescending(item => item.CreateDate)
                .AsEnumerable());
            dic.Add("finish", db.ComeBillSet.Where(item => item.AdminId == adminid
                && item.Status != ComeBillStatus.ToConfirm
                && item.Status != ComeBillStatus.Working)
                .OrderByDescending(item => item.CreateDate)
                .AsEnumerable());
            ViewData.Add("adminid", adminid);
            return View(dic);
        }

        [HttpGet]
        public ActionResult GetWorkInfo(int? adminid)
        {
            ViewBag.adminid = adminid.Value;
            return View();

            //var db = new ModelContext();
            ////int year = 2015;
            //// int mou = 7;
            //ViewBag.year = year;
            //ViewBag.mou = mou;

            //int count = 0;
            ////var startTime = new DateTime(year , mou,1);
            ////var endTime  = new DateTime();
            ////if(mou == 12){
            ////    endTime = new DateTime(year+1,1,1);
            ////}
            ////else{
            ////    endTime = new DateTime(year,mou +1,1);
            ////}

            ////获取当月新增有限用户数 
            ////TODO: 逻辑待验证
            //var userIdList = db.ComeBillSet.Where(item => item.Status == ComeBillStatus.Finish).Select(item => item.UserId).Distinct().ToList();
            //foreach (var userId in userIdList)
            //{
            //    var firbill = db.ComeBillSet.Where(item => item.UserId == userId).OrderBy(item => item.CreateDate).FirstOrDefault();
            //    if (firbill.CreateDate.Value.Month == mou && firbill.CreateDate.Value.Year == year)
            //    {
            //        count++;
            //    }
            //}
            ////ViewBag.usercount = db.ComeBillSet.Where(item => item.Status == ComeBillStatus.Finish).Select(item => item.UserId).Distinct().Count();
            //ViewBag.usercount = count;
            //// = Helper.GetUserCount() - 10;
            //ViewBag.tocom = db.ComeBillSet.Count(item => item.Status == ComeBillStatus.ToConfirm);
            //ViewBag.todo = db.ComeBillSet.Count(item => item.Status == ComeBillStatus.Working);
            //ViewBag.topay = db.ComeBillSet.Count(item => item.Status == ComeBillStatus.ToPay);
            //ViewBag.finish = db.ComeBillSet.Count(item => item.Status == ComeBillStatus.Finish);
            //ViewBag.cancel = db.ComeBillSet.Count(item => item.Status == ComeBillStatus.Cancel);

            //return View();
        }

        [HttpGet]
        public ActionResult GetWorkInfoPart(int? adminid, int? year, int? mou)
        {
            var db = new ModelContext();
            //int year = 2015;
            // int mou = 7;
          //  ViewBag.year = year;
         //   ViewBag.mou = mou;

            int count = 0;
            //var startTime = new DateTime(year , mou,1);
            //var endTime  = new DateTime();
            //if(mou == 12){
            //    endTime = new DateTime(year+1,1,1);
            //}
            //else{
            //    endTime = new DateTime(year,mou +1,1);
            //}

            //获取当月新增有限用户数 
            //TODO: 逻辑待验证
            var userIdList = db.ComeBillSet.Where(item => item.Status == ComeBillStatus.Finish).Select(item => item.UserId).Distinct().ToList();
            foreach (var userId in userIdList)
            {
                var firbill = db.ComeBillSet.Where(item => item.UserId == userId).OrderBy(item => item.CreateDate).FirstOrDefault();
                if (firbill.FinishTime.Value.Month == mou && firbill.FinishTime.Value.Year == year)
                {
                    count++;
                }
            }
            ViewBag.usercount = count;
            ViewBag.tocom = db.ComeBillSet.Count(item => item.Status == ComeBillStatus.ToConfirm);
            ViewBag.todo = db.ComeBillSet.Count(item => item.Status == ComeBillStatus.Working);
            ViewBag.topay = db.ComeBillSet.Count(item => item.Status == ComeBillStatus.ToPay);
            ViewBag.finish = db.ComeBillSet.Count(item => item.Status == ComeBillStatus.Finish);
            ViewBag.cancel = db.ComeBillSet.Count(item => item.Status == ComeBillStatus.Cancel);

            return View();
        }

    }
}

