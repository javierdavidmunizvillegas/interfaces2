using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB004.api.Models._002.Request
{
    public class APICOB004002MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }//Id de la compañía 
        public string Enviroment { get; set; }//Entorno
        public string SessionId { get; set; }//Id de sesión
        [Required]
        public string APTypeTransaction { get; set; }//Tipo de transacción
        public List<APDocumentLedgerRequest> DocumentLedgerList { get; set; }//Documentos de diario contable


    }
}
