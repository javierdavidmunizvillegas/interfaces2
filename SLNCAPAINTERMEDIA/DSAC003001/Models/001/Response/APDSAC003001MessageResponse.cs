using System;
using System.Collections.Generic;
using System.Text;

namespace DSAC003001.Models._001.Response
{
    class APDSAC003001MessageResponse
    {
        public string SessionId { get; set; }//id de la sesion
        public List<APTechnicalReport> APTechnicalReport { get; set; }//Lista de informes técnicos
    }
}
