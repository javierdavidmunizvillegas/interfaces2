using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ003.api.Models._001.Request
{
    public class APLedgerJournalLineSalesOrder
    {
        
        public decimal Amount { get; set; }        
        public DateTime TransDate { get; set; }        
        public Boolean CheckAdvancedLiquidated { get; set; }
    }
}
