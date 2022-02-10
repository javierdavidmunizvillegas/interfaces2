using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA011.Models._002.Response
{
    class APIVTA011002MessageResponse
    {
        public string SessionId { get; set; } //Guid . Id de sesion
        public bool StatusId { get; set; } // Estado true = ok y False = Error
        public List<string> ErrorList { get; set; } // Listado de errores 
    }
}
