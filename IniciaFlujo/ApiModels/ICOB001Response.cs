using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class ICOB001Response:Base
    {
        public string SessionId { get; set; }
        public List<APSettlementTransactionHeader> APSettlementTransactionHeaderList { get; set; }
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }

        public ICOB001Response()
        {
            APSettlementTransactionHeaderList = new List<APSettlementTransactionHeader>();
        }
    }
}
