using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ005.api.Models._001.Response
{
    public class APLedgerJournalDepositsResponse
    {
       
        public decimal CollectionAmount { get; set; }
      
        public string DepositNumber { get; set; }
       
        public string Voucher { get; set; }
    }
}
