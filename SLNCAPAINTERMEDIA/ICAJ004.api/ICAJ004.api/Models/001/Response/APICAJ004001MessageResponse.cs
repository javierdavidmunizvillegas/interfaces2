using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ004.api.Models.Response
{
    public class APICAJ004001MessageResponse
    {
        public List<APLedgerJournalCashDepositsResponse> ledgerJournalCashDepositsResponseList { get; set; }
        public string TimeStartEnd { get; set; }        
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
