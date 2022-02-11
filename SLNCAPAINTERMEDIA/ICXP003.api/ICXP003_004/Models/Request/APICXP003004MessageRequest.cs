using System;
using System.Collections.Generic;
using System.Text;

namespace ICXP003_004.Models.Request
{
    class APICXP003004MessageRequest
    {
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }
        public List<APVendorJournalContract> apvendorjournalcontract { get; set; }
    }
}
