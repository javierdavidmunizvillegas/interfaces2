using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO012.Models._001
{
    public class APIPRO012001MessageResponse
    {
        [Required]
        public string SessionId { get; set; }

        [Required]
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }

        [Required]
        public List<APTAMFundTable> APTAMFundTableContract { get; set; }
    }
}
