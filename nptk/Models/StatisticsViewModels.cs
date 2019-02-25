using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class StatisticViewModels
    {
        [Display(Name = "Felhasználónév")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Vezetéknév")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Keresztnév")]
        public string FirstName { get; set; }

        [Required]
        public decimal UserTotalDistance { get; set; }

        [Required]
        public decimal UserTotalClimb { get; set; }

        [Required]
        public decimal DistanceTotal { get; set; }

        [Required]
        public decimal ClimbTotal { get; set; }

    }
}