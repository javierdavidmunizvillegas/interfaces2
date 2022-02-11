using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA003.api.Models._004.Response
{
    public class APIVTA003004MessageResponse
    {
        [Required]
        public string SessionId { get; set; }

        public List<APInventLocationIVTA003004> APInventlocationList { get; set; }

        [Required]
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }
    }
}
