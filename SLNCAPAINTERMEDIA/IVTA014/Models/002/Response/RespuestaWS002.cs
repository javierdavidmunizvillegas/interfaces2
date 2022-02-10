using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA014.Models._002.Response
{
    class RespuestaWS002
    {
        public string statusCode { get; set; }//Código respuesta transacción
        public string response { get; set; }//Resultado transacción
        public string descripcionId { get; set; }//Descripción transacción
        public List<string> errorList { get; set; }//Descripción transacción
    }
}
