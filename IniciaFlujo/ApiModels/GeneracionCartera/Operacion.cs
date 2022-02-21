using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.CarteraDTO
{
    public class Operacion
    {

        [JsonProperty("OperationId")]
        public string OperacionId { get; set; }

        [JsonProperty("InvoiceList")]
        public List<CarteraFactura> InvoiceList { get; set; }

    }

    public partial class CarteraFactura
    {

        [JsonProperty("InvoiceId")]
        public string InvoiceId { get; set; }

        [JsonProperty("FatherInvoice")]
        public string FatherInvoice { get; set; }
    
    }

}
