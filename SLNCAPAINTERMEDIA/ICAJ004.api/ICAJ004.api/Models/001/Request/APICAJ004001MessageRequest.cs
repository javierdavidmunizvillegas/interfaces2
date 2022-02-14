using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ004.api.Models.Request
{
    public class APICAJ004001MessageRequest
    {
        
        public string DataAreaId { get; set; }        
        public string SessionId { get; set; }        
        public string Enviroment { get; set; }        
        public List<APLedgerJournalCashDepositsRequest> LedgerJournalCashDepositsRequestList { get; set; }
    }
    
}
