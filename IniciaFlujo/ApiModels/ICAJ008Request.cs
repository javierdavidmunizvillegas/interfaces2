using ApiModels.ICAJ008;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class ICAJ008Request
    {
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public List<APDocumentInvoiceTableICAJ008001> APDocumentInvoiceTableICAJ008001 { get; set; }

      
        public ICAJ008Request()
        {
            APDocumentInvoiceTableICAJ008001 = new List<APDocumentInvoiceTableICAJ008001>();
        }
    }
    
    public partial class APDocumentInvoiceTableICAJ008001
    {
        public string SalesId { get; set; }
        public string CustAccount { get; set; }
        public string SalesOrigin { get; set; }
        public string InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string NumberSecuence { get; set; }
        public DateTime DocumentDate { get; set; }
        public string PostingProfile { get; set; }
        public List<APDocumentInvoiceLinesICAJ008001> DocumentInvoiceLinesList { get; set; }

        public APDocumentInvoiceTableICAJ008001()
        {
            DocumentInvoiceLinesList = new List<APDocumentInvoiceLinesICAJ008001>();
        }
    }

    public partial class APDocumentInvoiceLinesICAJ008001
    {
        public string ItemId { get; set; }
        public string Serial { get; set; }
        public int Qty { get; set; }

    }

}
