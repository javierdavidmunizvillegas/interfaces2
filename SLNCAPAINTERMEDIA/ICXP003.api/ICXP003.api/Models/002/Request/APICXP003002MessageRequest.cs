using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICXP003.api.Models._002.Request
{
    public class APICXP003002MessageRequest
    {
      
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }           
        public string SessionId { get; set; }      
        public List<APLedgerInvoiceContract> APLedgerInvoiceContractList { get; set; }
    }
}
