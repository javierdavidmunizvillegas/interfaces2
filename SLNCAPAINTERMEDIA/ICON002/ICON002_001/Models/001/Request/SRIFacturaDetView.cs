using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_001.Models._001.Request
{
    public class SRIFacturaDetView
    {
        public Guid FacturaId { get; set; }
        public int DetSecuencia { get; set; }
        public string CodigoPrincipal { get; set; }
        public string CodigoAuxiliar { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Descuento { get; set; }
        public decimal PrecioTotalSinImpuesto { get; set; }
        public string DaNombre1 { get; set; }
        public string DaValor1 { get; set; }
        public string DaNombre2 { get; set; }
        public string DaValor2 { get; set; }
        public string DaNombre3 { get; set; }
        public string DaValor3 { get; set; }
        public string DaExtraNombre { get; set; }
        public string DaExtraValor { get; set; }
        public int Estado { get; set; }

    }
}
