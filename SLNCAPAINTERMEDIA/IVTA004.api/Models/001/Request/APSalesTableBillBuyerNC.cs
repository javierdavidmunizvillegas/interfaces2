using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA004.api.Models._001.Request
{
    public class APSalesTableBillBuyerNC
    {
        public string APBillBuyerProvisionId { get; set; }
        public string APBillBuyerNumber { get; set; }
        public decimal APAmountBillBuyer { get; set; }
    }
}
