using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVTA011.Models._001.Request
{
    class APIVTA011001MessageRequest
    {
         // 001 Periodo vigente para multinova
        public string DataAreaId { get; set; }//Id de la compañía 
        public string Enviroment { get; set; }//Entorno

        public string SessionId { get; set; }//id de la sesion

        public string TransDate { get; set; } //Fecha ejecución Datetime
    }
}
