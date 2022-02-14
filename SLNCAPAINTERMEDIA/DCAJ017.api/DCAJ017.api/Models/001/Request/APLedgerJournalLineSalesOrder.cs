using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ017.api.Models._001.Request
{
    public class APLedgerJournalLineSalesOrder
    {
        public decimal Amount { get; set; }
        public bool CheckAdvancedLiquidated { get; set; }
        public DateTime TransDate { get; set; }
    }
}
