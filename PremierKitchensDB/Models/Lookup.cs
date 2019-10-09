using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PremierKitchensDB.Models
{
    public class Lookup
    {
        [Required]
        public string Domain { get; set; }
        [Required]
        public string LookupCode { get; set; }
        [Required]
        public string LookupName { get; set; }

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
    }
}
