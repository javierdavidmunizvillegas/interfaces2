using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ024.api.Models._001.Response
{
    public class APILOG002001MessageResponse
    {
        public decimal AmountDebitNote { get; set; }
        public DateTime DateDebitNote { get; set; } // DateTime
        public List<APDocumentCreditNoteResponse> DocumentCreditNoteList { get; set; }
        public List<string> ErrorList { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string NumDebitNote { get; set; }
        public string SessionId { get; set; }
        public bool StatusId { get; set; }
        public string VoucherDebitNote { get; set; }
        
    }
}
