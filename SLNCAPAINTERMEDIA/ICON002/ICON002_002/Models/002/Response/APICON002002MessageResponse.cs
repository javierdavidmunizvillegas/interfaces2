using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_002.Models._002.Response
{
    public class APICON002002MessageResponse
    {
        public string Documento { get; set; }
        public string NumAutorizacion { get; set; }
        public DateTime FechaAutorizacion { get; set; }
        public string ClaveAcceso { get; set; }
        public int Estado { get; set; }
        public string Respuesta { get; set; }
        public string SessionId { get; set; }
        public bool StatusId { get; set; }
        public string[] ErrorList { get; set; }

    }
}
