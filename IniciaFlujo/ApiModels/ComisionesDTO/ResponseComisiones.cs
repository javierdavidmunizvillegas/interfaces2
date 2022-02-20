using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.ComisionesDTO
{
    public class ResponseComisiones
    {

        [JsonProperty("statusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("response")]
        public string Response { get; set; }

        [JsonProperty("descripcionId")]
        public string DescripcionId { get; set; }

        [JsonProperty("errorList")]
        public List<string> ListaErrores { get; set; }
    }
}
