using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE006.api.Models._001.Request
{
    public class APICRE006001MessageRequest
    {
        public string VATNum { get; set; }        
        public string DataAreaId { get; set; }
        public string SessionId { get; set; }
        public string Enviroment { get; set; }
        
    }
    
}
