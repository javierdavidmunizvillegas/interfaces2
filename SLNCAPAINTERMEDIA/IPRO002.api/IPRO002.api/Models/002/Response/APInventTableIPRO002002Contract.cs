using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO002.api.Models._002.Response
{
    public class APInventTableIPRO002002Contract
    {
        public string ItemId { get; set; }
        public List<APInventTableLMATIPRO002002Contract> APInventTableLMATMaterials { get; set; }
       
    }
}
