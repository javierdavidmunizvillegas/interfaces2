using System;
using System.Collections.Generic;
using System.Text;

namespace APICAJ008002.Models._003.Request
{
    class APDocumentInvoiceRequestProvisionNCList
    {
        public DateTime InvoiceDate { get; set; }//Fecha de emisión de la factura, Date
        public string InvoiceId { get; set; }//Numero de la factura
        public string VoucherNC { get; set; }//Código del producto
        public string VoucherProvision { get; set; }//Voucher de la Provision reversada
        public decimal AmountNC { get; set; }//Id del asiento de la factura

    }
}
