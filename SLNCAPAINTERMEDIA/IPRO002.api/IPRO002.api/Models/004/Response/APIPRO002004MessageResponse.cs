using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO002.api.Models._004.Response
{
    public class APIPRO002004MessageResponse
    {
        [Required]
        public string SessionId { get; set; }

        public List<APInventTransIPRO002004Contract> APInventTransList { get; set; }

        [Required]
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }
    }
}
