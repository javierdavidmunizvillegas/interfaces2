/*
 Objetivo: Almacena variables de APDCAJ017002MessageRequest
 Archivo: APDCAJ017002MessageRequest.cs
 Versión: 1.0
 Creación: 07/03/2022
 Autor: Solange Moncada
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ017.api.Models._002.Request
{
    public class APDCAJ017002MessageRequest
    {
        
        public string DataAreaId { get; set; }
        public string Environment { get; set; }
        public string SessionId { get; set; }        
        public string CustAccount { get; set; }        
        public string InputCustId { get; set; }        
        public string OperationId { get; set; }        
        public decimal Amount { get; set; }        
        public string AccountType { get; set; }        
        public string BankId { get; set; }        
        public string PaymReference { get; set; }                
        public DateTime TransDate { get; set; }
        public string BankTransType { get; set; }
    }
}
