﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecomerce.Models
{
    public class Warehouse
    {

        [Key]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, Double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Index("Warehouse_CompanyId_Name_Index", 1, IsUnique = true)]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }


        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(50, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters", MinimumLength = 3)]
        [Display(Name = "Warehouse")]
        [Index("Warehouse_CompanyId_Name_Index", 2,IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string Name { get; set; }

        

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(20, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters", MinimumLength = 3)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(100, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters", MinimumLength = 3)]
        public string Address { get; set; }
        
        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, Double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, Double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "City")]
        public int CityId { get; set; }

        public virtual Department Department { get; set; }

        public virtual City City { get; set; }

        public virtual Company Company { get; set; }


    }
}