using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ003.api.Models._001.Request
{
    public class APLedgerJournalLine
    {
        public string Voucher { get; set; }        
        public DateTime TransDate { get; set; }        
        public string CustAccount { get; set; }
        public string[] VoucherList { get; set; }
        public string APIdentificationList { get; set; }
        public string DocumentNum { get; set; }        
        public decimal AmountCurCredit { get; set; }        
        public string PaymMode { get; set; }        
        public string PostingProfile { get; set; }        
        public string APStoreId { get; set; }
        public string APBoxCode { get; set; }        
        public string APUserPaym { get; set; }        
        public string APTransactionType { get; set; }
        public string SalesId { get; set; }
        public string APNumberOT { get; set; }
        public DateTime DocumentDate { get; set; }
        public string PaymentReference { get; set; }
        public string PaymSpec { get; set; }
        public APVendTransRegistrationFine APVendTransRegistrationFine { get; set; }
        public APLedgerJournalLineSalesOrder APLedgerJournalLineSalesOrder { get; set; }
        public APPaymModeElectronic APPaymModeElectronic { get; set; }
        public APPaymModeCheck APPaymModeCheck { get; set; }
        public APRetentionComprobante APRetentionComprobante { get; set; }
        public string OperationId { get; set; }

    }
}
