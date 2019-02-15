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

namespace nptk.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<int, CustomUserLogin, CustomUserRole, CustomUserClaim>
    {
        [Display(Name = "E-mail")]
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

        [Display(Name = "Jelentkezések")]
        public virtual ICollection<SignUp> SignUps { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, int> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
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

        public System.Data.Entity.DbSet<nptk.Models.Tour> Tours { get; set; }

        public System.Data.Entity.DbSet<nptk.Models.SignUp> SignUps { get; set; }

        public decimal DistanceCount(int? UserId)
        {
            decimal count = 0;
            // linq query or something similar instead of the foreach???
            foreach (SignUp signUp in SignUps)
            {
                if (signUp.UserID == UserId)
                    count += signUp.Tour.Distance;
            }
            return count;
        } 
        
        public int ClimbCount(int? UserId)
        {
            int count = 0;
            foreach (SignUp signUp in SignUps)
            {
                if (signUp.UserID == UserId)
                    count += signUp.Tour.Climb;
            }
            return count;
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