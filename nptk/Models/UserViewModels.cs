using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Vezetéknév")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Keresztnév")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Felhasználónév")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "E-mail megerősítve")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Születési dátum")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Phone]
        [Display(Name = "Telefonszám")]
        public string Number { get; set; }
    }
}