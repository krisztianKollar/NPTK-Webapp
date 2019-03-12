using nptk.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nptk.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            GetActualTour();
            GetPreviousTour();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Amit a túrakörről tudni érdemes:";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Ahol érdemes információt keresni:";

            return View();
        }

        public Tour GetActualTour()
        {
            Tour ActualTour = db.Tours.SingleOrDefault(x => x.IsActual == true);
            if (db.Tours.Count() == 0)
            {
                ViewBag.TourTitle = "Nincs aktuális túra";
                ViewBag.TourAbout = "Még nincs aktuális túra";
            }
            if (ActualTour == null)
            {
                ActualTour = db.Tours.SingleOrDefault(x => x.Date >= DateTime.Now);
                ActualTour.IsActual = true;
                db.SaveChanges();
            }
            ViewBag.TourTitle = ActualTour.Title;
            ViewBag.TourAbout = ActualTour.About;
            ViewBag.ActId = ActualTour.TourId;
            return ActualTour;
        }

        public void GetPreviousTour()
        {
            Tour Actual = GetActualTour();
            Tour PreviousTour = db.Tours.OrderByDescending(t => t.Date).FirstOrDefault(x => x.Date < Actual.Date);
            if (db.Tours.Count() == 0 || PreviousTour == null)
            {
                ViewBag.TourTitle = "Nincs előző túra";
                ViewBag.TourAbout = "Nincs előző túra";
            }
            ViewBag.PrevTourTitle = PreviousTour.Title;
            ViewBag.PrevTourAbout = PreviousTour.About;
            ViewBag.PrevId = PreviousTour.TourId;
        }
    }
}