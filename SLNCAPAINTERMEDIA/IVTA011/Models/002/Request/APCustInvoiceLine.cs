using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA011.Models._002.Request
{
    class APCustInvoiceLine
    {
        // 002 Ventas MULTINOVA (Contado/Crédito y las Devoluciones) 
        public string ItemId { get; set; }//Articulo
        public decimal Qty { get; set; }//Cantidad
        public string ItemName { get; set; }//Descripcion
        public decimal Amount { get; set; }//Monto
        public string InvoiceId { get; set; }//Factura
        public List<APFinancialDimension> APFinancialDimensionList { get; set; }//Listado de dimensiones financieras

    }
}
