using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICXP003.api.Models._002.Request
{
    public class APLedgerInvoiceContract
    {
      
        public DateTime TransDate { get; set; }      
        public LedgerJournalACType AccountType { get; set; }      
        public string Account { get; set; }      
        public DateTime InvoiceDate { get; set; }
        public string DocumentCode { get; set; }
        public string Series { get; set; }
        public string Invoice { get; set; }      
        public EnumNoYes ElectronicInvoice { get; set; }      
        public DateTime ExpirationDate { get; set; }      
        public int From { get; set; }      
        public int To { get; set; }      
        public string AuthorizationNumber { get; set; }
        public string AuthorizationNumberEInvoice { get; set; }      
        public decimal Credit { get; set; }      
        public decimal Debit { get; set; }      
        public DateTime ExpirationDate1 { get; set; }      
        public string Period { get; set; }      
        public string BonusTypeId { get; set; }

    }
    public enum LedgerJournalACType { Ledger = 0, Cust = 1, Vend = 2, Project = 3, FixedAssets = 5, Bank = 6 }
    public enum EnumNoYes { N0 = 0, Si = 1}
}
