using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO004.Models
{
    public class APIPRO004001MessageResponse
    {
        [Required]
        public string SessionId { get; set; }

        [Required]
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }

        public List<APInventTableIPRO004001> APInventTableList { get; set; }
    }
}
