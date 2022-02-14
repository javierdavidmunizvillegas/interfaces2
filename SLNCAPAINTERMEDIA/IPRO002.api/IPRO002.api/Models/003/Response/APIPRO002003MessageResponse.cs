using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO002.api.Models._003.Response
{
    public class APIPRO002003MessageResponse
    {
        [Required]
        public string SessionId { get; set; }

        public List<APInventTableIPRO002003Contract> APInventTableList { get; set; }

        [Required]
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }
    }
}
