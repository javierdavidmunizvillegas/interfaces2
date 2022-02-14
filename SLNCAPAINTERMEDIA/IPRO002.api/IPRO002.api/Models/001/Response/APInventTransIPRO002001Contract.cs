using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO002.api.Models._001.Response
{
    public class APInventTransIPRO002001Contract
    {
        public string ItemId { get; set; }
        public decimal Cost { get; set; }
        public DateTime TransDate { get; set; }
        public string ProductLifecycleStateId { get; set; }
    }
}
