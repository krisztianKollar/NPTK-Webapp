using Microsoft.AspNet.Identity.Owin;
using nptk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nptk.Controllers
{
    public class HikersController : Controller
    {

        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Hikers
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }
    }
}