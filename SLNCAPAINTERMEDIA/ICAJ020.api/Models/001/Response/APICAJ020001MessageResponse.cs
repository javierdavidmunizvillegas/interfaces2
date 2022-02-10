using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ020.api.Models._001.Response
{
    public class APICAJ020001MessageResponse
    {
        public string SessionId { get; set; } // Id de sesión
        public Boolean StatusId { get; set; } //Descripción "True" o "False"
        public List<string> ErrorList { get; set; } // Listado de errores

        public List<APDocumentResponseList> DocumentResponseList { get; set; } // Lista de paquetes shippify

    }
}
