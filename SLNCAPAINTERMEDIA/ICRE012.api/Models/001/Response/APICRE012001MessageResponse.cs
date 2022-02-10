using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE012.api.Models._001.Response
{
    public class APICRE012001MessageResponse
    {
        
        public string SessionId { get; set; }
        public List<APInvoice> APInvoice { get; set; }
        public List<APJournalTable> APJournalTable { get; set; }
        
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
