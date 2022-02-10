using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE012.api.Models._001.Response
{
    public class APJournalTrans
    {
        public string TransDate { get; set; } //DateTime
        public string Voucher { get; set; } //voucher
       // public string dataAreaId { get; set; }
        public string CustAccount { get; set; }
        public string CustName { get; set; }
        public string CustGroup { get; set; } // nuevo campo
        public string Txt { get; set; } //txt
        public List<string> InvoiceIdList { get; set; }
        public decimal AmountDebit { get; set; } //
        public decimal AmountCredit { get; set; } // nuevo
        public string PaymMode { get; set; }
        public string PostingProfile { get; set; }
        public string APStoreId { get; set; }
        public string APStoreName { get; set; }
        public string UserCreation { get; set; }
        public string TransactionType { get; set; }
        public string SalesId { get; set; }
        public string NumberOT { get; set; }
        public string CajaCode { get; set; }
        public string APIdentificationList { get; set; }
       




    }
}
