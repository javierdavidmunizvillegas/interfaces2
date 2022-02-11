using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB001.api.Models._001.Request
{
    public class APICOB001001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }

        public string Enviroment { get; set; }

        [Required]
        public List<APSettlementTransactionHeader> APSettlementTransactionHeaderList { get; set; }

        [Required]
        public string CustAccount { get; set; }

        public string SessionId { get; set; }
    }
}
