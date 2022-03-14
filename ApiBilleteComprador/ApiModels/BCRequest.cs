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

    public class BCCamposActualizar
    {
        public string NumeroFacturAplicado { get; set; }
        public decimal NumeroPedidoAplicado { get; set; }
        public decimal ValorNC { get; set; }
        public string NumeroNC { get; set; }
        public int  Recibo { get; set; }
        public decimal NumeroPedidoOrigen { get; set; }
    }
}
