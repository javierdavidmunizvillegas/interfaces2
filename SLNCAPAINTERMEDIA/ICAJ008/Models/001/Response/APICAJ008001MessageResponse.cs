using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ008.api.Models._001.Response
{
    public class APICAJ008001MessageResponse
    {
        public string SessionId { get; set; }//Id de sesión
        public string DataAreaId { get; set; }
        public Boolean StatusId { get; set; }//Descripción "True" o "False"
        public List<string> ErrorList { get; set; }//Listado de errores
        public List<APDocumentInvoiceRequestTableICAJ008001> DocumentInvoiceRequestTableList { get; set; }//Listado de pedidos


    }
}
