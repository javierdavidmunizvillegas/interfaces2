using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA004.api.Models._001.Request
{
    public class APBuyerNumberTickectList
    {
        public string APBuyerNumberTickect { get; set; }
        public bool APBuyerTicketCheck { get; set; }
        public decimal APAmountGeneratedBuyerTicket { get; set; }
        public bool APCreditNoteBuyerTicketCheck { get; set; }
        public string APPostingProfile { get; set; }
    }
}
