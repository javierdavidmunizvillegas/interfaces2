using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._002.Response
{
    public class APItemsContractIPRO001002
    {
       
        public string ItemId { get; set; }
      
        public List<APListMaterialstList002> APListMaterialsList { get; set; }
    }
}
