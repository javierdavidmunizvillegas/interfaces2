using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.CarteraDTO
{
    public class CarteraDetalleVM
    {


        [JsonProperty("NumeroPedido")]
        public int NumeroPedido { get; set; }

        [JsonProperty("Secuencia")]
        public int Secuencia { get; set; }

        [JsonProperty("IdAsientoFactura")]
        public string AsientoFactura { get; set; }

        [JsonProperty("NumeroFactura")]
        public string NumeroFactura { get; set; }

        [JsonProperty("CodigoArticulo")]
        public string CodigoArticulo { get; set; }

        [JsonProperty("Cantidad")]
        public int Cantidad { get; set; }

        [JsonProperty("FechaEmision")]
        public DateTime FechaEmision { get; set; }

        [JsonProperty("TotalLinea")]
        public decimal TotalLinea { get; set; }



    }
}
