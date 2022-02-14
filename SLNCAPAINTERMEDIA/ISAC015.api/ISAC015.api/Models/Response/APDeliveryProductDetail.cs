using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC015.api.Models.Response
{
    public class APDeliveryProductDetail
    {
        public string AddressDelivery { get; set; }
        public DateTime DateDelivery { get; set; }
        public string Delivery { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Qty { get; set; }
        public string ReasonDeliveryFail { get; set; }
        public string Status { get; set; }
        public string StoreDelivery { get; set; }
        
    }
}
