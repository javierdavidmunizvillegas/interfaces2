using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class ICAJ008Response:Base
    {
        public string sessionId { get; set; }
        public List<string> errorList { get; set; }
        public bool statusId { get; set; }

        public List<APDocumentInvoiceRequestTableICAJ008001> documentInvoiceRequestTableList { get; set; }
        public ICAJ008Response()
        {
            documentInvoiceRequestTableList = new List<APDocumentInvoiceRequestTableICAJ008001>();
        }
    }
    public partial class APDocumentInvoiceRequestTableICAJ008001
    {
        public string CustAccount { get; set; }
        public string SalesId { get; set; }
        public string SalesIdAccount { get; set; }
        public string Store { get; set; }
        public string PostingProfile { get; set; }
        public decimal TotalAmount { get; set; }
        public List<APDocumentInvoiceRequestLinesICAJ008001> documentInvoiceRequestLinesList { get; set; }
        public List<APDocumentInvoiceRequestProvisionNCList> documentInvoiceRequestProvisionNCList { get; set; }

        public APDocumentInvoiceRequestTableICAJ008001()
        {
            documentInvoiceRequestLinesList = new List<APDocumentInvoiceRequestLinesICAJ008001>();
            documentInvoiceRequestProvisionNCList = new List<APDocumentInvoiceRequestProvisionNCList>();
        }
    }
    public partial class APDocumentInvoiceRequestLinesICAJ008001
    {
        public string Voucher { get; set; }
        public string InvoiceId { get; set; }
        public int secuenciaFacturacion { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<ItemLists> itemList { get; set; }

        public APDocumentInvoiceRequestLinesICAJ008001()
        {
            itemList = new List<ItemLists>();
        }
    }

    public partial class APDocumentInvoiceRequestProvisionNCList
    {
        public string InvoiceId { get; set; }
        public string VoucherNC { get; set; }
        public string VoucherProvision { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal AmountNC { get; set; }
    }

    public partial class ItemLists
    {
        public string itemId { get; set; }
        public int qty { get; set; }
        public decimal amountLine { get; set; }
        public int ordenItems { get; set; }
    }
}
