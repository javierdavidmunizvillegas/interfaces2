using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE004.api.Models._001.Response
{
    public class APICRE004001MessageResponse
    {
        [Required]
        public string SessionId { get; set; }
        public List<APSalesOrderICRE004001> APSalesOrderList { get; set; }
        [Required]
        public Boolean StatusId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
