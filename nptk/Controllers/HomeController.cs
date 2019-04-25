using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using nptk.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace nptk.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        ToursController ToursController = new ToursController();
        private NewsController NewsController = new NewsController();
        private ApplicationUserManager _userManager;


        public HomeController() { }

        public HomeController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

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

        public async Task<ActionResult> SendComment(ContactViewModel model)
        {
            Debug.WriteLine(model.Email);
            Debug.WriteLine(model.Comment);
            //var admins = Roles.GetUsersInRole("Admin");
            //Debug.WriteLine("Admins: " + admins.ToString());
            Debug.WriteLine("usercount: " + db.Users.Count());
            Debug.WriteLine("rolecount: " + db.Roles.Count());
            foreach ( CustomRole role in db.Roles)
            {
                Debug.WriteLine(role.Users);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string body = model.Name + " (" + model.Email + ") üzenete: \n\n" + model.Comment;
                    foreach (ApplicationUser user in db.Users)
                    {
                        Debug.WriteLine("user: " + user.UserName.ToString());
                        string[] roles = Roles.GetRolesForUser(user.UserName);
                        foreach (string role in roles)
                            Debug.WriteLine("role: " + role.ToString());
                        if (roles.Contains("Admin"))
                            Debug.WriteLine(user.UserName + " is an admin.");
                        else
                        {
                            Debug.WriteLine(user.Id + " is not admin " + roles.ToString());
                        }
                        await UserManager.SendEmailAsync(user.Id, "Nagy Pele Túrakör – " + model.Name + " üzenete", body);

                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
            return RedirectToAction("About");
        }
    }
}

