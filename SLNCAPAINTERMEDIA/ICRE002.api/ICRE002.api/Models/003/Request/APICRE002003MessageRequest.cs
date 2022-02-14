using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE002.api.Models._003.Request
{
    public class APICRE002003MessageRequest
    {
        
        public string DataAreaId { get; set; }

        public string Enviroment { get; set; }
       
        public string SessionId { get; set; }

        
        public List<APIndependetEntrepContractICRE001001> APIndependetEntrep { get; set; }
    }
    
}
