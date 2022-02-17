using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class AsistenciaResponse : Base
    {
        public string codigoTransaccion { get; set; }
        public string estadoTransaccion { get; set; }
        public string descripcionTransaccion { get; set; }
    }
}
