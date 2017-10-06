using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Ecomerce.Models
{
    public class User
    {

        [Key]
        public int UserId { get; set; }

        
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(256, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters", MinimumLength = 3)]
        [Display(Name = "E-Mail")]
        [Index("User_UserName_Index", IsUnique = true)]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(50, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters", MinimumLength = 3)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(50, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters", MinimumLength = 3)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

     
        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(20, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters", MinimumLength = 3)]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(100, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters", MinimumLength = 3)]

        public string Address { get; set; }


        [DataType(DataType.ImageUrl)]
        public string Photo { get; set; }

        
        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, Double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Department")]
        public int DepartmentId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, Double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "City")]
        public int CityId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, Double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }

        [Display(Name = "User")]
        public string FullName { get {return string.Format("{0}{1}",FirstName,LastName); } }

        [NotMapped]
        public HttpPostedFileBase PhotoFile { get; set; }
        [JsonIgnore]
        public virtual Department Department { get; set; }
        [JsonIgnore]
        public virtual City City { get; set; }
        [JsonIgnore]
        public virtual Company Company { get; set; }
    }
}