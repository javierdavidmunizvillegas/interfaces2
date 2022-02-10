using IVTA010.Models._001.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace IVTA010.Models._001
{
    public class RespuestaWS
    {
        public string numDocumentosSiGenerados { get; set; }//nemreo de documentos generados
        public List<string> DetalleDocumentosSiGenerados { get; set; }//documentos SI generado
        public string NoProcesadas { get; set; }//Descripción transacción
        public List<string> DetalleDocumentosNoGenerados { get; set; }//documentos NO generado

        /* public List<string> DetalleDocumentosSiGenerados { get; set; }//Resultado transacción
         public string error { get; set; }//Descripción transacción
         public string DetalleError { get; set; }//Descripción transacción*/
    }
}
