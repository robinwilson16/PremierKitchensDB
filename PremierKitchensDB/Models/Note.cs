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
    public class Note
    {
        public int NoteID { get; set; }
        public int CustomerID { get; set; }

        [Display(Name = "Note")]
        public string NoteText { get; set; }

        [Display(Name = "Show As Alert?")]
        public bool IsAlert { get; set; }

        [Display(Name = "Created Date")]
        public DateTime? CreatedDate { get; set; }

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
    }
}
