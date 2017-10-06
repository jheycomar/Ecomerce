using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecomerce.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(50, ErrorMessage ="The field {0} can contain maximun {1} and minimum {2} characters",MinimumLength = 3)]
        [Display(Name = "Department")]
        [Index("Department_Name_Index", IsUnique = true)]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<City> Cities { get; set; }
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