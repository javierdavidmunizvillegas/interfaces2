using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ024.api.Models._001.Request
{
    public class APICAJ024001MessageRequest
    {
        
        public string DataAreaId { get; set; }        
        public string Enviroment { get; set; }
        public string SessionId { get; set; }        
        public decimal AmountDebitNote { get; set; }        
        public string CustAccount { get; set; }        
        public string DateCreditNote { get; set; } // datetime        
        public List<APDocumentLiquidateRequest> DocumentLiquidateList { get; set; }        
        public string NumCreditNote { get; set; }        
        public string RubroSIAC { get; set; }        
        public string VoucherCreditNote { get; set; }    
        
    }
}
