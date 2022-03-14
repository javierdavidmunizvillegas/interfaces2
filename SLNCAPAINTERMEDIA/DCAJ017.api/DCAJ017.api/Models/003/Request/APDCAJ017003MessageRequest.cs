/*
 Objetivo: Almacena variables de APDCAJ017003MessageRequest
 Archivo: APDCAJ017003MessageRequest.cs
 Versión: 1.0
 Creación: 07/03/2022
 Autor: Solange Moncada
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ017.api.Models._003.Request
{
    public class APDCAJ017003MessageRequest
    {
        
        public string DataAreaId { get; set; }
        public string Environment { get; set; }
        public string SessionId { get; set; }        
        public string CustAccount { get; set; }        
        public string InputCustId { get; set; }        
        public string OperationId { get; set; }        
        public string CheckStatus { get; set; }        
        public string ReasonCode { get; set; }        
        public string ReasonDescription { get; set; }
        public string SalesId { get; set; }
        public string Voucher { get; set; }
        public APAditionalInformationContract APAditionalInformationContract { get; set; }        
    }
}
