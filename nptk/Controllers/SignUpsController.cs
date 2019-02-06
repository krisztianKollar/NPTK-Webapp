using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using nptk.Models;

namespace nptk.Controllers
{
    public class SignUpsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: SignUps
        public ActionResult Index()
        {
            var signUps = db.SignUps.Include(s => s.Tour);
            return View(signUps.ToList());
        }

        // GET: SignUps/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SignUp signUp = db.SignUps.Find(id);
            if (signUp == null)
            {
                return HttpNotFound();
            }
            return View(signUp);
        }

        // GET: SignUps/Create
        public ActionResult Create()
        {
            ViewBag.TourID = new SelectList(db.Tours, "TourId", "Title");
            return View();
        }

        // POST: SignUps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SignUpID,TourID,UserID")] SignUp signUp)
        {
            if (ModelState.IsValid)
            {
                db.SignUps.Add(signUp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TourID = new SelectList(db.Tours, "TourId", "Title", signUp.TourID);
            return View(signUp);
        }

        // GET: SignUps/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SignUp signUp = db.SignUps.Find(id);
            if (signUp == null)
            {
                return HttpNotFound();
            }
            ViewBag.TourID = new SelectList(db.Tours, "TourId", "Title", signUp.TourID);
            return View(signUp);
        }

        // POST: SignUps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SignUpID,TourID,UserID")] SignUp signUp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(signUp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TourID = new SelectList(db.Tours, "TourId", "Title", signUp.TourID);
            return View(signUp);
        }

        // GET: SignUps/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SignUp signUp = db.SignUps.Find(id);
            if (signUp == null)
            {
                return HttpNotFound();
            }
            return View(signUp);
        }

        // POST: SignUps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SignUp signUp = db.SignUps.Find(id);
            db.SignUps.Remove(signUp);
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
