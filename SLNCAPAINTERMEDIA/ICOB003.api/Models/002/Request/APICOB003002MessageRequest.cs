using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB003.api.Models._002.Request
{
    public class APICOB003002MessageRequest
    {
        public string DataAreaId { get; set; }//Id de la compañía 
        public string Enviroment { get; set; }//Ambiente
        public List<APCustInvoiceTable> APCustInvoiceTableList { get; set; }//Objeto factura de servicios o nota de debito
        public string SessionId { get; set; }//Id de sesión Guid

    }
}
