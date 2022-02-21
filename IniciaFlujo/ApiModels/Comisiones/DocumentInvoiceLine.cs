using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.ComisionesDTO
{
    public class DocumentInvoiceLine
    {
        [JsonProperty("itemId")]
        public string ItemId { get; set; }

        [JsonProperty("serial")]
        public string Serial { get; set; }

        [JsonProperty("qty")]
        public int Cantidad { get; set; }

        [JsonProperty("postingProfile")]
        public string PostingProfile { get; set; }

    }
}
