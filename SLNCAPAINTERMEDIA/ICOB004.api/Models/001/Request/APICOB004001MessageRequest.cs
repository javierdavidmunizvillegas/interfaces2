using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB004.api.Models._001.Request
{
    public class APICOB004001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }//Id de la compañía 
        public string Enviroment { get; set; }//Entorno
        public string SessionId { get; set; }//Id de sesión
        public List<APDocumentNCRequest> DocumentNCList { get; set; }//Id de la compañía 
        
    }
}
