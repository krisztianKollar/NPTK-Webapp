using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using nptk.Models;
using Owin;
using System;
using System.Web;

[assembly: OwinStartupAttribute(typeof(nptk.Startup))]
namespace nptk
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesandUsers();
        }

        private void CreateRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<CustomRole, int>(new RoleStore<CustomRole, int, CustomUserRole>(context));
            var UserManager = new UserManager<ApplicationUser, int>(new UserStore<ApplicationUser, CustomRole, int, CustomUserLogin, CustomUserRole, CustomUserClaim>(context));

            // Creating Admin Role and creating a default Admin User     
            if (!roleManager.RoleExists("Admin"))
            {
                //first we create Admin role   
                var role = new CustomRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);
            }

            ////Here we create an Admin super user who will maintain the website     
            var user = new ApplicationUser
            {
                FirstName = "Firstname",
                LastName = "Lastname",
                BirthDate = new DateTime(1977, 12, 31, 23, 30, 0),
                UserName = "kollarkr",
                Email = "nagypeletura@gmail.com"
            };

            string userPWD = "A_z200711";

            var chkUser = UserManager.Create(user, userPWD);
            if (chkUser.Succeeded)
            {
                var result1 = UserManager.AddToRole(user.Id, "Admin");
                context.SaveChanges();
                ;
            }






            /*var roleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            var UserManager = new UserManager<ApplicationUser, int>(new UserStore(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {
                // first we create Admin rool   
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole
                {
                    Name = "Admin"
                };
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser
                {
                    FirstName = "Krisztián",
                    LastName = "Kollár",
                    BirthDate = new DateTime(1977, 12, 31, 23, 30, 0),
                    UserName = "kollarkr",
                    Email = "nagypeletura@gmail.com"
                };

                string userPWD = "A_z200711";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");
                    context.SaveChanges();

                }
            }    */
        }
    }
}


