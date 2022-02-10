using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA010.Models._001.Request
{
    public class APCustInvoiceJourIVTA010001
    {
        public string InvoiceId { get; set; }//Número de Factura
        public string APStoreId { get; set; }//Tienda
        public DateTime InvoiceDate { get; set; }//Fecha emisión factura Datetime
        public string CustAccount { get; set; }//nombre cliente
        public string VATNumCust { get; set; }//Cédula del cliente
        public string Email { get; set; }//Correo del cliente
        public string VATNumEI { get; set; }//Cedula emprendedor independiente EI
        public string CityId { get; set; }//Ciudad Principal
        public decimal AmountIVA { get; set; }//Monto Incluido IVA En caso de devoluciones debe ir negativo
        public string PaymType { get; set; }//Tipo de Pago
        public decimal AmountSinIVA { get; set; }//Monto sin IVA En caso de devoluciones debe ir negativo
        public List<APCustInvoiceTransIVTA010001> CustInvoiceTrans { get; set; }//Datos del detalle de factura/Nota de crédito



    }
}
