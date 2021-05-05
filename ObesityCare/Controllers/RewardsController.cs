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
    public class RewardsController : Controller
    {
        private RewardModel db = new RewardModel();
        private StarModel db_star = new StarModel();
        private ClaimModel db_claim = new ClaimModel();

        // GET: Rewards
        [Authorize]
        public ActionResult Index()
        {
            string usrid = User.Identity.GetUserId();
           
            var stars = from r in db_star.Star select r;
            var starlst = stars.Where(r => r.UserId.Equals(usrid)).ToList();
           
            if (starlst.Count == 0 )
            {
                if (ModelState.IsValid)
                {
                    Star newStar = new Star();
                    newStar.Amount = 0;
                    newStar.UserId = usrid;
                    newStar.Id = db_star.Star.Count() * 5;
                    db_star.Star.Add(newStar);
                    db_star.SaveChanges();
                    ViewData["totalstar"] = 0;


                }
            }
            else
            {
                ViewData["totalstar"] = starlst.First().Amount;
            }
            
            return View(db.Rewards.ToList());
        }

        // GET: Rewards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return HttpNotFound();
            }
            return View(reward);
        }
       


        // GET: Rewards/Create
        public ActionResult Create()
        {
           
            return View();
        }

        // POST: Rewards/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Cost")] Reward reward)
        {
          
            if (ModelState.IsValid)
            {
                if (db.Rewards.ToList().Count == 0)
                {
                    db.Rewards.Add(reward);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                var last = db.Rewards.ToList()[db.Rewards.ToList().Count - 1];
                reward.Id = last.Id + 1;

                db.Rewards.Add(reward);
                db.SaveChanges();           

                return RedirectToAction("Index");
            }

            return View(reward);
        }

        // GET: Rewards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return HttpNotFound();
            }
            return View(reward);
        }

        // POST: Rewards/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Cost")] Reward reward)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reward).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(reward);
        }


        // GET: Rewards/Claim/5
        public ActionResult Claim(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reward reward = db.Rewards.Find(id);
            string usrid = User.Identity.GetUserId();
            var stars = from r in db_star.Star select r;
            var starlst = stars.Where(r => r.UserId.Equals(usrid)).ToList();
            ViewData["totalstar"] = starlst.First().Amount;
            if (reward == null)
            {
                return HttpNotFound();
            }
            return View(reward);
        }


        // POST: Rewards/Claim/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性；有关
        // 更多详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Claim([Bind(Include = "Id,Name,Cost")] Reward reward)
        {
            if (ModelState.IsValid)
            {
                string usrid = User.Identity.GetUserId();
                var stars = from r in db_star.Star select r;
                var u_star = stars.Where(r => r.UserId.Equals(usrid)).ToList().First();
                var id = reward.Id;
                Reward m_reward = db.Rewards.Find(id);
                if (u_star.Amount >= m_reward.Cost)
                {
                    u_star.Amount -= m_reward.Cost;
                    db_star.Entry(u_star).State = EntityState.Modified;
                    db_star.SaveChanges();
                    ViewData["totalstar"] = u_star.Amount;
                    ViewBag.Message = String.Format("The has been exchanged successfully!", DateTime.Now.ToString());
                    //System.Windows.Forms.MessageBox.Show("");
                    Claim m_claim = new Claim();
                    m_claim.Date = DateTime.Now;
                    m_claim.UserId = usrid;
                    m_claim.Item = m_reward.Name;
                    db_claim.Claim.Add(m_claim);
                    db_claim.SaveChanges();

                    return View(m_reward);
                }
                else
                {
                    ViewData["totalstar"] = u_star.Amount;
                    ViewBag.Message = String.Format("You need more stars to exchange ", reward.Name);
                    //System.Windows.Forms.MessageBox.Show("");
                    return View(m_reward);
                }
                
            }
            return RedirectToAction("Index");
        }

        // GET: Rewards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reward reward = db.Rewards.Find(id);
            if (reward == null)
            {
                return HttpNotFound();
            }
            return View(reward);
        }

        


        // POST: Rewards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Reward reward = db.Rewards.Find(id);
            db.Rewards.Remove(reward);
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
