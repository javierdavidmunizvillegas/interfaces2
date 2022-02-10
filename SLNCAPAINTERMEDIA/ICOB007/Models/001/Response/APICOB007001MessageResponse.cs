using System;
using System.Collections.Generic;
using System.Text;

namespace ICOB007.Models._001.Response
{
   public class APICOB007001MessageResponse
    {
        public string SessionId { get; set; }//Id de sesión
        public Boolean StatusId { get; set; }//Descripción "True" o "False"
        public List<string> ErrorList { get; set; }//Listado de errores
        public string NumberOT { get; set; }//Número de OT
        public string Dacion { get; set; }//Código de dación
        public Int64 RecIdLine { get; set; }// RecId  línea de la dación


    }
}
