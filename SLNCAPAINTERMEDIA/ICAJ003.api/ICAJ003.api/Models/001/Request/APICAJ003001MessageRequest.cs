using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ003.api.Models
{
    public class APICAJ003001MessageRequest
    {
        
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public APLedgerJournalTableCustPaym APLedgerJournalTableCustPaym { get; set; }
        public string SessionId { get; set; }
    }
}
