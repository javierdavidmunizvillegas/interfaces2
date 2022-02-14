using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_001.Models._001.Request
{
    public class SriFacturaReembolsoView
    {
        public Guid FacturaId { get; set; }
        public int ReSecuencia { get; set; }
        public string TipoIdentificacionProveedor { get; set; }
        public string IdentificacionProveedor { get; set; }
        public string CodPaisPagoProveedor { get; set; }
        public string TipoProveedor { get; set; }
        public string CodDoc { get; set; }
        public string EstabDoc { get; set; }
        public string PtoEmiDoc { get; set; }
        public string SecuencialDoc { get; set; }
        public DateTime FechaEmisionDoc { get; set; }
        public string NumeroautorizacionDoc { get; set; }


    }
}
