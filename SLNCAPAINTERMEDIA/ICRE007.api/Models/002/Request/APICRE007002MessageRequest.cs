using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE007.api.Models._002.Request
{
    public class APICRE007002MessageRequest
    {

        // 002 consulta de perfiles de asiento contable de Clientes
        [Required]
        public string DataAreaId { get; set; }//Id de la compañía 
        public string prioridad { get; set; }//Prioridad 

        public string SessionId { get; set; }//Id de sesión //Guid
        public string Enviroment { get; set; } //Id del ambiente
    }
   // public enum Prioridad { Bajo = 0, Medio = 1, Alto = 2 }
}

