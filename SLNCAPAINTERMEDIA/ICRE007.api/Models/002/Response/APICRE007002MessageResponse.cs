using ICRE007.api.Models._002.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE007.api.Models._002.Response
{
    public class APICRE007002MessageResponse
    {
        public List<string> ErrorList { get; set; }//Listado de grupo Cliente List<APIPRO007001Error>
        public string SessionId { get; set; }//Id de sesión guid
        public List<APPostingProfile> PostingProfileList { get; set; }//Listado de grupo Cliente
        public bool StatusId { get; set; }//descripcion ok/error   Descripcion

    }
}
