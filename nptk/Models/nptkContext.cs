﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace nptk.Models
{
    public class NptkContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public NptkContext() : base("name=nptkContext")
        {
        }

        public System.Data.Entity.DbSet<nptk.Models.Tour> Tours { get; set; }
        public System.Data.Entity.DbSet<nptk.Models.Hiker> Hikers { get; set; }
        public System.Data.Entity.DbSet<nptk.Models.SignUp> SignUps { get; set; }
    }
}
