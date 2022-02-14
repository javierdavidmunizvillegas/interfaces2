using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB009.api.Models._001.Response
{
    public class APDationLineResponse
    {
        public string ItemId { get; set; }
        public string Series { get; set; }
        public string ItemName { get; set; }
        public string Brand { get; set; }
        public string Line { get; set; }
        public string Group { get; set; }
        public string Subgroup { get; set; }
        public string Capacity { get; set; }
        public string NumberOT { get; set; }     
        public string QualifiedItem { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public string Qualified { get; set; }
        public string Observation { get; set; }

    }
}
