using System;
using System.Collections.Generic;
using System.Text;

namespace ICXP003_004.Models.Request
{
    class APVendorJournalContract
    {
        public string apcodeindependentvend { get; set; }
        public string ruc { get; set; }
        public string transdate { get; set; }
        public decimal credit { get; set; }
        public string period { get; set; }       
        public string bonustypeid { get; set; }
        
    }
}
