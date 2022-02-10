using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA010.Models._001.Request
{
    public class APIVTA010001MessageRequest
    {
        public string DataAreaId { get; set; }//Id de la compañía 
        public string Enviroment { get; set; }//Ambiente
        public string SessionId { get; set; }//Id de sesión Guid
        public List<APCustInvoiceJourIVTA010001> APCustInvoiceJourList { get; set; }//Listado factura 



    }
}
