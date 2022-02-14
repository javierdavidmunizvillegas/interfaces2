using ICAJ003.api.Models._001.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ003.api.Models
{
    public class APLedgerJournalTableCustPaym
    {
        public string JournalNum { get; set; }        
        public string Name { get; set; }        
        public List<APLedgerJournalLine> APLedgerJournalLineList { get; set; }
        public string InputCustId { get; set; }
    }
}
