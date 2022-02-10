using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO007.api.Models._001.Request
{
    public class APIPRO007001MessageRequestLegado
    {
        // 001 actualizar , insertar pronostico de la demanda
        public string DataAreaId { get; set; }//Id de la compañía 

        public string SessionId { get; set; }//id de la sesion

        public List<APForecastSales> APForecastSalesList { get; set; } //Listado del pronostico de la demanda
        public string Enviroment { get; set; } //Id del ambiente

    }
}
