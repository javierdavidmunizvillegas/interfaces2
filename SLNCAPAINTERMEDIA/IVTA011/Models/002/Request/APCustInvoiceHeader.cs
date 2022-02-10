using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA011.Models._002.Request
{
    class APCustInvoiceHeader
    {
        // 002 Ventas MULTINOVA (Contado/Crédito y las Devoluciones) 
        public string APStoreId { get; set; }//Tienda
        public string APDocumentType { get; set; }//Tipo de documento
        public string InvoiceId { get; set; }//Factura
        public string InvoiceDate { get; set; }//Fecha del documento Date
        public string Nombre { get; set; }//Nombre
        public string VATNum { get; set; }//Ruc
        public string APCodeIndependentVend { get; set; }//cedula
        public string VATNumEI { get; set; }//Ruc
        public decimal SubTotalAmount { get; set; }//Monto del subtotal de ventas
        public decimal SubTotalAmountCash { get; set; }//Subtotal de contado
        public decimal SubTotalAmountCredit { get; set; }//Subtotal de credito
        public List<APCustInvoiceLine> APCustInvoiceLineList { get; set; }//Listado de las lineas de facturas

    }
}
