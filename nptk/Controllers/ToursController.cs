using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using nptk.Models;

namespace nptk.Controllers
{
    [Authorize]
    public class ToursController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tours
        public ActionResult Index(string sortOrder)
        {
            ViewBag.TitleSortParm = sortOrder == "title" ? "title_desc" : "title";
            ViewBag.DistanceSortParm = sortOrder == "distance" ? "distance_desc" : "distance";
            ViewBag.ClimbSortParm = sortOrder == "climb" ? "climb_desc" : "climb";
            ViewBag.DateSortParm = string.IsNullOrEmpty(sortOrder) ? "" : "date";
            var tours = from t in db.Tours
                           select t;
            switch (sortOrder)
            {
                case "title":
                    tours = tours.OrderBy(t => t.Title);
                    break;
                case "title_desc":
                    tours = tours.OrderByDescending(t => t.Title);
                    break;
                case "distance":
                    tours = tours.OrderBy(t => t.Distance);
                    break;
                case "distance_desc":
                    tours = tours.OrderByDescending(t => t.Distance);
                    break;
                case "climb":
                    tours = tours.OrderBy(t => t.Climb);
                    break;
                case "climb_desc":
                    tours = tours.OrderByDescending(t => t.Climb);
                    break;
                case "date":
                    tours = tours.OrderBy(t => t.Date);
                    break;
                default:
                    tours = tours.OrderByDescending(t => t.Date);
                    break;
            }

            return View(tours.ToList());
        }

        // GET: Tours/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // GET: Tours/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tours/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Title,Date,Track,Distance,Climb")] Tour tour)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Tours.Add(tour);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Nem sikerült a létrehozás. Próbáld újra, s ha nem megy, keresd az adminisztrátort!");
            }
            return View(tour);
        }

        // GET: Tours/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // POST: Tours/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditPost(int? TourId)
        {
            if (TourId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var tourToUpdate = db.Tours.Find(TourId);
            if (TryUpdateModel(tourToUpdate, "",
               new string[] { "Title", "Date", "Track", "Distance", "Climb" }))
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
            return View(tourToUpdate);

        }

        // GET: Tours/Delete/5
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
            Tour tour = db.Tours.Find(id);
            if (tour == null)
            {
                return HttpNotFound();
            }
            return View(tour);
        }

        // POST: Tours/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Tour tour = db.Tours.Find(id);
                db.Tours.Remove(tour);
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
