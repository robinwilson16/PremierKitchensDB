﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PremierKitchensDB.Models
{
    public class Area
    {
        public int AreaID { get; set; }
        public string AreaName { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }

        public string CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public ApplicationUser ApplicationUserCreatedBy { get; set; }

        [Display(Name = "Updated Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? UpdatedDate { get; set; }

        public string UpdatedBy { get; set; }
        [ForeignKey("UpdatedBy")]
        public ApplicationUser ApplicationUserUpdatedBy { get; set; }

        public ICollection<CustomerArea> CustomerArea { get; set; }
    }
}
