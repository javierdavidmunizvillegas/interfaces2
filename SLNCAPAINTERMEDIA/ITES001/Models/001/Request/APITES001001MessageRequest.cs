using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ITES001.Models._001.Request
{
    class APITES001001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }//Id de la compañía 
        [Required]
        public List<APVendTransRegistration> APVendTransRegistrationList { get; set; }//Listado de registros
        public string SessionId { get; set; } //Id de Sesion

        public string Enviroment { get; set; } //Id del ambiente
    }
}
