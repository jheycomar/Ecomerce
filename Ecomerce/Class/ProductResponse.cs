using Ecomerce.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ecomerce.Class
{
    public class ProductResponse
    {
     
        public int ProductId { get; set; }
        public string Description { get; set; }
        public string BarCode { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Remarks { get; set; }
        public double Stock { get; set; }
        public List<Inventory> Inventory { get; set; }
        public Company Company { get; set; }
        public Category Category { get; set; }
        public Tax Tax { get; set; }

    }
}