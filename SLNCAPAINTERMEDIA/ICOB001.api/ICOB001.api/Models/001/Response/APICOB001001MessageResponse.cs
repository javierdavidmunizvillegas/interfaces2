using ICOB001.api.Models._001.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB001.api.Models._001.Response
{
    public class APICOB001001MessageResponse
    {
        [Required]
        public string SessionId { get; set; }

        [Required]
        public List<APSettlementTransactionHeader> APSettlementTransactionHeaderList { get; set; }

        [Required]
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }
    }
}
