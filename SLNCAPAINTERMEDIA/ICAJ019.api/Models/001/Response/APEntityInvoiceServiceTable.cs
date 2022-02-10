using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Response
{
    public class APEntityInvoiceServiceTable
    {
        public string CustAccount { get; set; }//Código de cliente
        public string Invoice { get; set; }//Número de factura de servicio
        public decimal Amount { get; set; }//Monto de la factura de servicio
        public DateTime InvoiceDate { get; set; }//Fecha de la factura de servicio
        public string DocumentRelation { get; set; }//Documento relacionado
        public string InvoiceVoucher { get; set; }//Asiento de la factura
        public string TypeOfDocument { get; set; }//Tipo de documento
        public List<APEntityInvoiceServiceLine> APEntityInvoiceServiceLineList { get; set; }//Listado de relación de facturas de servicios o notas de débitos de servicios 
        


    }
}
