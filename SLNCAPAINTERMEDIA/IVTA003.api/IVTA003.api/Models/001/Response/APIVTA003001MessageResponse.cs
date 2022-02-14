using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA003.api.Models._001.Response
{
    public class APIVTA003001MessageResponse
    {
        [Required]
        public string SessionId { get; set; }

        public List<APLogisticsAddress> APCustTableList { get; set; }

        [Required]
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }
    }
}
