using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.CarteraDTO
{
    public class DocumentInvoiceLine
    {
        [JsonProperty("InvoiceDate")]
        public DateTime InvoiceDate { get; set; }

        [JsonProperty("InvoiceId")]
        public string InvoiceId { get; set; }

        [JsonProperty("SecuenciaFacturacion")]
        public int SecuenciaFacturacion { get; set; }

        [JsonProperty("ItemList")]
        public List<ItemList> ItemList { get; set; }

        [JsonProperty("TotalAmount")]
        public decimal TotalAmount { get; set; }

        [JsonProperty("Voucher")]
        public string Voucher { get; set; }

    }
}
