using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class BCResult : Base
    {
        public string Id { get; set; }
        public string NumeroBillete { get; set; }
        public string Cedula { get; set; }
        public string FechaVencimiento { get; set; }
        public string Usado { get; set; }
    }
}
