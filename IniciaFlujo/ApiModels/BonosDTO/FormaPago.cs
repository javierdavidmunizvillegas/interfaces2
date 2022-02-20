using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.BonosDTO
{
    public class FormaPago
    {
        public Boolean Credito { get; set; }

        public Boolean Efectivo { get; set; }

        public Boolean Tarjeta { get; set; }
    }
}
