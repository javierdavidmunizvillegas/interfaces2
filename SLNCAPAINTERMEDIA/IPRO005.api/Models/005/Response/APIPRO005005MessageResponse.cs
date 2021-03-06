using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._005.Response
{
    public class APIPRO005005MessageResponse
    {
        
        public string SessionId { get; set; }
      
        public List<APTransfersSearchContract> APTransfersSearchContractList  { get; set; }
       // public string TimeStartEnd { get; set; }

        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
