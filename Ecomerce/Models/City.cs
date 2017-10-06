using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecomerce.Models
{
    public class City
    {
        [Key]
        public int CityId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(50, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters", MinimumLength = 3)]
        [Display(Name = "City")]
        [Index("City_Name_Index",2, IsUnique = true)]
        public string Name { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, Double.MaxValue, ErrorMessage ="You must select a {0}")]
        [Index("City_Name_Index",1, IsUnique = true)]
        public int DepartmentId { get; set; }
        [JsonIgnore]
        public virtual Department Department { get; set; }
        [JsonIgnore]
        public virtual ICollection<Company> Companies { get; set; }
        [JsonIgnore]
        public virtual ICollection<User> Users { get; set; }
        [JsonIgnore]
        public virtual ICollection<Warehouse> Warehouses { get; set; }
        [JsonIgnore]
        public virtual ICollection<Customer> Customers { get; set; }
    }
}