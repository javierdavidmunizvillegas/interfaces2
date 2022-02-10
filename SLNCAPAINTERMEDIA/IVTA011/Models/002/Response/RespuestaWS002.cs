using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA011.Models._002.Response
{
    class RespuestaWS002
    {
        public bool StatusId { get; set; }//Estado true = ok y False = Error
        public List<string> ErrorList { get; set; }//Listado de errores
    }
}
