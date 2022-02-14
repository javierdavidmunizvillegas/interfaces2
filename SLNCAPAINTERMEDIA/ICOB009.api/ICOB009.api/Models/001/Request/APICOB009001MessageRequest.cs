using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB009.api.Models._001.Request
{
    public class APICOB009001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }

        [Required]
        public APDationRequests APDationRequests { get; set; }
    }
}
