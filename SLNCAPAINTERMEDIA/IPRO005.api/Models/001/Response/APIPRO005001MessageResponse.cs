using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._001.Response
{
    public class APIPRO005001MessageResponse
    {   
        
        public string SessionId { get; set; }
       
        public List<APItemsContract> APItemsContractList { get; set; }
      
        public bool StatusId { get; set; }
       // public string TimeStartEnd { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
