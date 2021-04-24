using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ObesityCare.Models;
using Microsoft.AspNet.Identity;

namespace ObesityCare.Controllers
{
    public class RecordsController : Controller
    {
        private ObesityCare_dbRecords db = new ObesityCare_dbRecords();

        // GET: Records
        [Authorize]
        public ActionResult Index()
        {
            //var AllRecord = db.Record;

            //for (int i = 0; i < AllRecord.Remove; i++)
            //{
            //    if (!AllRecord[i].UserId.Equals(User.Identity.GetUserId()))
            //    {
            //        AllRecord.RemoveAt(i);
            //    }               
            //}
            string id = User.Identity.GetUserId();
            var records = from r in db.Record select r;
            records = records.Where(r => r.UserId.Equals(id));

            return View(records.ToList());
        }


        [HttpPost]
        public JsonResult GetEmpName(string Prefix)
        {

            var countries = new List<String>(new string[]{
                            "Afghanistan",
                            "Albania",
                            "Algeria",
                            "AmericanSamoa",
                            "Andorra",
                            "Angola",
                            "Anguilla",
                            "Antigua&Barbuda",
                            "Argentina",
                            "Armenia",
                            "Aruba",
                            "Australia",
                            "Austria" });
            return Json(countries, JsonRequestBehavior.AllowGet);
        }

        // GET: Records/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Record.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // GET: Records/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Records/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Location,Duration,Star,UserId")] Record record)
        {
            if (ModelState.IsValid)
            {

                db.Record.Find(db.Record.Count());
                record.Id = db.Record.Count() + 5;
                record.UserId = User.Identity.GetUserId();
                db.Record.Add(record);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(record);
        }

        // GET: Records/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Record.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // POST: Records/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Location,Duration,Star")] Record record)
        {
            if (ModelState.IsValid)
            {
                db.Entry(record).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(record);
        }

        // GET: Records/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Record record = db.Record.Find(id);
            if (record == null)
            {
                return HttpNotFound();
            }
            return View(record);
        }

        // POST: Records/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Record record = db.Record.Find(id);
            db.Record.Remove(record);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
