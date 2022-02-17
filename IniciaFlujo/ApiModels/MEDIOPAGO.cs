using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class MEDIOPAGO
    {
        public string FormaPago { get; set; }
        public decimal Valor { get; set; }
        public string AsientoContable { get; set; }
    }
}
