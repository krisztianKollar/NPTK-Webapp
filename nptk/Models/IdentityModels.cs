using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System.Linq;

namespace nptk.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        ApplicationDbContext context = new ApplicationDbContext();

        [Display(Name = "E-mail")]
        [DataType(DataType.EmailAddress)]
        public override string Email { get; set; }

        [Display(Name = "Felhasználónév")]
        public override string UserName { get; set; }

        [Required]
        [Display(Name = "Vezetéknév")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Keresztnév")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Születési idő")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Név")]
        public string FullName => LastName + " " + FirstName;

        [Display(Name = "Rang")]
        public string HikerRank => context.GetTheHikerRank(Id);

        [Display(Name = "Jelentkezések")]
        public virtual ICollection<SignUp> SignUps { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim("FirstName", FirstName));
            userIdentity.AddClaim(new Claim("FullName", FullName));
            userIdentity.AddClaim(new Claim("HikerRank", HikerRank));

            return userIdentity;
        }


    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, CustomRole,
        int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public ApplicationDbContext()
            : base("nptkContext")
        {
            //this.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Tour> Tours { get; set; }

        public DbSet<SignUp> SignUps { get; set; }

        public decimal DistanceCount(int? Id)
        {
            decimal distanceCount = (from t in this.Tours
                             from s in t.SignUps
                             where s.UserID == Id
                             select (decimal?)t.Distance).Sum() ?? 0;
            Debug.WriteLine("DistanceCount: " + distanceCount + " id: " + Id);
            return distanceCount;
        }

        public int ClimbCount(int? Id)
        {
            int climbCount = (from t in this.Tours
                              from s in t.SignUps
                              where s.UserID == Id
                              select (int?)t.Climb).Sum() ?? 0;
            Debug.WriteLine("ClimbCount: " + climbCount + " id: " + Id);
            return climbCount;
        }

        public int TourCount(int? Id)
        {
            int tourCount = (from t in this.Tours
                             from s in t.SignUps
                             where s.UserID == Id
                             select t).Count();
            return tourCount;
        }

        public string GetTheHikerRank(int id)
        {
            int num = TourCount(id);
            if (num >= 3)
                return "golden";
            else if (num >= 2 && num < 3)
                return "silver";
            else if (num >= 1 && num < 2)
                return "bronze";
            return "none";
        }

        public decimal DistanceTotal()
        {
            decimal distanceTotal = (from t in Tours select t.Distance).Sum();
            return distanceTotal;
        }

        public int ClimbTotal()
        {
            return (from t in Tours select t.Climb).Sum();
        }

        public int TourTotal()
        {
            return (from t in Tours select t).Count();
        }
    }

    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }

    public class CustomRole : IdentityRole<int, CustomUserRole>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class UserStore : UserStore<ApplicationUser, CustomRole, int,
        CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        public UserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, int, CustomUserRole>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    /*public class ApplicationRoleManager : RoleManager<CustomRole, int>
    {
        public ApplicationRoleManager(IRoleStore<CustomRole, int> roleStore)
            : base(roleStore)
        {
        }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            return new ApplicationRoleManager(new RoleStore<CustomRole, int, CustomUserRole>(context.Get<ApplicationDbContext>()));
        }
    }*/
}