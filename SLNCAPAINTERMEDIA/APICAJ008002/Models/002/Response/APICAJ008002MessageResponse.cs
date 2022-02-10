using System;
using System.Collections.Generic;
using System.Text;

namespace APICAJ008002.Models._002.Response
{
    class APICAJ008002MessageResponse
    {
        public string SessionId { get; set; }//Id de sesión
        public Boolean StatusId { get; set; }//Descripción "True" o "False"
        public List<string> ErrorList { get; set; }//Listado de errores
        public string NumberOT { get; set; }//Número de OT instalación

    }
}
