using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO005.api.Models._001.Request
{
    public class APIPRO005001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
       
        public string SessionId { get; set; }
        public List<string> ItemIdList { get; set; }

    }
}
