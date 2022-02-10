using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC001.api.Models._001.Response
{
    public class APISAC001001MessageResponseLegado
    {
        public string SessionId { get; set; }//Id de sesión Guid
        public string DescriptionId { get; set; }//Descripción trnsaccion
        public List<string> ErrorList { get; set; }//Detalle del error

    }
}
