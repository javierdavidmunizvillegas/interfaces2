using System;
using System.Collections.Generic;
using System.Text;

namespace APICAJ008002.Models._003.Response
{
    class RespuestaWS003
    {
        public string StatusCode { get; set; }//Estado true = ok y False = Error
        public List<string> MessageError { get; set; }//Listado de errores
        public List<ResponseCarteraDetalle>  OperationList { get; set; }

    }
}
