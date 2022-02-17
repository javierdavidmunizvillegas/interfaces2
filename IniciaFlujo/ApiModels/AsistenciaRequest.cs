using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class AsistenciaRequest
    {
        public string codigoProductoSeguro { get; set; }
        public string numeroFactura { get; set; }
        public string numeroCertificado { get; set; }
        public string codigoSucursal { get; set; }
        public decimal prima { get; set; }
        public int numeroOperacion { get; set; }
        public DateTime fechaPago { get; set; }
        public int numeroCuota { get; set; }
        
    }
}
