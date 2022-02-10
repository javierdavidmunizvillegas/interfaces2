using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB003.api.Models._003.Response
{
    public class APICOB003003MessageResponse
    {
        public string APIdentificationList { get; set; }//Identificador de lista
        public string LedgerJournalTransId { get; set; }//ID asiento
        public string CustAccount { get; set; }
        public string TransDate { get; set; }
        public decimal Amount { get; set; }
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }//Listado de errores
    }
}
