using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_001.Models._001.Request
{
    public class SriFacturaCabTotImpuestosView
    {
        public Guid FacturaId { get; set; }
        public int ImpuestoSecuencia { get; set; }
        public string Codigo { get; set; }
        public string CodigoPorcentaje { get; set; }
        public decimal BaseImponible { get; set; }
        public decimal Tarifa { get; set; }
        public decimal DescuentoAdicional { get; set; }
        public decimal Valor { get; set; }
        public string RtCodigoRetencion { get; set; }
        public decimal RtPorcentajeRetener { get; set; }
        public decimal RtValorRetenido { get; set; }
        public string RtCodDocSustento { get; set; }
        public string RtNumDocSustento { get; set; }
        public DateTime RtFechaEmisionDocSustento { get; set; }
        public string Adicional { get; set; }
        public int Estado { get; set; }



    }
}
