using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA014.Models._001.Request
{
    public class APIVTA014001MessageRequest
    {
        public string DataAreaId { get; set; } //Id de la compañía 
        public string Enviroment { get; set; } // Ambiente
        public List<APCustInvoiceJourIVTA014001> APCustInvoiceJourList { get; set; } // Listado factura 
        public string SessionId { get; set; } //Id de Sesion

    }
}
