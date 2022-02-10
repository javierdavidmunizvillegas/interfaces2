using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ019.api.Models._001.Response
{
    public class APEntityCreditNoteTable
    {
        public string CustAccount { get; set; }//Código de cliente
        public string CreditNote { get; set; }//Número de nota de crédito
        public Decimal Amount { get; set; }//Monto de la nota de crédito
        public DateTime Date { get; set; }//Fecha de la nota de crédito
        public string DocumentOrigin { get; set; }//Documento de origen
        public string VoucherCreditNote { get; set; }//Asiento de la nota de crédito
        public string MotiveCreditNote { get; set; }//Motivo de la nota de crédito
        public string TypeOfDocument { get; set; }//Tipo de documento
       public List<APEntityCreditNoteLine> APEntityCreditNoteLineList { get; set; }//Listado de relación de notas de créditos de servicios

    }
}
