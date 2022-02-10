using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Response
{
    public class APEntitySalesOrderInvoiceLine
    {
        public string SalesId { get; set; }//Número de orden de venta
        public string Invoice { get; set; }//Número de Factura o documento 
        public string InvoiceVoucher { get; set; }//Voucher de la factura
        public DateTime InvoiceDate { get; set; }//Fecha de la factura
        public decimal InvoiceAmount { get; set; }//Monto de la factura
        public List<APEntityInvoiceReceivableLine> APEntityInvoiceReceivableLineList { get; set; }//Listado de relación de ordenes de ventas y facturas / Cobros

    }
}
