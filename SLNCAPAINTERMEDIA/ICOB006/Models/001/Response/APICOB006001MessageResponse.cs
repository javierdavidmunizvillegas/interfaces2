using ICOB006.Models._001.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ICOB006.Models._001.Response
{
    class APICOB006001MessageResponse
    {
        public string SessionId { get; set; }//Id de sesión
        public Boolean DescriptionId { get; set; }//Descripción "True" o "False"
        public List<string> ErrorList { get; set; }//Listado de errores
        public string NumCheque { get; set; }//Número de cheque
        public string Response { get; set; }//Respuesta de SIAC



    }
}
