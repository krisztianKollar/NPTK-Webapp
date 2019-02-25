using System;
using System.Collections.Generic;
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


            var model = new StatisticViewModels
            {
                UserName = user.UserName,
                LastName = user.LastName,
                FirstName = user.FirstName,
                UserTotalDistance = db.DistanceCount(user.Id), //Id-s are not ok here!
                UserTotalClimb = db.ClimbCount(user.Id),
                DistanceTotal = db.DistanceTotal(),
                ClimbTotal = db.ClimbTotal()
            };


            decimal DistanceTotal = db.DistanceTotal();
            int ClimbTotal = db.ClimbTotal();
            int TourTotal = db.TourTotal();
            ViewBag.DistanceTotal = db.DistanceTotal();
            ViewBag.ClimbTotal = db.ClimbTotal(); 
            ViewBag.TourTotal = db.TourTotal();

            return View(model);
        }
    }
}