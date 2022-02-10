using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA010.Models._001.Response
{
    class APIVTA010001MessageResponse
    {
        public string SessionId { get; set; }//Id de sesión
        public bool StatusId { get; set; }//Estado true = ok y False = Error
        public List<string> ErrorList { get; set; }//Listado de errores
        public List<string> AprobList { get; set; }//Listado de errores

    }
}
