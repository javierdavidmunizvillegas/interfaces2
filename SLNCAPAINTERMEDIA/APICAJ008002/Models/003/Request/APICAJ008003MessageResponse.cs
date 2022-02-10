using System;
using System.Collections.Generic;
using System.Text;

namespace APICAJ008002.Models._003.Request
{
    class APICAJ008003MessageResponse
    {
        public string DataAreaId  { get; set; }//Id de la compañía 
        public List<APDocumentInvoiceRequestTableICAJ008001> DocumentInvoiceRequestTableList { get; set; }//Listado de pedidos

    }
}
