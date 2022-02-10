
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ012.api.Models._002.Request
{
    public class APICAJ012002MessageRequest
    { 
        [Required]
        public string DataAreaId { get; set; }
        
        public string Enviroment { get; set; }
       
        public string SessionId { get; set; }
      


    }
}
