using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Response
{
    public class APEntityOrderDevolutionTable
    {
        public string CustAccount { get; set; }//Código de cliente
        public string ADM { get; set; }//Orden de devolución
        public string ReturnReasonCode { get; set; }//Código de motivo de devolución
        public string SalesIdOrigin { get; set; }//Numero de orden de venta origen
        public string InvoiceIdOrigin { get; set; }//Numero de factura de origen
        public string SalesId { get; set; } //Número de orden devuelta
        public decimal Amount { get; set; } //Monto de la orden devuelta
        public string Status { get; set; } // Estado de la orden devuelta

        public List<APEntityOrderDevolutionLine> APEntityOrderDevolutionLineList { get; set; } //Listado de relación de ordenes de ordenes de devolución, pedido de devolución y NC

    }
}
