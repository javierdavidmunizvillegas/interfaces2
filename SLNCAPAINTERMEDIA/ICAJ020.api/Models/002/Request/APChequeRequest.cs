using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ020.api.Models._002.Request
{
    public class APChequeRequest
    {
        public string BankId { get; set; } //Banco del cheque
        public string NumberAccountCheck { get; set; } // Número de cuenta del cheque
        public string NumberCheck { get; set; } // Número del cheque
        public string DueDateCheque { get; set; } // fecha de vencimiento DateTime
        public decimal AmountCheck { get; set; } // valor del cheque
        public string CustAccount { get; set; } // Código de cliente
        public string Description { get; set; } // Descripción 
        public string PostingProfile { get; set; } // Perfil de asiento contable 
        public string  DateReverse { get; set; } // fecha del reverso  DateTime
        public string CodeStore { get; set; } // código de la tienda
        public string CodeCash { get; set; } // código de la caja 
        public string User { get; set; } // usuario que generó el reverso 
        public string TypeTransaction { get; set; } // tipo de transacción 



    }
}
