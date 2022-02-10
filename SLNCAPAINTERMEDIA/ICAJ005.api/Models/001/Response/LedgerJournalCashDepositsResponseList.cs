using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ005.api.Models._001.Response
{
    public class LedgerJournalCashDepositsResponseList
    {
        public decimal CollectionAmount { get; set; } // datetime
        public string Bank { get; set; }
        public string DepositNumber { get; set; }
    }
}
