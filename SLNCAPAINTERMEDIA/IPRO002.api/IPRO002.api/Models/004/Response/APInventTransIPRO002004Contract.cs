using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO002.api.Models._004.Response
{
    public class APInventTransIPRO002004Contract
    {
        public string ItemId { get; set; }
        public DateTime TransDate { get; set; }
        public string Style { get; set; }
        public string InventLocationId { get; set; }
    }
}
