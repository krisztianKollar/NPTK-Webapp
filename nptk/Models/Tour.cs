using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class Tour
    {

        public int TourId { get; set; }

        [Display(Name = "Túra")]
        public string Title { get; set; }

        [Display(Name = "Dátum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy. MM. dd.}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Útvonal")]
        public string Track { get; set; }

        [Display(Name = "Távolság")]
        public decimal Distance { get; set; }

        [Display(Name = "Szintemelkedés")]
        public int Climb { get; set; }

        public bool IsActive { get; set; }

        public bool IsActual { get; set; }

        [Display(Name = "Jelentkezések")]
        public virtual ICollection<SignUp> SignUps { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}