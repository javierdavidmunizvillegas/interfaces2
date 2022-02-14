using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE002.api.Models._002.Request
{
    public class APICRE002002MessageRequest
    {
        
        public string DataAreaId { get; set; }

        public string Enviroment { get; set; }

        public string SessionId { get; set; }

        
        public APIndependetEntrepContractICRE001002 APIndependetEntrep { get; set; }
      
    }
}
