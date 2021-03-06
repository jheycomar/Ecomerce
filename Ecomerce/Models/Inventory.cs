﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ecomerce.Models
{
    public class Inventory
    {
        [Key]
        public int InventoryId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
        public double Stock { get; set; }

        [JsonIgnore]
        public virtual Warehouse Warehouse { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }

    }
}