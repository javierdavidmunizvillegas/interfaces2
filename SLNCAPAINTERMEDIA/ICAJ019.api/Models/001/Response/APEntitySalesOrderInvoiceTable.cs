using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Response
{
    public class APEntitySalesOrderInvoiceTable
    {
        public string CustAccount { get; set; }//Código de cliente
        public string SalesId { get; set; }//Número de orden de venta
        public string OrderClient { get; set; }//Orden de cliente o número de pedido
        public string SalesStatus { get; set; }//Estado de la orden de venta
        public decimal AmountOrder { get; set; }//Monto de la orden de venta
        public List<APEntitySalesOrderInvoiceLine> APEntitySalesOrderInvoiceLineList { get; set; }//Listado de relación de ordenes de ventas y facturas
       


    }
}
