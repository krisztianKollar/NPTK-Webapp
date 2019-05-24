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
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Útvonal")]
        public string Track { get; set; }

        [Display(Name = "Távolság")]
        [DisplayFormat(DataFormatString = "{0:#,##0.0#}", ApplyFormatInEditMode = true)]
        public double Distance { get; set; }

        [Display(Name = "Szintemelkedés")]
        public int Climb { get; set; }

        [Display(Name = "A túráról")]
        [DataType(DataType.MultilineText)]
        public string About { get; set; }

        [Display(Name = "Aktív túra?")]
        public bool IsActive { get; set; }

        [Display(Name = "Aktuális túra?")]
        public bool IsActual { get; set; }

        /*[Display(Name = "Képek")]
        public virtual ICollection<Picture> TourPics { get; set; }*/

        [Display(Name = "Képek")]
        public virtual Gallery Gallery { get; set; }

        [Display(Name = "Jelentkezők")]
        public virtual ICollection<SignUp> SignUps { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}