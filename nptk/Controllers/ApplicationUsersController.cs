using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using nptk.Models;

namespace nptk.Controllers
{
    [Authorize]
    public class ApplicationUsersController : Controller
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


        // GET: ApplicationUsers
        public ActionResult Index(string sortOrder)
        {
            try
            {
                ViewBag.FullNameSortParm = string.IsNullOrEmpty(sortOrder) ? "fullname_desc" : "";
                ViewBag.UserNameSortParm = sortOrder == "username" ? "username_desc" : "username";
                ViewBag.EmailSortParm = sortOrder == "email" ? "email_desc" : "email";
                ViewBag.BirthDateSortParm = sortOrder == "birthdate" ? "birthdate_desc" : "birthdate";
                var users = from u in db.Users
                            select u;
                switch (sortOrder)
                {
                    case "fullname_desc":
                        users = users.OrderByDescending(u => u.LastName + " " + u.FirstName);
                        break;
                    case "username":
                        users = users.OrderBy(u => u.UserName);
                        break;
                    case "username_desc":
                        users = users.OrderByDescending(u => u.UserName);
                        break;
                    case "email":
                        users = users.OrderBy(u => u.Email);
                        break;
                    case "email_desc":
                        users = users.OrderByDescending(u => u.Email);
                        break;
                    case "birthdate":
                        users = users.OrderBy(u => u.BirthDate);
                        break;
                    case "birthdate_desc":
                        users = users.OrderByDescending(u => u.BirthDate);
                        break;
                    default:
                        users = users.OrderBy(u => u.LastName + " " + u.FirstName);
                        break;
                }
                return View(users.ToList());
            }
            catch (Exception e)
            {

                Debug.WriteLine(e.ToString());
            }

            return View();
        }

        // GET: ApplicationUsers/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ApplicationUser applicationUser = db.Users.Find(id);
                if (applicationUser == null)
                {
                    return HttpNotFound();
                }

                var DistanceCount = db.DistanceCount(id);
                var ClimbCount = db.ClimbCount(id);
                ViewBag.Distances = DistanceCount;
                ViewBag.Climbs = ClimbCount;

                return View(applicationUser);
            }
            catch (Exception e)
            {

                Debug.WriteLine(e.ToString());
            }
            return View();
        }

        // GET: ApplicationUsers/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create(CreateUserViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser { UserName = model.UserName, LastName = model.LastName, FirstName = model.FirstName, BirthDate = model.BirthDate, Email = model.Email, EmailConfirmed = model.EmailConfirmed, PhoneNumber = model.PhoneNumber };

                    var password = user.LastName.Substring(0, 1).ToUpper() + user.FirstName.Substring(0, 1).ToLower() + "-" + user.BirthDate.ToString().Substring(0, 4);
                    var result = await UserManager.CreateAsync(user, password);
                    Debug.WriteLine(password);
                    if (result.Succeeded)
                    {
                        //db.Users.Add(user);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                // If we got this far, something failed, redisplay form
                return View(model);
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Nem sikerült a létrehozás. Próbáld újra, s ha nem megy, keresd az adminisztrátort!");
            }
            return View(model);
        }

        // GET: ApplicationUsers/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult EditPost(int? Id)
        {
            if (Id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userToUpdate = db.Users.Find(Id);
            if (TryUpdateModel(userToUpdate, "",
               new string[] { "LastName", "FirstName", "BirthDate", "Email", "EmailConfirmed", "PhoneNumber", "PhoneNumberConfirmed", "UserName" }))
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
            return View(userToUpdate);
        }

        // GET: ApplicationUsers/Delete/5
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
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int? id)
        {
            try
            {
                ApplicationUser applicationUser = db.Users.Find(id);
                db.Users.Remove(applicationUser);
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "Admin")]
        public ActionResult GetDistanceCount(int id)
        {
            try
            {
                var count = db.DistanceCount(id);
                Debug.WriteLine("PÖCS: " + count);
                ViewBag.Distances = count;
                return View();
            }
            catch (Exception e)
            {

                Debug.WriteLine(e.ToString());
            }
            return View();
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
