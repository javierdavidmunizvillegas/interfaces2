using System;
using System.Collections.Generic;
using System.Text;

namespace APICAJ008002.Models._003.Response
{
    class ResponseCarteraFactura
    {
        public string InvoiceId { get; set; }//nùmero factura
        public string FatherInvoice { get; set; } //numero factura Padre
    }
}
