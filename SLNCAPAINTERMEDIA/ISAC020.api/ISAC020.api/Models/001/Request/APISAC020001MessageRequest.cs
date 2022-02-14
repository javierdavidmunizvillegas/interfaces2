using ISAC020.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC020.Models
{
    public class APISAC020001MessageRequest
    {
        
        public string DataAreaId { get; set; }
        
        public string Enviroment { get; set; }

        public string SessionId { get; set; }

        
        public APInventTransferTable APInventTransferTableISAC020001 { get; set; }
    }
    
}
