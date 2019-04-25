using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class Picture
    {
        [Key]
        public int PicID { get; set; }
        public int GalleryID { get; set; }
        public string Path { get; set; }
        public string PicName { get; set; }

        public virtual Gallery Gallery { get; set; }
    }
}