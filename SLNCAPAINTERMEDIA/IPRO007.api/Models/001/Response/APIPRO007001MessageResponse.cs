using IPRO007.api.Models._001.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO007.api.Models._001.Response
{
    public class APIPRO007001MessageResponse
    {
        public List<Apforecastsalesheaderlist> APForecastSalesHeaderList { get; set; } //codigo de articulo
        public string SessionId { get; set; }//Id de sesión //Guid

        public bool StatusId  { get; set; }//descripcion ok/error   Descripcion

        public List<string> ErrorList { get; set; }//Listado de grupo Cliente List<APIPRO007001Error>
    }
}


