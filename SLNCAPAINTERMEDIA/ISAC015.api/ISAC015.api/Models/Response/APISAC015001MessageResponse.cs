using ISAC015.api.Models.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC015.api.Models.Response
{
    public class APISAC015001MessageResponse
    {        
        public string SessionId { get; set; }        
        public Boolean StatusId { get; set; }        
        public List<string> ErrorList { get; set; }
        public List<APDeliveryProduct> DeliveryProduct { get; set; }
    }
}
