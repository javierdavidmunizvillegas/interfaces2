using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.ComisionesDTO
{
    public class RequestComisiones
    {
        [JsonProperty("dataAreaId")]
        public string DataAreaId { get; set; }

        [JsonProperty("enviroment")]
        public string Entorno { get; set; }

        [JsonProperty("sessionId")]
        public string Sesion { get; set; }

        [JsonProperty("documentInvoiceList")]
        public List<DocumentInvoice> DocumenInvoiceList { get; set; }
    }
}
