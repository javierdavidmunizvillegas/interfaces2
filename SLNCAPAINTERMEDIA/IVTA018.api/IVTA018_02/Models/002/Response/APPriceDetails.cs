using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA018_02.api.Models._002.Response
{
    public class APPriceDetails
    {
        public string CodeError { get; set; }
        public string Description { get; set; }
        public int DescriptionCod { get; set; }
        public string DescriptionError { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal Percent { get; set; }        
        public decimal TotalValue { get; set; }
        public string TradeAgreement { get; set; }
    }
}
