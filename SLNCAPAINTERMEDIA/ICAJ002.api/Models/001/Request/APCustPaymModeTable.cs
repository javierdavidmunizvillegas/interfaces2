using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ002.api.Models._001.Request
{
    public class APCustPaymModeTable
    {
       // public string id { get; set; } // Forma de Pago
        public string PaymMode { get; set; } // Forma de Pago
        public string NamePaymMode { get; set; } //Nombre
        public List<APCustPaymModeSpec> CustPaymModeSpec { get; set; } //Listado de Especificación del pago
    }
}
