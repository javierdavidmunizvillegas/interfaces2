using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC021.api.Models._001.Request
{
    public class APSalesLine
    {        
        public string ItemId { get; set; }        
        public decimal Qty { get; set; }        
        public decimal SalesPrice { get; set; }
        public decimal LineDisc { get; set; }
        public decimal APPromotionDiscount { get; set; }
        public decimal APSalesCost { get; set; }
        public string APECInvoiceDetail { get; set; }        
        public List<APFinancialDimension> APFinancialDimensionList { get; set; }
    }
}
