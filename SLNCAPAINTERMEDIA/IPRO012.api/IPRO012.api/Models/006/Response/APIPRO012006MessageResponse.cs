using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO012.Models._006
{
    public class APIPRO012006MessageResponse
    {
        [Required]
        public string SessionId { get; set; }

        [Required]
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }

        public List<APInventLocation> APInventLocationList { get; set; }
    }
}
