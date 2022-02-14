using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC015.api.Models.Request
{
    public class APDeliveryProductQuery
    {
        public string CustAccount { get; set; }
        public string Invoice { get; set; }
        public string ItemId { get; set; }
        public string DeliveryType { get; set; }
    }
}
