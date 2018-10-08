using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PremierKitchensDB.Models
{
    public class AuditTrail
    {
        public int AuditTrailID { get; set; }
        public string TableName { get; set; }
        public int ObjectID { get; set; }
        public string WhereClause { get; set; }
        public string RowDescription { get; set; }

        [Display(Name = "Changes Made")]
        public string ChangeInfo { get; set; }
        public int ChangeType { get; set; }
        public string Screen { get; set; }

        [Display(Name = "Made On")]
        public DateTime? UpdatedDate { get; set; }

        [Display(Name = "Made By")]
        public string UpdatedBy { get; set; }
        [ForeignKey("UpdatedBy")]
        [Display(Name = "Made By")]
        public ApplicationUser ApplicationUserUpdatedBy { get; set; }
    }
}
