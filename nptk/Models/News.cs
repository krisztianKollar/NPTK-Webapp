using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class News
    {
        public int NewsId { get; set; }

        [Display(Name = "Hír")]
        public string NewsTitle { get; set; }

        [Display(Name = "Mikor?")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy. MM. dd.}", ApplyFormatInEditMode = true)]
        public DateTime NewsDate { get; set; }

        [Display(Name = "Miről van szó?")]
        [DataType(DataType.MultilineText)]
        public string NewsAbout { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}