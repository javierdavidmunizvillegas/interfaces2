using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.CarteraDTO
{
    public class DocumentInvoice
    {

        [JsonProperty("CustAccount")]
        public string CusAccount { get; set; }

        [JsonProperty("SalesId")]
        public string SalesId { get; set; }

        [JsonProperty("SalesIdAccount")]
        public string SalesIdAccount { get; set; }

        [JsonProperty("Store")]
        public string Store { get; set; }

        [JsonProperty("PostingProfile")]
        public string PostingProfile { get; set; }

        [JsonProperty("SalesOriginId")]
        public string SalesOriginId { get; set; }

        [JsonProperty("TotalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonProperty("User")]
        public string User { get; set; }

        [JsonProperty("Terminal")]
        public string Terminal { get; set; }

        [JsonProperty("DocumentInvoiceRequestLinesList")]
        public List<DocumentInvoiceLine> Lista { get; set; }

        //[JsonProperty("DocumentInvoiceRequestProvisionNCList")]
        //public List<ProvisionNC> ProvisionNOcList { get; set; }

    }
}
