using System;
using System.Collections.Generic;
using System.Text;

namespace ICXP003.Models.Request
{
    class APPaymentVendorContract
    {
        public string period { get; set; }
        public string apcodeindependentvend { get; set; }
        public string ruc { get; set; }
        public string invoicenumber { get; set; }
        public string paymmode { get; set; }
        public string fixedvalue { get; set; }
        public string paymentdate { get; set; }
    }
}
