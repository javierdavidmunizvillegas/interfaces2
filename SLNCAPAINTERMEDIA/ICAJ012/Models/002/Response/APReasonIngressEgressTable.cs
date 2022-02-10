using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ012.api.Models._002.Response
{
    public class APReasonIngressEgressTable
    {
       
        public string APReasonIngressEgressId { get; set; }
        public string Description { get; set; }
        public string MainAccountId { get; set; }
        public string PostingProfile { get; set; }
       
        public string LedgerJournalName { get; set; }

    }
}
