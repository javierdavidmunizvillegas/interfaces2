using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ004.api.Models.Request
{
    public class APLedgerJournalCashDepositsRequest
    {
        
        public DateTime PreparationDate { get; set; }        
        public DateTime DateCollection { get; set; }        
        public string CollectionEntry { get; set; }        
        public decimal CollectionAmount { get; set; }        
        public string PaymentCollection { get; set; }        
        public string Bank { get; set; }        
        public string DepositNumber { get; set; }        
        public string UserDeposit { get; set; }
    }
}
