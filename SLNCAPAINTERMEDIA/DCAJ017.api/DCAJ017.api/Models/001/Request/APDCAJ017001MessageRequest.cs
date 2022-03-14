/*
 Objetivo: Almacena variables de APDCAJ017001MessageRequest
 Archivo: APDCAJ017001MessageRequest.cs
 Versión: 1.0
 Creación: 07/03/2022
 Autor: Solange Moncada
*/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ017.api.Models._001.Request
{
    public class APDCAJ017001MessageRequest
    {        
        public string DataAreaId { get; set; }
        public string Environment { get; set; }
        public string SessionId { get; set; }        
        public string CustAccount { get; set; }        
        public string InputCustId { get; set; }        
        public string OperationId { get; set; }        
        public decimal Amount { get; set; }        
        public string PostingProfile { get; set; }        
        public string PostingProfileAdvance { get; set; }
        public APInformationalFields APInformationalFields { get; set; }     
        public APLedgerJournalLineSalesOrder APLedgerJournalLineSalesOrder { get; set; }
        public APVendTransRegistrationFine APVendTransRegistrationFine { get; set; }

    }
}
