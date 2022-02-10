using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ020.api.Models._002.Request
{
    public class APDocumentRetentionRequest
    {
        public string NumberRetention { get; set; } //Número de comprobante
        public string TransDate { get; set; } //Fecha de la retención DateTime
        public string AuthorizationNumberRet { get; set; } //Número de autorización

    }
}
