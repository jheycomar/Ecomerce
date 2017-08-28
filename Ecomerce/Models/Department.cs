using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Ecomerce.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(50, ErrorMessage ="The field {0} can contain maximun {1} and minimum {2} characters",MinimumLength = 3)]
        [Display(Name = "Department")]

        public string Name { get; set; }

        public virtual ICollection<City> Cities { get; set; }
    }
}