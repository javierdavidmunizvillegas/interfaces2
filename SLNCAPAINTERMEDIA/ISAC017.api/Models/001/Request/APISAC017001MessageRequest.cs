using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.api.Models._001.Request
{
    public class APISAC017001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }
        public string SessionId { get; set; }
        
        public string Enviroment { get; set; }  
        [Required]
        public APSalesTableReturn ReturnTable { get; set; }
    }
}
