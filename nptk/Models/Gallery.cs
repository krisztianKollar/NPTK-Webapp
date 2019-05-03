using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class Gallery
    {
        [ForeignKey("Tour")]
        public int GalleryID { get; set; }

        public int TourID { get; set; }

        public virtual Tour Tour { get; set; }

        [Display(Name = "A galéria képei")]
        public virtual ICollection<Picture> TourPics { get; set; }
    }
}