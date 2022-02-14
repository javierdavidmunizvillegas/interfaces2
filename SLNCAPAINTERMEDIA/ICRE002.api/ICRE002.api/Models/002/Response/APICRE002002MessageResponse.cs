using ICRE002.api.Models._002.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE002.api.Models._002.Response
{
    public class APICRE002002MessageResponse
    {
        
        public string SessionId { get; set; }//Id de sesión

        
        public APIndependetEntrepContractICRE001002 APIndependetEntrep { get; set; }//Objeto Emprendedor Independiente
        
        
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }
    }
}
