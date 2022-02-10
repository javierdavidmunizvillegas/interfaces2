using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC001.api.Models._001.Response
{
    public class APISAC001001MessageResponse
    {
        public string SessionId { get; set; }//Id de sesión Guid
        public bool StatusId { get; set; }//Descripción trnsaccion
        public List<string> ErrorList { get; set; }//Detalle del error

    }
}
