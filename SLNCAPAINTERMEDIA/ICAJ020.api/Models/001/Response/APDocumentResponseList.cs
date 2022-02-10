using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ020.api.Models._001.Response
{
    public class APDocumentResponseList
    {
        public string Voucher { get; set; } // Número de asiento principal que se va a deshacer
        public string LastSettlementVoucher { get; set; } // Número de asiento de liquidación que se va a deshacer
        public decimal Amount { get; set; } // Monto
        public string TransDate { get; set; } //Fecha de ejecución DateTime
        public List<string> ErrorList { get; set; } // Listado de errores
        public Boolean StatusId { get; set; } //Descripción "True" o "False"



    }
}
