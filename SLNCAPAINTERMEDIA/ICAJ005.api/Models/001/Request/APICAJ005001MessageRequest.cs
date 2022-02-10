using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ005.api.Models._001.Request
{
    public class APICAJ005001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }
        public string SessionId { get; set; }
        
        public string Enviroment { get; set; }

        public List<APLedgerJournalDepositsRequest> LedgerJournalDepositsRequestList { get; set; }
        //[Required]
//public List<LedgerJournalCashDepositsRequestList> LedgerJournalCashDepositsRequestList { get; set; }

    }
}
