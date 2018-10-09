﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PremierKitchensDB.Models
{
    public class SourceOfInformation
    {
        public int SourceOfInformationID { get; set; }

        [Display(Name = "Source of Information")]
        public string SourceOfInformationName { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        [Display(Name = "Created By")]
        public ApplicationUser ApplicationUserCreatedBy { get; set; }

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
        [ForeignKey("UpdatedBy")]
        [Display(Name = "Updated By")]
        public ApplicationUser ApplicationUserUpdatedBy { get; set; }

        public ICollection<Customer> Customer { get; set; }
    }
}
