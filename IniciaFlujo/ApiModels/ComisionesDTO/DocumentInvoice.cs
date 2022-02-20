using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.ComisionesDTO
{
    public class DocumentInvoice
    {
        [JsonProperty("salesId")]
        public string SalesId { get; set; }

        [JsonProperty("custAccount")]
        public string CustAccount { get; set; }

        [JsonProperty("salesOrigin")]
        public string SalesOrigin { get; set; }

        [JsonProperty("InvoiceId")]
        public string InvoiceId { get; set; }

        [JsonProperty("InvoiceDate")]
        public DateTime InvoiceDate { get; set; }

        [JsonProperty("numberSecuence")]
        public string NumberSecuence { get; set; }

        [JsonProperty("documentDate")]
        public DateTime DocumentDate { get; set; }

        [JsonProperty("documentInvoiceLinesList")]
        public List<DocumentInvoiceLine> ListaDocumentInvoiceLine { get; set; }

    }
}
