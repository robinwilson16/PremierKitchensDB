using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PremierKitchensDB.Models
{
    public class Address
    {
        public int AddressID { get; set; }
        public int CustomerID { get; set; }

        [Display(Name = "Address")]
        [Required]
        public string Address1 { get; set; }

        [Display(Name = "Address 2")]
        public string Address2 { get; set; }

        [Display(Name = "Address 3")]
        public string Address3 { get; set; }

        [Display(Name = "Address 4")]
        public string Address4 { get; set; }

        [Display(Name = "Post Code")]
        [Required]
        public string PostcodeOut { get; set; }

        [Display(Name = "Post Code")]
        [Required]
        public string PostcodeIn { get; set; }

        [Display(Name = "Home Phone")]
        public string HomePhone { get; set; }

        [Display(Name = "Date From")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "Date To")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? DateTo { get; set; }

        [Display(Name = "Is Primary")]
        public bool IsPrimary { get; set; }

        [Display(Name = "Address Type")]
        public int AddressTypeID { get; set; }

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        [JsonIgnore]
        [Display(Name = "Created By")]
        public ApplicationUser ApplicationUserCreatedBy { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }
        
        public string UpdatedBy { get; set; }
        [ForeignKey("UpdatedBy")]
        [JsonIgnore]
        [Display(Name = "Updated By")]
        public ApplicationUser ApplicationUserUpdatedBy { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }
        [JsonIgnore]
        public AddressType AddressType { get; set; }
    }
}
