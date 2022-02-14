using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_001.Models._001.Request
{
    public class SriFacturaFormaPagoView
    {
        public Guid FacturaId { get; set; }
        public int FormaPagoSecuencia { get; set; }
        public string FormaPagoCodigo { get; set; }
        public decimal FormaPagoTotal { get; set; }
        public decimal FormaPagoUnidadPlazo { get; set; }
        public string FormaPagoUnidadTiempo { get; set; } //Estaba decimal como en el documento pero legado lo tiene como string


    }
}
