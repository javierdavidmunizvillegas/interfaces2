using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA019.api.Models.Request
{
    public class APPurchaserTicketContract
    {
        public decimal PurchaserTicketAmount { get; set; }
        public string PurchaserTicketNumber { get; set; }
        public string PurchaserTicketProvisionId { get; set; }
        
    }
}
