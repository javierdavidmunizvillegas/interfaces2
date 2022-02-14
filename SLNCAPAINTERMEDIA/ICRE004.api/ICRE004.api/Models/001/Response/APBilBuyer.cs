using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE004.api.Models._001.Response
{
    public class APBilBuyer
    {
        public decimal APAmountBillBuyer { get; set; }
        public decimal APAmountGeneratBillBuyer { get; set; }
        public Boolean APBillBuyer { get; set; }        
        public Boolean APBillBuyerNC { get; set; }
        public string APBillBuyerNumber { get; set; }
        public string APBillBuyerProvisionId { get; set; }
        
        
    }
}
