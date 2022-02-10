using ITES001.Models._001.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ITES001.Models._001.Response
{
    class APITES001001MessageResponse
    {
        public string DataAreaId { get; set; }//Id de la compañía 
        public string SessionId { get; set; }//Id de sesión Guid
        public List<APVendTransRegistrationResponse> APVendTransRegistrationList { get; set; }//Listado de registros
        public bool StatusId { get; set; }//descripcion ok/error   Descripcion
        public List<string> ErrorList { get; set; }//Listado de grupo Cliente List<APIPRO007001Error>

    }
}
