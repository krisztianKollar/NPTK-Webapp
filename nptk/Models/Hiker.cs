using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class Hiker
    {
        public int ID { get; set; }
        [Display(Name = "Felhasználónév")]
        public string UserName { get; set; }
        [Display(Name = "Vezetéknév")]
        public string LastName { get; set; }
        [Display(Name = "Keresztnév")]
        public string FirstName { get; set; }
        [Display(Name = "Születési dátum")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        public virtual ICollection<SignUp> SignUps { get; set; }
    }
}