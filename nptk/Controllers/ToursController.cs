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
using nptk.Helpers;
using System.Diagnostics;
using Microsoft.Ajax.Utilities;
using System.IO;

namespace nptk.Controllers
{
    [Authorize]
    public class ToursController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private GalleriesController galleriesController = new GalleriesController();

        // GET: Tours
        public ActionResult Index(string sortOrder, string filtTour)
        {
            ViewBag.Rb = filtTour;

            GetActualTour();
            ClearActiveBeforeActual();

            var tours = from t in db.Tours select t;
            if (filtTour == "all")
                tours = from t in db.Tours select t;
            if (filtTour == "next")
            {
                tours = from t in db.Tours
                        where t.Date > DateTime.Now
                        select t;
            }
            if (filtTour == "prev")
            {
                tours = from t in db.Tours
                        where t.Date < DateTime.Now
                        select t;
            }

            ViewBag.TitleSortParm = sortOrder == "title" ? "title_desc" : "title";
            ViewBag.DistanceSortParm = sortOrder == "distance" ? "distance_desc" : "distance";
            ViewBag.ClimbSortParm = sortOrder == "climb" ? "climb_desc" : "climb";
            //ViewBag.DateSortParm = string.IsNullOrEmpty(sortOrder) ? "" : "date";
            ViewBag.DateSortParm = sortOrder == "date" ? "date_desc" : "date"; 
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
                case "date_desc":
                    tours = tours.OrderByDescending(t => t.Date);
                    break;
                default :
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
            ViewBag.SignUp = 0;
            int UserId = Convert.ToInt32(User.Identity.GetUserId());
            if (db.SignUps.Include(x => x.TourID).Where(x => x.TourID == id && x.UserID == UserId).Count() > 0)
            {
                ViewBag.SignUp = 1;
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
        public ActionResult Create(TourViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Tour tour = new Tour
                    {
                        Title = model.Title,
                        Date = model.Date,
                        Track = model.Track,
                        Distance = model.Distance,
                        Climb = model.Climb,
                        About = model.About,
                        IsActive = model.IsActive
                    };


                    /*if (model.Image != null)
                    {
                        string extension = Path.GetExtension(model.Image.FileName);
                        tour.PosterPath = "poster" + tour.Date.ToString("yyyyMMdd").ToLower() + extension;
                        var path = Path.Combine(Server.MapPath("/Content/Posters/" + tour.PosterPath));
                        model.Image.SaveAs(path);
                    }*/
                    Debug.WriteLine(model.UploadedPics.Count());
                    if (model.UploadedPics.Count() != 0)
                    {
                        if (model.UploadedPics.Count() == 1)
                        {
                            foreach (var pic in model.UploadedPics)
                            {
                                string extension = Path.GetExtension(pic.FileName);
                                string PicPath = "poster" + tour.Date.ToString("yyyyMMdd").ToLower();
                                var path = Path.Combine(Server.MapPath("/Content/Posters/" + PicPath + extension));
                                pic.SaveAs(path);

                                Picture picture = new Picture
                                {
                                    Path = PicPath + extension,
                                    PicName = PicPath
                                };

                                db.Pictures.Add(picture);
                            }
                        }

                        if (model.UploadedPics.Count() > 1)
                        {
                            var picCount = 0;
                            foreach (var pic in model.UploadedPics)
                            {
                                picCount += 1;
                                string extension = Path.GetExtension(pic.FileName);
                                string PicPath = "tour" + tour.Date.ToString("yyyyMMdd_").ToLower() + picCount;
                                var path = Path.Combine(Server.MapPath("/Content/TourGallery/" + PicPath + extension));
                                pic.SaveAs(path);
                                Picture picture = new Picture
                                {
                                    Path = PicPath + extension,
                                    PicName = PicPath
                                };

                                db.Pictures.Add(picture);
                            }
                        }
                    }

                    db.Tours.Add(tour);
                    db.SaveChanges();
                    GetActualTour();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Nem sikerült a létrehozás. Próbáld újra, s ha nem megy, keresd az adminisztrátort!");
            }
            return View();
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
               new string[] { "Title", "Date", "Track", "Distance", "Climb", "About", "IsActive", "IsActual" }))
            {
                try
                {
                    db.SaveChanges();
                    GetActualTour();
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
                if (tour.Gallery != null)
                {
                    galleriesController.DeletePicFiles(tour.Gallery.GalleryID);
                }
                db.Tours.Remove(tour);
                GetActualTour();
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        // POST: Tours/SignUpTour 
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult SignUpTour(FormCollection form)
        {
            SignUp signUp = new SignUp();
            try
            {
                if (ModelState.IsValid)
                {
                    int TourId = Convert.ToInt32(form["TourId"]);
                    int UserId = Convert.ToInt32(User.Identity.GetUserId());
                    if (db.SignUps.Include(x => x.TourID).Where(x => x.TourID == TourId && x.UserID == UserId).Count() == 0)
                    {
                        signUp.TourID = TourId;
                        signUp.UserID = UserId;
                        db.SignUps.Add(signUp);
                        db.SaveChanges();
                        return Redirect(Request.UrlReferrer.PathAndQuery);
                    }
                    else
                    {
                        SignUp signUpToRemove = db.SignUps.SingleOrDefault(x => x.TourID == TourId && x.UserID == UserId);
                        db.SignUps.Remove(signUpToRemove);
                        db.SaveChanges();
                        return Redirect(Request.UrlReferrer.PathAndQuery);
                    }
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Nem sikerült a jelentkezés. Próbáld újra, s ha nem megy, keresd az adminisztrátort!");
            }
            return View();
        }

        public Tour GetActualTour()
        {
            if (db.Tours.Count() == 0)
            {
                ViewBag.TourTitle = "Nincs aktuális túra";
                ViewBag.TourAbout = "Még nincs aktuális túra";
            }
            Tour ActualTour = db.Tours.OrderBy(t => t.Date).FirstOrDefault(x => x.Date >= DateTime.Now);
            ActualTour.IsActual = true;
            ActualTour.IsActive = true;

            var toursToCheck = db.Tours.Where(t => t.TourId != ActualTour.TourId && t.IsActual == true).ToList();

            if (toursToCheck.Count() != 0)
            {
                toursToCheck.ForEach(t => t.IsActual = false);
            }

            db.SaveChanges();


            return ActualTour;
        }

        public Tour GetPreviousTour()
        {
            Tour Actual = GetActualTour();
            Tour PreviousTour = db.Tours.OrderByDescending(t => t.Date).FirstOrDefault(x => x.Date < Actual.Date);
            if (db.Tours.Count() == 0 || PreviousTour == null)
            {
                ViewBag.TourTitle = "Nincs előző túra";
                ViewBag.TourAbout = "Nincs előző túra";
            }
            return PreviousTour;


        }

        public void ClearActiveBeforeActual()
        {
            Tour Actual = GetActualTour();

            var activeTours = db.Tours.Where(t => t.Date < Actual.Date && t.IsActive == true).ToList();
            activeTours.ForEach(a => a.IsActive = false);
            db.SaveChanges();
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
