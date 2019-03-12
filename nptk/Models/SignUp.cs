using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class SignUp
    {
        public SignUp()
        {
        }

        public int SignUpID { get; set; }
        public int TourID { get; set; }
        public int UserID { get; set; }
        
        public virtual Tour Tour { get; set; }
        public virtual ApplicationUser User { get; set; }

        public override string ToString()
        {
            return ("SignUpID = " + SignUpID + " TourID = " + TourID + " UserID= " + UserID);
        }

        
    }


}