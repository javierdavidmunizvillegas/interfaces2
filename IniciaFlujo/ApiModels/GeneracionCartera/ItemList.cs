using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.CarteraDTO
{
    public class ItemList
    {
        [JsonProperty("AmountLine")]
        public decimal AmountLine { get; set; }

        [JsonProperty("ItemId")]
        public string ItemId { get; set; }

        [JsonProperty("OrdenItems")]
        public int OrdenItems { get; set; }

        [JsonProperty("Qty")]
        public int Qty { get; set; }
    }
}
