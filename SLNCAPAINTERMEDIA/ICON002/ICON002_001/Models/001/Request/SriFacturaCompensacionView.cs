using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_001.Models._001.Request
{
    public class SriFacturaCompensacionView
    {
        public Guid FacturaId { get; set; }
        public int Secuencial { get; set; }
        public int Codigo { get; set; }
        public int Tarifa { get; set; }
        public decimal Valor { get; set; }


    }
}
