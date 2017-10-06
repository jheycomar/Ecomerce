using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ecomerce.Models
{
    public class Tax
    {

        [Key]
        public int TaxId { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [StringLength(50, ErrorMessage = "The field {0} can contain maximun {1} and minimum {2} characters", MinimumLength = 3)]
        [Display(Name = "Tax")]
        [Index("Tax_CompanyId_Description_Index", 2, IsUnique = true)]
        public string Description { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(0, 1, ErrorMessage = "You must select a {0} between {1} and {2}")]
        [DisplayFormat(DataFormatString = "{0:P2}", ApplyFormatInEditMode = false)]
        public double Rate { get; set; }

        [Required(ErrorMessage = "The field {0} is required")]
        [Range(1, Double.MaxValue, ErrorMessage = "You must select a {0}")]
        [Index("Tax_CompanyId_Description_Index", 1, IsUnique = true)]
        [Display(Name = "Company")]
        public int CompanyId { get; set; }
        [JsonIgnore]
        public virtual Company Company { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product> Products { get; set; }
    }
}