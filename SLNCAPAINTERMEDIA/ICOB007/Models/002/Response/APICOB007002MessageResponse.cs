using System;
using System.Collections.Generic;
using System.Text;

namespace ICOB007.Models._002.Response
{
    class APICOB007002MessageResponse
    {
        public string SessionId { get; set; }//Id de sesión
        public Boolean StatusId { get; set; }//Descripción "True" o "False"
        public List<string> ErrorList { get; set; }//Listado de errores
        public List<string> NumberOTList { get; set; }//Lista de Número de OT

    }
}
