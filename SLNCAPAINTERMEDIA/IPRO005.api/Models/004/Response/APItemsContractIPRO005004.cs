using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._004.Response
{
    public class APItemsContractIPRO005004
    {
        
        public string ItemId { get; set; }
      
        public List<APInventorySearchContract> APInventorySearchContractList { get; set; }
    }
}
