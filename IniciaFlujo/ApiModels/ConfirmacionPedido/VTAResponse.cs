using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class VTAResponse:Base
    {
        public string CodigoMensaje { get; set; }
        public string Respuesta { get; set; }
        public List<string> MensajeError { get; set; }

        public VTAResponse()
        {
            MensajeError = new List<string>();
        }
    }
}
