using System;
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
            try
            {
                if (ModelState.IsValid)
                {
                    db.SignUps.Add(signUp);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Nem sikerült a létrehozás. Próbáld újra, s ha nem megy, keresd az adminisztrátort!");
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
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditPost(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var signupToUpdate = db.SignUps.Find(Id);
            if (TryUpdateModel(signupToUpdate, "",
               new string[] { "TourID", "UserID" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Nem sikerült a mentés. Próbáld újra – ha úgy se működik, adminisztrátori segítség kell!");
                }
            }
            return View(signupToUpdate);
        }

        // GET: SignUps/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Sikertelen a törlés! Próbáld újra, s ha továbbra is fennáll a probléma, keresd az adminisztrátort!";
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
            try
            {
                SignUp signUp = db.SignUps.Find(id);
                db.SignUps.Remove(signUp);
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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
