using System;
using System.Collections.Generic;
using System.Text;

namespace ICOB007.Models._002.Request
{
    public class APICOB007002MessageRequest
    {
        public string DataAreaId { get; set; }//Id de la compañía
        public string Enviroment { get; set; }//Entorno
        public string SessionId { get; set; }//Id de sesión
        public List<APDacionQualifiedRequest> DocItemList { get; set; }//Lista de artículos calificados

    }
}
