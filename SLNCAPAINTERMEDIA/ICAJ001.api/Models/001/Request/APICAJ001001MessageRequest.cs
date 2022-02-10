using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ001.Models._001.Request
{
    public class APICAJ001001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }

        public string SessionId { get; set; }

        public string Enviroment { get; set; }
    }
}
