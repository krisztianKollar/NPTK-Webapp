using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using nptk.Models;

namespace nptk.Controllers
{
    public class GalleriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Galleries
        public ActionResult Index()
        {
            var galleries = db.Galleries.Include(g => g.Tour);
            return View(galleries.ToList());
        }

        // GET: Galleries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // GET: Galleries/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.TourId = new SelectList(db.Tours.Where(t => t.Gallery == null).OrderByDescending(t => t.Date), "TourId", "Title");
            return View();
        }

        // POST: Galleries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(GalleryViewModel model)
        {
            try
            {
                ViewBag.TourId = new SelectList(db.Tours.Where(t => t.Gallery == null).OrderByDescending(t => t.Date), "TourId", "Title");
                //var errors = ModelState.Values.SelectMany(v => v.Errors);
                if (ModelState.IsValid)
                {
                    Gallery gallery = new Gallery
                    {
                        GalleryID = model.TourId,
                        TourID = model.TourId
                    };
                    db.Galleries.Add(gallery);
                    db.SaveChanges();
                    Tour tour = db.Tours.Find(model.TourId);
                    if (model.UploadedPics.Count() == 1 && tour.Date > DateTime.Now)
                    {
                        var pic = model.UploadedPics[0];
                            string extension = Path.GetExtension(pic.FileName);
                            string PicPath = "poster_" + model.TourId;
                            var path = Path.Combine(Server.MapPath("/Content/Posters/" + PicPath + extension));
                            pic.SaveAs(path);
                            Picture picture = new Picture
                            {
                                Path = PicPath + extension,
                                PicName = PicPath,
                                GalleryID = gallery.GalleryID
                            };

                            db.Pictures.Add(picture);
                    }
                    if (model.UploadedPics.Count() > 1)
                    {
                        var picCount = 0;
                        foreach (var pic in model.UploadedPics)
                        {
                            picCount += 1;
                            string extension = Path.GetExtension(pic.FileName);
                            string PicPath = "tour" + model.TourId + "_" + picCount;
                            var path = Path.Combine(Server.MapPath("/Content/TourGallery/" + PicPath + extension));
                            pic.SaveAs(path);
                            Picture picture = new Picture
                            {
                                Path = PicPath + extension,
                                PicName = PicPath,
                                GalleryID = gallery.GalleryID
                            };
                            db.Pictures.Add(picture);
                        }
                    }

                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException dex)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Nem sikerült a létrehozás. Próbáld újra, s ha nem megy, keresd az adminisztrátort!");
            }
            return View();
        }


        // GET: Galleries/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            ViewBag.TourID = new SelectList(db.Tours, "TourId", "Title", gallery.Tour.TourId);
            return View(gallery);
        }

        // POST: Galleries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "GalleryID,TourID")] Gallery gallery)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gallery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TourID = new SelectList(db.Tours, "TourId", "Title", gallery.Tour.TourId);
            return View(gallery);
        }

        // GET: Galleries/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Sikertelen az album törlése! Próbáld újra, s ha továbbra is fennáll a probléma, keresd az adminisztrátort!";
            }
            Gallery gallery = db.Galleries.Find(id);
            if (gallery == null)
            {
                return HttpNotFound();
            }
            return View(gallery);
        }

        // POST: Galleries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Gallery gallery = db.Galleries.Find(id);
                if (gallery.TourPics.Count() != 0)
                {
                    DeletePicFiles(id);
                }
                db.Galleries.Remove(gallery);
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }

        public void DeletePicFiles(int id)
        {
            IEnumerable<Picture> picsToDelete = db.Pictures.Where(p => p.GalleryID == id).ToList();
            foreach (Picture pic in picsToDelete)
            {
                string path = Path.Combine(HttpRuntime.AppDomainAppPath, "Content/TourGallery/" + pic.Path);
                System.IO.File.Delete(path);
            }
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
