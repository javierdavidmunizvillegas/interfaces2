using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_001.Models._001.Request
{
    public class SriFacturaDetImpuestosView
    {
        public Guid FacturaId { get; set; }
        public int DetSecuencia { get; set; }
        public int ImpuestoSecuencia { get; set; }
        public string Codigo { get; set; }
        public string CodigoPorcentaje { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal Tarifa { get; set; }
        public decimal Valor { get; set; }
        public string Tipdocsustento { get; set; }
        public string Numdocsustento { get; set; }
        public int Estado { get; set; }
    }
}
