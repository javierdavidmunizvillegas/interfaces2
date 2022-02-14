using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB001.api.Models._001.Request
{
    public class APDocumentICOB001001
    {
        [Required]
        public string Voucher { get; set; }

        public string InvoiceId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string VoucherId { get; set; }

        public string[] ErrorList { get; set; }
    }
}
