﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class Tour
    {

        public int Id { get; set; }
        [Display(Name = "Túra")]
        public string Title { get; set; }

        [Display(Name = "Dátum")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        [Display(Name = "Útvonal")]
        public string Track { get; set; }
        [Display(Name = "Távolság (km)")]
        public decimal Distance { get; set; }
        [Display(Name = "Szintemelkedés (m)")]
        public int Climb { get; set; }

    }
}