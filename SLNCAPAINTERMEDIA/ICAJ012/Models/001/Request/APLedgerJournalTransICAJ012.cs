using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ012.api.Models._001.Request
{
    public class APLedgerJournalTransICAJ012
    {
        [Required]
        public DateTime TransDate { get; set; }
        [Required]
        public string Txt { get; set; }
        [Required]
        public int AccountType { get; set; } // LedgerJournalACType
       
        public string Account { get; set; }
        [Required]
        public int OffsetAccountType { get; set; } // LedgerJournalACType
        [Required]
        public string OffSetAccount { get; set; }
        [Required]
        public decimal Debit { get; set; }
        [Required]
        public decimal Credit { get; set; }
        public string DocumentNum { get; set; }
        public string ApStoreId { get; set; }
        public string APUserGeneMovi { get; set; }
        public string ApTransactionType { get; set; }
        public string ApBoxCode { get; set; }
        [Required]
        public List<APFinancialDimension> APFinancialDimensionList { get; set; }

        ///RESPONSE
        public string Voucher { get; set; }
       // public LedgerJournalACType AccountType { get; set; }
       // public string Account { get; set; }
       // public LedgerJournalACType OffsetAccountType { get; set; }
       // public string OffSetAccount { get; set; }
       // public decimal Debit { get; set; }
       // public decimal Credit { get; set; }
        //public string DocumentNum { get; set; }
    }
   /* public enum LedgerJournalACType
    {
        Ledger=0,
        Cust=1,
        Vend=2,
        Project=3,
        Asset=5,
        Bank=6
    }
    */
}
