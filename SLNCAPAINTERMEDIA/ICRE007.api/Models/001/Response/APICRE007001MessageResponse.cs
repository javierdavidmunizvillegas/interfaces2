using ICRE007.api.Models._001.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE007.api.Models._001.Response
{
    public class APICRE007001MessageResponse
    {
        public string SessionId { get; set; }//Id de sesión Guid
        public List<APCustGroup> CustGroupList { get; set; }//Listado de grupo Cliente
        public bool StatusId { get; set; }//descripcion ok/error   Descripcion
        public List<string> ErrorList { get; set; }//Listado de grupo Cliente List<APIPRO007001Error>

    }
}
