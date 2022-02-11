using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA003.api.Models._005.Response
{
    public class APIVTA003005MessageResponse
    {
        [Required]
        public string SessionId { get; set; }

        public List<APLogisticsAddressIVTA003004> APLogisticsAddressDistrictList { get; set; }

        [Required]
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }
    }
}
