using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nptk.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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