using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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

        //
        // GET: /GetMoney/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /GetMoney/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /GetMoney/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /GetMoney/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /GetMoney/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /GetMoney/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /GetMoney/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
