using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._004.Request
{
    public class APIPRO005004MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        
        public string SessionId { get; set; }
        public List<string> ItemIdList { get; set; }
    }
}
