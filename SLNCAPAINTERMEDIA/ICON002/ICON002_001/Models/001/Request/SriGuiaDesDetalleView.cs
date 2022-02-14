using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_001.Models._001.Request
{
    public class SriGuiaDesDetalleView
    {
        public Guid FacturaId { get; set; }
        public int DesSecuencia { get; set; }
        public int DetSecuencia { get; set; }
        public string CodigoInterno { get; set; }
        public string Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public string CodigoAdicional { get; set; }
        public string DaNombre1 { get; set; }
        public string DaValor1 { get; set; }
        public string DaNombre2 { get; set; }
        public string DaValor2 { get; set; }
        public string DaNombre3 { get; set; }
        public string DaValor3 { get; set; }
        public int Estado { get; set; }


    }
}
