using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.CarteraDTO
{
    public class RequestCartera
    {
        [JsonProperty("StatusId")]
        public Boolean StatusCode { get; set; }

        [JsonProperty("ErrorList")]
        public List<String> ErrorList { get; set; }

        [JsonProperty("DocumentInvoiceRequestTableList")]
        public List<DocumentInvoice> ListaDocumentos { get; set; }

        [JsonProperty("SessionId")]
        public string Sesion { get; set; }
    }
}
