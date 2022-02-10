using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA009.api.Models._001.Response
{
    public class APIVTA009001MessageResponse
    {
        [Required]
        public string SessionId { get; set; }
        [Required]
        public bool StatusId { get; set; }
        public string TimeStartEnd { get; set; }
        public List<string> ErrorList { get; set; }

    }
}
