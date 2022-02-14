using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ017.api.Models._001.Request
{
    public class APVendTransRegistrationFine
    {
        public decimal Amount { get; set; }
        public string BusinessUnit { get; set; }
        public string Cpn { get; set; }
        public string CustAccount { get; set; }
        public string CustIdentification { get; set; }
        public string InvoiceId { get; set; }
        public string Motive { get; set; }
        public string Voucher { get; set; }
    }
}
