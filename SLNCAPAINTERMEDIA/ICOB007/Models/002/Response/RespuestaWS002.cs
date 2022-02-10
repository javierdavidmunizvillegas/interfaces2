using System;
using System.Collections.Generic;
using System.Text;

namespace ICOB007.Models._002.Response
{
    class RespuestaWS002
    {
        public int codigoTransaccion { get; set; }//Código respuesta transacción
        public string estadoTransaccion { get; set; }//Resultado transacción
        public string descripcionTransaccion { get; set; }//Descripción transacción
        public List<string> ordenesTrabajo { get; set; }//Lista de Número de OT
    }
}
