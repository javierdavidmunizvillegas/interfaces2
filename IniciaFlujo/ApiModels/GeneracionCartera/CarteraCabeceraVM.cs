using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.CarteraDTO
{
    public class CarteraCabeceraVM
    {

        [JsonProperty("NumeroPedido")]
        public string NumeroPedido { get; set; }

        [JsonProperty("CodigoAlmacen")]
        public string CodigoAlmacen { get; set; }

        [JsonProperty("NumeroOrdenVentaSmart")]
        public int OrdenVentaSmart { get; set; }

        [JsonProperty("NumeroOrdenVentaDynamic")]
        public int OrdenVentaDynamic { get; set; }

        [JsonProperty("OrigenFactura")]
        public string OrigenFactura { get; set; }

        [JsonProperty("CuentaCliente")]
        public string CuentaCliente { get; set; }

        [JsonProperty("FechaDocumento")]
        public DateTime FechaDocumento { get; set; }

        [JsonProperty("PerfilContabilizacion")]
        public string PerfilContabilizacion { get; set; }

        [JsonProperty("TotalPedido")]
        public decimal TotalPedido { get; set; }

        [JsonProperty("Estado")]
        public string Estado { get; set; }

    }
}
