using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB003.api.Models._003.Request
{
    public class APICOB003003MessageRequest
    {
        public string DataAreaId { get; set; }//Id de la compañía 
        public string Enviroment { get; set; }//Ambiente
        public string APTypeTransaction { get; set; }//
        public List<DocumentLedgerList> DocumentLedgerList { get; set; }//Objeto factura de servicios o nota de debito
        public string SessionId { get; set; }//Id de sesión Guid

    }
}
