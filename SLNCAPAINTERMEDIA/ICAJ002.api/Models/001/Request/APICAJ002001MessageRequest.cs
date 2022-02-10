using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ002.api.Models._001.Request
{
    public class APICAJ002001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; } // Id de la compañía
        public string SessionId { get; set; } //Id de Sesion
        public string Enviroment { get; set; } //Id del ambiente

    }
}
