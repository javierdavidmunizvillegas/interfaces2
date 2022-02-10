using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ026.api.Models._001.Response
{
    public class APICAJ026001MessageResponse
    {
        public List<APStoreContract> APStoreList { get; set; }//Listado de tiendas
        public Boolean StatusId { get; set; }//Estado del registro 1=OK; 0=ERROR
        public List<string> ErrorList { get; set; }//Detalle del error
        public string SessionId { get; set; }//Id de sesión

    }
}
