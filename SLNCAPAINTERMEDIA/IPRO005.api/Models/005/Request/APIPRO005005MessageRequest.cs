using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._005.Request
{
    public class APIPRO005005MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }
       
        public string SessionId { get; set; }
      
        public string Enviroment { get; set; }
        [Required]
        public List<string> InventLocationList { get; set; }
        [Required]
        public string DateStart { get; set; } // DateTime
        [Required]
        public string DateEnd { get; set; }//DateTime
    }
}
