using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB004.api.Models._002.Response
{
    public class APDocumentLedgerResponse
    {
        public string APIdentificationList { get; set; }//Identificador de lista
        public string Voucher { get; set; }//Asiento del registro contable
        public string CustAccount { get; set; }//Código del cliente
        public string TransDate { get; set; }//Fecha de asiento datetime
        public decimal Amount { get; set; }//Monto del asiento


    }
}
