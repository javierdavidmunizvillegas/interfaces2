using System;
using System.Collections.Generic;
using System.Text;

namespace APICAJ008002.Models._002.Response
{
    public class RespuestaWSOT
    {
        public RespuestaWSOT()
        {
            NumberOT = string.Empty;

        }
        public string NumberOT { get; set; }//Número de OT

        public RespuestaWS001 Respuesta { get; set; }
    }
    public class RespuestaWS001
    {
        public string CodigoTransaccion { get; set; }//Código respuesta transacción
        public string EstadoTransaccion { get; set; }//Resultado transacción
        public string DescripcionTransaccion { get; set; }//Descripción transacción

    }
}
