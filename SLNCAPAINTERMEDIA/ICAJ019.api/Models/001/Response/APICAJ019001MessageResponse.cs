using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Response
{
    public class APICAJ019001MessageResponse
    {
        public string SessionId { get; set; }//Id de sesión
        public List<APEntityReceivableTable> APEntityReceivableTableList { get; set; }//Listado Relación de Cobros con Ordenes de Ventas y Reversos
        public List<APEntitySalesOrderInvoiceTable> APEntitySalesOrderInvoiceTableList { get; set; }//Listado de Relación de Orden de Venta y Facturas
        public List<APEntityOrderDevolutionTable> APEntityOrderDevolutionTableList { get; set; }//Listado de Relación de Ordenes de Devolución, Pedidos devueltos y Notas de Crédito
        public List<APEntityInvoiceServiceTable> APEntityInvoiceServiceTableList { get; set; }//Listado de Relación de facturas de servicios o notas de débitos de servicios
        public List<APEntityCreditNoteTable> APEntityCreditNoteTableList { get; set; }//Listado de Relación de notas de créditos de servicios
        public bool StatusId { get; set; }//Estado true = ok y False = Error boolean
        public List<string> ErrorList { get; set; }//Listado de errores


    }
}
