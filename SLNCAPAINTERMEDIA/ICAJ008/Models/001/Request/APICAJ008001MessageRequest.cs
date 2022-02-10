using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ008.api.Models._001.Request
{
    public class APICAJ008001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }//Id de la compañía 
        public string Enviroment { get; set; }//Entorno

        public string SessionId { get; set; }//id de la sesion
        [Required]
        public List<APDocumentInvoiceTableICAJ008001> APDocumentInvoiceTableICAJ008001 { get; set; }//Listado de documentos de facturación
    }
}
