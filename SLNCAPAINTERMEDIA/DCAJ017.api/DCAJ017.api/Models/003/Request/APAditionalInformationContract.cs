using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ017.api.Models._003.Request
{
    public class APAditionalInformationContract
    {
        
        public string InvoiceId { get; set; }        
        public string CPN { get; set; }        
        public string CustAccount { get; set; }        
        public decimal Amount { get; set; }        
        public string Reason { get; set; }        
        public string BusinessUnit { get; set; }        
        public string InventTransId { get; set; }
       
    }
}
