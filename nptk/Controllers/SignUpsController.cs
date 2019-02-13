﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using nptk.Models;

namespace nptk.Controllers
{
    [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.TourID = new SelectList(db.Tours, "TourId", "Title");
            ViewBag.UserID = new SelectList(db.Users, "Id", "FullName");
            return View();
        }

        // POST: SignUps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "TourID,UserID")] SignUp signUp)
        {
            if (ModelState.IsValid)
            {
                db.SignUps.Add(signUp);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TourID = new SelectList(db.Tours, "TourId", "Title", signUp.TourID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "FullName", signUp.UserID);
            return View(signUp);
        }

        // GET: SignUps/Edit/5
        [Authorize(Roles = "Admin")]
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
            ViewBag.UserID = new SelectList(db.Users, "Id", "FullName", signUp.UserID);
            return View(signUp);
        }

        // POST: SignUps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
