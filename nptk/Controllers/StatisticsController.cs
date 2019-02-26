using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using nptk.Models;

namespace nptk.Controllers
{
    // GET: /Statisctics/Index
    [Authorize]
    public class StatisticsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: Statistics
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId<int>();
            var user = UserManager.FindById(User.Identity.GetUserId<int>());
            var users = from u in db.Users
                        select u;
            List<StatisticViewModel> models = new List<StatisticViewModel>();


            foreach (ApplicationUser u in users)
            {
                var model = new StatisticViewModel
                {
                    UserName = u.UserName,
                    FullName = u.FullName,
                    UserTotalDistance = db.DistanceCount(u.Id),
                    UserTotalClimb = db.ClimbCount(u.Id),
                    UserTourCount = db.TourCount(u.Id),
                };
                models.Add(model);
            }

            ViewBag.UserTotalDistance = db.DistanceCount(user.Id);
            ViewBag.UserTotalClimb = db.ClimbCount(user.Id);
            ViewBag.DistanceTotal = db.DistanceTotal();
            ViewBag.ClimbTotal = db.ClimbTotal();
            ViewBag.TourTotal = db.TourTotal();
            ViewBag.TourCount = db.TourCount(user.Id);
            ViewBag.Models = models.OrderByDescending(x => x.UserTotalDistance).Take(12); 

            return View();
        }
    }
}