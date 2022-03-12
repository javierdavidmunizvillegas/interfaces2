using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class BCPedidoResult : Base
    {
        public string NumeroPedido { get; set; }
        public int CantidadBC { get; set; }

        public List<DetalleBC> DetallesBC { get; set; }

        public BCPedidoResult()
        {
            DetallesBC = new List<DetalleBC>();
        }

    }
    public partial class DetalleBC
    {
        public string NumeroBillete { get; set; }
        public string Usado { get; set; }
        public string FechaVencimiento { get; set; }
        public string IdProvisional { get; set; }
        public decimal Monto { get; set; }
    }
}
