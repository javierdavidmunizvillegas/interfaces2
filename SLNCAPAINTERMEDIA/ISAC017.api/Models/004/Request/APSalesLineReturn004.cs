using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.api.Models._004.Request
{
    public class APSalesLineReturn004
    {
        [Required]
        public string ItemId { get; set; }
        [Required]
        public decimal Qty { get; set; }
        public string SerialId { get; set; }
        public decimal Monto { get; set; }
    }
}
