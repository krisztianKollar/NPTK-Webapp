using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class TourViewModel
    {
        [Display(Name = "Túra")]
        public string Title { get; set; }

        [Display(Name = "Dátum")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy. MM. dd.}", ApplyFormatInEditMode = true)]
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

        [Display(Name = "Képek")]
        public string[] PicPaths { get; set; }

        public HttpPostedFileBase[] Gallery { get; set; }
    }
}