using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB004.api.Models._001.Response
{
    public class APCreditNoteResponse
    {
        public string APIdentificationList { get; set; }//Identificador de lista
        public string DocumentNum { get; set; }//Número de nota crédito
        public string DocumentApply { get; set; }
        public string CustAccount { get; set; }//Código del cliente
        public string DocumentDate { get; set; }//Fecha de documento datetime TransDate
      //  public string Voucher { get; set; }//Número de documento que aplica
        public decimal Amount { get; set; }//Monto de la nota de crédito
        public string Asiento { get; set; }//Asiento de la nota de crédito


    }
}
