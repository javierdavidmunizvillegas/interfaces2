using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA009.api.Models._001.Request
{
    public class APInventTableIVTA009001
    {
        [Required]
        public string ItemId { get; set; }
        [Required]
        public string VendAccount { get; set; }
        [Required]
        public decimal PurchPrice { get; set; }

    }
}
