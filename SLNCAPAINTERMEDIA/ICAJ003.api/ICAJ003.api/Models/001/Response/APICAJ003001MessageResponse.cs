using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ003.api.Models._001.Response
{
    public class APICAJ003001MessageResponse
    {
        
        public string SessionId { get; set; }
        public  List<APLedgerJournalTableCustPaym> APLedgerJournalTableCustPaym { get; set; }        
        public Boolean StatusId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
