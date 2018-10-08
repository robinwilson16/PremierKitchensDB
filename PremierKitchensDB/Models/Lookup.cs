using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

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
        public ApplicationUser ApplicationUserCreatedBy { get; set; }

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
        [ForeignKey("UpdatedBy")]
        [JsonIgnore]
        public ApplicationUser ApplicationUserUpdatedBy { get; set; }
    }
}
