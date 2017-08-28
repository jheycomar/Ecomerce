using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ecomerce.Models
{
    public class EcomerceDataContext:DbContext
    {
        public EcomerceDataContext():base("DefaultConnection")
        {

        }

        public System.Data.Entity.DbSet<Ecomerce.Models.Department> Departments { get; set; }

        public System.Data.Entity.DbSet<Ecomerce.Models.City> Cities { get; set; }
    }
}