using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._007.Request
{
    public class APIPRO005007MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }
       
        public string SessionId { get; set; }
       
        public string Enviroment { get; set; }
        public string ItemId { get; set; }
        [Required]
        public List<string> InventLocationList { get; set; }
        [Required]
        public string DateStart { get; set; } // DateTime
        [Required]
        public string DateEnd { get; set; } //DateTime

    }
}
