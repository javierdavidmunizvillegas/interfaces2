using System;
using System.Collections.Generic;
using System.Text;

namespace ICAJ018.api.Models._001.Response
{
    public class APDocumentDeposit
    {
        public DateTime TransDate { get; set; }
        public string AccountNum { get; set; }
        public decimal AmountDebit { get; set; }
        public string PaymMode { get; set; }
        public string PaymentReference { get; set; }
        public string UserName { get; set; }
        public string Voucher { get; set; }
        public string DescriptionConfirmation { get; set; }
        public string CodeMotive { get; set; }
        public string OffsetAccountNum { get; set; }
        public decimal AmountCredit { get; set; }
        public string DocumentNum { get; set; }
        public string CodeStore { get; set; }
        public string UserNameReverse { get; set; }
        public string TypeTransaction { get; set; }
        public string CodeCash { get; set; }
    }
   

}
