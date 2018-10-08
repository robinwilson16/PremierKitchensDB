using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PremierKitchensDB.Models
{
    public class CustomerArea
    {
        public int CustomerID { get; set; }
        public int AreaID { get; set; }

        [JsonIgnore]
        public Customer Customer { get; set; }
        [JsonIgnore]
        public Area Area { get; set; }
    }
}
