using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.CarteraDTO
{
    /// <summary>
    /// ALvaro Guachisaca: response cartera 
    /// </summary>
    /// 
    public class ResponseCartera:Base
    {
        [JsonProperty("StatusCode")]
        public string StatusCode { get; set; }

        [JsonProperty("MessageError")]
        public List<String> ErrorList { get; set; }

        [JsonProperty("OperationList")]
        public List<Operacion> OperationList { get; set; }

    }
}
