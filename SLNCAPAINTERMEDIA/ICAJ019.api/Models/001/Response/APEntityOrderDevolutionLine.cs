using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Response
{
    public class APEntityOrderDevolutionLine
    {
        public string SalesId { get; set; }// Número de orden devuelta
        public string CreditNote { get; set; }//Número de nota de crédito
        public string VoucherCreditNote { get; set; }// Asiento de nota de crédito
        public DateTime DateCreditNote { get; set; }//Fecha de la nota de crédito
        public string ReasonCreditNote { get; set; }//Motivo de la nota de crédito
        public Decimal AmountCreditNote { get; set; }//Monto de la nota de crédito
        public string TypeOfDocument { get; set; }// Tipo de documento
        public List<APEntityReceivableDevolutionLine> APEntityReceivableDevolutionLineList { get; set; }//Listado de relación de ordenes de ordenes de devolución, pedido de devolución y NC / COBRO



    }
}
