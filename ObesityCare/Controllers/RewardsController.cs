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
        Reward r1 = new Reward();
        Reward r2 = new Reward();
        Reward r3 = new Reward();
        Reward r4 = new Reward();
        Reward r5 = new Reward();

        // GET: Rewards
        [Authorize]
        public ActionResult Index()
        {
            string usrid = User.Identity.GetUserId();
           
            var stars = from r in db_star.Star select r;
            var starlst = stars.Where(r => r.UserId.Equals(usrid)).ToList();
            var rewards = from r in db.Rewards select r;
            rewards = rewards.Where(r => r.UserId.Equals(usrid));

            if (rewards.ToList().Count == 0) {
                var last = db.Rewards.ToList()[db.Rewards.ToList().Count - 1];
                r1.UserId = usrid;
                r1.Id = last.Id + 1;
                r1.Cost = 10;
                r1.Name = "A pack of trading cards";
                db.Rewards.Add(r1);
                r2.UserId = usrid;
                r2.Id = last.Id + 2;
                r2.Cost = 20;
                r2.Name = "Two movie tickets";
                db.Rewards.Add(r2);
                r3.UserId = usrid;
                r3.Id = last.Id + 3;
                r3.Cost = 30;
                r3.Name = "A family beach trip";
                db.Rewards.Add(r3);
                r4.UserId = usrid;
                r4.Id = last.Id + 4;
                r4.Cost = 40;
                r4.Name = "A family trip to Luna Park";
                db.Rewards.Add(r4);
                r5.UserId = usrid;
                r5.Id = last.Id + 5;
                r5.Cost = 50;
                r5.Name = "A Lego Set";
                db.Rewards.Add(r5);
                db.SaveChanges();
            }


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
            
            return View(rewards.ToList());
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

            reward.UserId = User.Identity.GetUserId();
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
                    //System.Windows.Forms.MessageBox.Show("");
                    Claim m_claim = new Claim();
                    m_claim.Date = DateTime.Now;
                    m_claim.UserId = usrid;
                    m_claim.Item = m_reward.Name;
                    if (db_claim.Claim.ToList().Count == 0)
                    {                      
                        db_claim.Claim.Add(m_claim);
                        db_claim.SaveChanges();
                    }
                    else
                    {
                        var last = db_claim.Claim.ToList()[db_claim.Claim.ToList().Count - 1];
                        m_claim.Id = last.Id + 1;
                        db_claim.Claim.Add(m_claim);
                        db_claim.SaveChanges();
                    }
                    ViewBag.Message = String.Format("The has been exchanged successfully!", DateTime.Now.ToString());
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
