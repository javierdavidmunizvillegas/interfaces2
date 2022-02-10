using System;
using System.Collections.Generic;
using System.Text;

namespace APICAJ008002.Models._002.Request
{
    class APICAJ008002MessageRequest
    {
        public List<APWorkOrderTableICAJ008002> DocumentInvoiceList { get; set; }//Listado de documentos de facturación

        public string SessionId { get; set; }//Session Id
        public string DataAreaId { get; set; }//Id de la empresa
        
       
      
    }
}
