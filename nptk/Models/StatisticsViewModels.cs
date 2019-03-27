using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class StatisticViewModel
    {
        [Display(Name = "Felhasználónév")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Név")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Megtett teljes táv")]
        public double UserTotalDistance { get; set; }

        [Required]
        [Display(Name = "Megtett teljes szint")]
        public int UserTotalClimb { get; set; }

        [Required]
        [Display(Name = "Teljesített túrák")]
        public int UserTourCount { get; set; }

        public override string ToString()
        {
            return ("UTDist: " + UserTotalDistance + " UTClimb: " + UserTotalClimb + " Username: " + UserName + " Fullname: " + FullName + " UserTourCount: " + UserTourCount);
        }
    }
}