using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_001.Models._001.Request
{
    public class SriFacturaReembolsoImpView
    {
        public Guid FacturaId { get; set; }
        public int ReSecuencia { get; set; }
        public int ImpSecuencia { get; set; }
        public string Codigo { get; set; }
        public string CodigoPorcentaje { get; set; }
        public int Tarifa { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal Impuesto { get; set; }
    }
}
