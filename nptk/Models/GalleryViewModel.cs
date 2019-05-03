using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class GalleryViewModel
    {
        public int TourId { get; set; }

        public virtual Tour Tour { get; set; }

        [Display(Name = "Képek")]
        public string[] PicPaths { get; set; }

        public HttpPostedFileBase[] UploadedPics { get; set; }
    }
}