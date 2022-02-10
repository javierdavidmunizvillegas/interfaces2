using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA011.Models._002.Request
{
    class APIVTA011002MessageRequest
    {
        // 002 Ventas MULTINOVA (Contado/Crédito y las Devoluciones) 
        public string DataAreaId { get; set; }//Id de la Compañia
        public string Enviroment { get; set; }//Entorno

        public string SessionId { get; set; }//id de la sesion Guid
        public List<APCustInvoiceHeader> ApCustInvoiceHeaderList { get; set; }//Listado de ventas facturadas Multinova


    }
}
