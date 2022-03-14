/*
 Objetivo: Almacena variables de APDCAJ017001MessageResponse
 Archivo: APDCAJ017001MessageResponse.cs
 Versión: 1.0
 Creación: 07/03/2022
 Autor: Solange Moncada
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ017.api.Models._001.Response
{
    public class APDCAJ017001MessageResponse
    {
        
        public string SessionId { get; set; }        
        public Boolean StatusId { get; set; }
        public List<string> ErrorList { get; set; }        
        public string CustAccount { get; set; }        
        public string InputCustId { get; set; }        
        public string OperationId { get; set; }        
        public string InventTransId { get; set; }        
        public decimal Amount { get; set; }        
        public string CheckStatus { get; set; }
    }
}
