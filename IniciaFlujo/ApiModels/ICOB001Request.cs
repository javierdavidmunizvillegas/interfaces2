using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class ICOB001Request
    {
        public string CustAccount { get; set; }
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public List<APSettlementTransactionHeader> APSettlementTransactionHeaderList { get; set; }

        public ICOB001Request()
        {
            APSettlementTransactionHeaderList = new List<APSettlementTransactionHeader>();
        }
    }

    public partial class APSettlementTransactionHeader
    {
        public string VoucherSettlement { get; set; }
        public decimal Amount { get; set; }
        public string InvoiceId { get; set; }
        public string IdReciboCobro { get; set; }
        public DateTime DateTrans { get; set; }
        public List<APDocumentICOB001001> APDocumentList { get; set; }
        public APSettlementTransactionHeader()
        {
            APDocumentList = new List<APDocumentICOB001001>();
        }
    }

    public partial class APDocumentICOB001001
    {
        public string Voucher { get; set; }
        public string InvoiceId { get; set; }
        public decimal Amount { get; set; }
    }
}

