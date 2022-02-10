using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ009.api.Models._001.Response
{
    public class APCustInvoiceServiceResponse
    {
        public string IDTransaction { get; set; } //ID. Transacción Voucher String(20)

        public string NumberDocument { get; set; } // No documento InvoiceId String(20)
        public string DocumentApplies { get; set; } //Documento que modifica APECDocumentApplies String(20)

        public Decimal Value { get; set; } // Valor Amount Decimal
        public string ListIdentifier  { get; set; }
        public List<APCustInvoiceServiceLineResponseList001> APCustInvoiceServiceLineResponseList { get; set; }

    }
}
