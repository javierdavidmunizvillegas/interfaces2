using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ005.api.Models._001.Response
{
    public class APICAJ005001MessageResponse
    {
         public List<APLedgerJournalDepositsResponse> LedgerJournalDepositsResponseList { get; set; }
        //public List<LedgerJournalCashDepositsResponseList> LedgerJournalCashDepositsResponseList { get; set; }

        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }
        

    }
}
