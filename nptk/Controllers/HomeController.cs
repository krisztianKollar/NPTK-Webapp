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
        ToursController ToursController = new ToursController();
        private NewsController NewsController = new NewsController();

        public ActionResult Index()
        {
            Tour ActualTour = ToursController.GetActualTour();
            Tour PreviousTour = ToursController.GetPreviousTour();
            News ActualNews = NewsController.GetActualNews();
            ToursController.ClearActiveBeforeActual();
            ViewBag.TourTitle = ActualTour.Title;
            ViewBag.TourAbout = ActualTour.About;
            ViewBag.ActId = ActualTour.TourId;
            ViewBag.PrevTourTitle = PreviousTour.Title;
            ViewBag.PrevTourAbout = PreviousTour.About;
            ViewBag.PrevId = PreviousTour.TourId;
            ViewBag.ActualNewsId = ActualNews.NewsId;
            ViewBag.NewsTitle = ActualNews.NewsTitle;
            ViewBag.ActualNews = ActualNews.NewsAbout;

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
    }
}