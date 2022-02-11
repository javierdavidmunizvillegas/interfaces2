using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE002.api.Models
{
    public class APICRE002001MessageRequest
    {
        
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }        
        public APCustTableContract APCustTable { get; set; } 
    }
    
}
