using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace nptk.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
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
        public string FullName
        {
            get { return LastName + " " + FirstName; }
        }

        public virtual ICollection<SignUp> SignUps { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("nptkContext", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public System.Data.Entity.DbSet<nptk.Models.Tour> Tours { get; set; }

        public System.Data.Entity.DbSet<nptk.Models.SignUp> SignUps { get; set; }

        
    }
}