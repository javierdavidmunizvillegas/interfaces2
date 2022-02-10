using System;
using System.Collections.Generic;
using System.Text;

namespace ICOB006.Models._001.Response
{
    class RespuestaWS001
    {
        public string numeroCheque { get; set; }//fecha incial
        public string codigoTransaccion { get; set; }//Código respuesta   transacción
        public string estadoTransaccion { get; set; }//ok 
        public string descripcionTransaccion { get; set; }//Estado true = ok y False = Error
        

    }
}
