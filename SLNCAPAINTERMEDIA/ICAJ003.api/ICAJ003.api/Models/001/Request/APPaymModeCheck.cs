using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ003.api.Models._001.Request
{
    public class APPaymModeCheck
    {
        public string APStoreId { get; set; }        
        public string DocumentNumber { get; set; }        
        public string AccountNum { get; set; }        
        public DateTime DueDateCheck { get; set; }        
        public decimal Amount { get; set; }        
        public string CustAccount { get; set; }        
        public string Description { get; set; }        
        public string[] InvoiceIdList { get; set; }        
        public string PostingProfile { get; set; }        
        public DateTime PaymDate { get; set; }             
        public string APBoxCode { get; set; }        
        public string APUserGeneralDocument { get; set; }        
        public string ApTransactionType { get; set; }
        public string BankId { get; set; }
        
    }
}
