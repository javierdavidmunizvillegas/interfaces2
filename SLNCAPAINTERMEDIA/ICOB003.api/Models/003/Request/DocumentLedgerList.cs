using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB003.api.Models._003.Request
{
    public class DocumentLedgerList
    {
        public string APIdentificationList { get; set; }//Id de la compañía 
        public string CustAccount { get; set; }
        public string TransDate { get; set; }
        public decimal AmountDebit { get; set; }
        public decimal AmountCredit { get; set; }
        public string PostingProfile { get; set; }
        public string DocumentNegoc { get; set; }

    }
}
