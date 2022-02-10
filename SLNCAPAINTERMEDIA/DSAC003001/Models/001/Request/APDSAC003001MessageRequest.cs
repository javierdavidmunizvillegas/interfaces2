using System;
using System.Collections.Generic;
using System.Text;

namespace DSAC003001.Models._001.Request
{
    class APDSAC003001MessageRequest
    {
        public string DataAreaId { get; set; }//Id de la compañía 
        public string Enviroment { get; set; }//Entorno

        public string SessionId { get; set; }//id de la sesion
        public DateTime ObjectValidFrom { get; set; }//Fecha de consulta
    }
}
