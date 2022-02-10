using System;
using System.Collections.Generic;
using System.Text;

namespace ICOB006.Models._001.Request
{
    class APInvoiceListRequest
    {
        public string InvoiceId { get; set; }//Número de factura que liquida con el cheque
        public string Voucher { get; set; }//Número de asiento de factura que liquida con el cheque
        public decimal AmountInvoice { get; set; }

    }
}
