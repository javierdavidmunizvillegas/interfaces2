using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB003.api.Models._001.Request
{
    public class APICOB003001MessageRequest
    {
        public string DataAreaId { get; set; }//Id de la compañía 
        public string Enviroment { get; set; } //Id del ambiente
        public APLedgerJournalReverse APLedgerJournalRevese { get; set; }//Objeto con el diario a reversar
        public string SessionId { get; set; } //Id de sesión Guid

    }
}
