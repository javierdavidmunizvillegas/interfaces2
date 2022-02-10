using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.api.Models._004.Response
{
    public class APCreditNoteHeader
    {
        public string CustAccount { get; set; } // Código del cliente
        public string VatNum { get; set; } // Identificación de cliente
        public string CreditNoteNum { get; set; } // Número de la Nota de crédito
        public string CreditNoteDate { get; set; } // Fecah de Nota de crédito
        public string ReturnNum { get; set; } //Número de orden de devolución (ADM)
        public string InvoiceNum { get; set; } // Factura origen
        public string ReasonRefund { get; set; } //Motivo de NC
        public string Voucher { get; set; } //Número de voucher
        public decimal CreditNoteAmount { get; set; } // Código del cliente
        public List<APCreditNoteLine> ItemReturnList { get; set; } // Listado código de disposicion




    }
}
