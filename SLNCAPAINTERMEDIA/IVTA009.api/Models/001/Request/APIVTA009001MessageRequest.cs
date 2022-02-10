using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA009.api.Models._001.Request
{
    public class APIVTA009001MessageRequest
    {
        [Required]
        public string Enviroment { get; set; }
        [Required]
        public APInventTableIVTA009001 ApInventTable { get; set; }
        public string SessionId { get; set; }
        [Required]
        public string DataAreaId { get; set; }
    }
}
