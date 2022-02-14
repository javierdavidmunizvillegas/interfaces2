using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB009.api.Models._001.Response
{
    public class APICOB009001MessageResponse
    {
        [Required]
        public string SessionId { get; set; }

        [Required]
        public Boolean StatusId { get; set; }

        public List<string> ErrorList { get; set; }
        public List<APDationResponse> APDationResponse { get; set; }
    }
}
