using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.api.Models._004.Request
{
    public class APSalesTableReturn004
    {
        [Required]
        public string CustAccount { get; set; }
        [Required]
        public string InvoiceId { get; set; }
        public string ReturnDeadline { get; set; }
        [Required]
        public string ReturnReasonCodeId { get; set; }
        public string ReturnReasonComment { get; set; }
        [Required]
        public List<APSalesLineReturn004> APSalesLineReturnList { get; set; }
    }
}
