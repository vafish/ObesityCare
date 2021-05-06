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
using Microsoft.SqlServer.Management.Smo;
using System.Runtime.Remoting.Contexts;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace ObesityCare.Controllers
{
    public class RecordsController : Controller
    {
        private ObesityCare_dbRecords db = new ObesityCare_dbRecords();
        private StarModel db_star = new StarModel();
        // GET: Records
        [Authorize]
        public ActionResult Index()
        {

            string id = User.Identity.GetUserId();
            var records = from r in db.Record select r;
            records = records.Where(r => r.UserId.Equals(id));          

            var stars = from r in db_star.Star select r;
            var starlst = stars.Where(r => r.UserId.Equals(id)).ToList();

            if (starlst.Count == 0)
            {
                if (ModelState.IsValid)
                {
                    Star newStar = new Star();
                    newStar.Amount = 0;
                    newStar.UserId = id;
                    newStar.Id = 0;

                    if (db_star.Star.ToList().Count == 0)
                    {
                        db_star.Star.Add(newStar);
                        db_star.SaveChanges();                      
                    }
                    else
                    {
                        var last = db_star.Star.ToList()[db_star.Star.ToList().Count - 1];
                        newStar.Id = last.Id + 1;

                        db_star.Star.Add(newStar);
                        db_star.SaveChanges();
                    }
                   
                    ViewData["totalstar"] = 0;
                }
            }
            else
            {
                ViewData["totalstar"] = starlst.First().Amount;
            }




            return View(records.ToList());
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
                record.UserId = User.Identity.GetUserId();
                if (db.Record.ToList().Count == 0)
                {
                    db.Record.Add(record);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                var last = db.Record.ToList()[db.Record.ToList().Count - 1];
                record.Id = last.Id + 1;
                
                db.Record.Add(record);
                db.SaveChanges();
                var star = from r in db_star.Star select r;
                var u_star = star.Where(r => r.UserId.Equals(record.UserId)).ToList().First();
                u_star.Amount += record.Star;
                db_star.Entry(u_star).State = EntityState.Modified;
                db_star.SaveChanges();
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
