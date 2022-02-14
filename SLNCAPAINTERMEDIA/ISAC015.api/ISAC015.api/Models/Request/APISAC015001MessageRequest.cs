using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC015.api.Models.Request
{
    public class APISAC015001MessageRequest
    {        
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }        
        public APDeliveryProductQuery DeliveryProductQuery { get; set; }
    }
}
