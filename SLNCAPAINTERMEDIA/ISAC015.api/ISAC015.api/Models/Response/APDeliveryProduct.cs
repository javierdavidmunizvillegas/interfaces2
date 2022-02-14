using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC015.api.Models.Response
{
    public class APDeliveryProduct
    {
        public string CustAccount { get; set; }
        public string CustName { get; set; }
        public string Invoice { get; set; }
        public string SalesId { get; set; }
        public List<APDeliveryProductDetail> DeliveryProductDetail { get; set; }
    }
}
