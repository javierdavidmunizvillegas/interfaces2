using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_001.Models._001.Request
{
    public class SriGuiaDestinatarioView
    {
        public Guid FacturaId { get; set; }
        public int DesSecuencia { get; set; }
        public string IdentificacionDestinatario { get; set; }
        public string RazonSocialDestinatario { get; set; }
        public string DirDestinatario  { get; set; }
        public string MotivoTraslado { get; set; }
        public string CodDocSustento { get; set; }
        public string NumDocSustento { get; set; }
        public string NumAutDocSustento { get; set; }
        public DateTime FechaEmisionDocSustento { get; set; }
        public string CodEstabDestino { get; set; }
        public string DocAduaneroUnico { get; set; }
        public string Ruta { get; set; }
        public int Estado { get; set; }
        

    }
}
