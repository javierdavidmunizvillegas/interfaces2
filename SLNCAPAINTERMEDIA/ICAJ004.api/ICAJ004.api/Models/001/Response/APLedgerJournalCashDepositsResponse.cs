using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ004.api.Models.Response
{
    public class APLedgerJournalCashDepositsResponse
    {
        
        public decimal CollectionAmount { get; set; }        
        public string DepositNumber { get; set; }        
        public string Voucher { get; set; }
    }
}
