using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class SignUp
    {
    }

    public class Enrollment
    {
        public int SignUpID { get; set; }
        public int TourID { get; set; }
        public int HikerID { get; set; }
        
        public virtual Tour tour { get; set; }
        public virtual Hiker hiker { get; set; }
    }
}