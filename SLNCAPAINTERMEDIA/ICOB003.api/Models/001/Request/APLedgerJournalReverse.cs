using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB003.api.Models._001.Request
{
    public class APLedgerJournalReverse
    {
        public string VoucherSettlement { get; set; }//Id del asiento original
        public string CustAccount { get; set; }//Código del Cliente
        public string DateTrans { get; set; }//Fecha de Liquidación date
        public string ReasonCode { get; set; }//Código de Motivo
        public decimal Amount { get; set; }//Monto

    }
}
