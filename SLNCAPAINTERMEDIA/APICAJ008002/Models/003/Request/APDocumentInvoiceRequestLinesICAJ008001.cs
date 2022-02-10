using System;
using System.Collections.Generic;
using System.Text;

namespace APICAJ008002.Models._003.Request
{
    class APDocumentInvoiceRequestLinesICAJ008001
    {
        public string Voucher { get; set; }//Id del asiento de la factura
        public string InvoiceId { get; set; }//Numero de la factura
        public int SecuenciaFacturacion { get; set; }
        public List<ItemListResponse> ItemList { get; set; }//Código del producto

        public DateTime InvoiceDate { get; set; }//Fecha de emisión de la factura, Date
        public decimal TotalAmount { get; set; }//Total de cada factura

    }
}
