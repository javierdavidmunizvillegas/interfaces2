using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class BCRequest
    {
        public string NumeroPedido { get; set; }
    }
    public class BCRegistro
    {
        public string Cedula { get; set; }
        public string Valor { get; set; }
        public string NumeroPedido { get; set; }
        public string IdProvisional { get; set; }

    }
}
