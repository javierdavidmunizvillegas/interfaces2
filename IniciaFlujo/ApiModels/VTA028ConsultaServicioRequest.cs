using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class VTA028ConsultaServicioRequest
    {
        public int NumeroPedidoSiac { get; set; }
    }

    public class VTA028ConsultaServicioResponse : Base
    {
        public string Codigo { get; set; }
        public string Mensaje { get; set; }
        public VTA028ConsultaDataCuotas CuotaDto { get; set; }
    }

    public class VTA028ConsultaDataCuotas
    {
        public int Id { get; set; }
        public int NumeroCuota { get; set; }
        public string InicioPeriodo { get; set; }
        public string FinPeriodo { get; set; }
        public decimal Prima { get; set; }
        public bool Cobrado { get; set; }
    }
}
