using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ005.api.Models._001.Request
{
    public class LedgerJournalCashDepositsRequestList
    {
        public string PreparationDate { get; set; } // datetime
        public string DateCollection { get; set; } // datetime
        public string CollectionEntry { get; set; } // datetime
        public decimal  CollectionAmount { get; set; } // datetime
        public string PaymentCollection { get; set; } // datetime
        public string Bank { get; set; }
        public string DepositNumber { get; set; }
        public string UserDeposit { get; set; }

    }
}
