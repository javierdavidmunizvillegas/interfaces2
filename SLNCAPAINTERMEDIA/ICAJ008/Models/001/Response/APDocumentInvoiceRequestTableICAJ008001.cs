using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ008.api.Models._001.Response
{
    public class APDocumentInvoiceRequestTableICAJ008001
    {
        public string CustAccount { get; set; }//Cuenta del cliente
        public string SalesId { get; set; }//Orden de venta
        public string SalesIdAccount { get; set; }//Orden de venta de cliente
        public string Store { get; set; }//Código de tienda
        public string PostingProfile { get; set; }//Perfil de contabilización
        public decimal TotalAmount { get; set; }//Total del Pedido
        public List<APDocumentInvoiceRequestLinesICAJ008001> DocumentInvoiceRequestLinesList { get; set; }//Código de tienda
        public List<APDocumentInvoiceRequestProvisionNCList> DocumentInvoiceRequestProvisionNCList { get; set; }//Listado de lineas de billete comprador


    }
}
