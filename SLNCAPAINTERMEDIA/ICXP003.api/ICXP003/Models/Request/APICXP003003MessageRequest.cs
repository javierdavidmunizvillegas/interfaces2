using System;
using System.Collections.Generic;
using System.Text;

namespace ICXP003.Models.Request
{
    class APICXP003003MessageRequest
    {
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }
        public List<APPaymentVendorContract> appaymentvendorcontractlist { get; set; }
    }
}
