using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICXP003.api.Models._001.Request
{
    public class APBankAccount
    {
        
        public string BankAccountName { get; set; }        
        public string BankRoutingNumber { get; set; }        
        public string APFormaPagoBancoId { get; set; }
        public string BankAccountNumber { get; set; }
        public string APBankAccountType { get; set; }
    }
}
