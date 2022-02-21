using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.CarteraDTO
{
    public class ProvisionNC
    {
        [JsonProperty("AmountNC")]
        public decimal AmountNC { get; set; }

        [JsonProperty("InvoiceDate")]
        public DateTime InvoiceDate { get; set; }

        [JsonProperty("InvoiceId")]
        public string InvoiceId { get; set; }

        [JsonProperty("VoucherNC")]
        public string VoucherNC { get; set; }

        [JsonProperty("VoucherProvision")]
        public string VoucherProvision { get; set; }

    }
}
