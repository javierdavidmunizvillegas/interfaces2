using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class VTA028NotificaServicioRequest
    {
        public int CuotaId { get; set; }
        public string NumeroFactura { get; set; }
        public string UsuarioGenerador { get; set; }
        public VTA028NotificaServicioAnticipos Anticipos { get; set; }

    }

    public class VTA028NotificaServicioAnticipos
    {
        public string IdAsiento { get; set; }
        public decimal Valor { get; set; }
    }


    public class VTA028NotificaServicioResponse:Base
    {
        public string Codigo { get; set; }
        public string Mensaje { get; set; }
        public VTA028ConsultaDataCuotas Dato { get; set; }
    }
}
