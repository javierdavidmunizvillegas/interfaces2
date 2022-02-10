using System;
using System.Collections.Generic;
using System.Text;

namespace ICOB006.Models._001.Request
{
    class APDocumentTaxRequest
    {
        public string InvoiceTax { get; set; }//Número de factura del documento fiscal
        public string VoucherTax { get; set; }//Número de asiento de factura del documento fiscal
        public decimal AmountTax { get; set; }//monto de la factura del documento fiscal
        public string DateTax { get; set; }//fecha de la factura del documento fiscal
        public string DescriptionTax { get; set; }//Descripción de la factura
        public string NumChequeTax { get; set; }//Número del cheque


    }
}
