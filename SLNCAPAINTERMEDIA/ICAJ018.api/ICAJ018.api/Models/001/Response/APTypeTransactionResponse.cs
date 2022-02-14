using System;
using System.Collections.Generic;
using System.Text;

namespace ICAJ018.api.Models._001.Response
{
    public class APTypeTransactionResponse
    {
        public TypeTransactionDeposit TypeTransactionDeposit { get; set; }
        public APDocumentDeposit[] DocumentDepositList { get; set; }

    }
    public enum TypeTransactionDeposit { preparacion_deposito = 0, deposito_confirmado = 1, reverso_deposito = 2 }
}
    
