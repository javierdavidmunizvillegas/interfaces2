using System;
using System.Collections.Generic;
using System.Text;

namespace ISAC018.Models._001.Response
{
    class RespuestaWS
    {
        public string codigoTransaccion { get; set; }//Código respuesta transacción
        public string estadoTransaccion { get; set; }//Resultado transacción
        public string descripcionTransaccion { get; set; }//Descripción transacción
    }
}
