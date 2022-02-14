using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB009.api.Models._001.Request
{
    public class APDationRequests
    {
        [Required]
        public string CustAccount { get; set; }
        public string DationNumber { get; set; }
        public DateTime InvoiceDateFrom { get; set; }
        public DateTime InvoiceDateTo { get; set; }
        public string InvoiceId { get; set; }
        public string RecallDocument { get; set; }
    }
}
