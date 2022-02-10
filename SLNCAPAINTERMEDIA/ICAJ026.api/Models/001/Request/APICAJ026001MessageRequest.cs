using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ026.api.Models._001.Request
{
    public class APICAJ026001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }//Id de la compañía 
       
        public string SessionId { get; set; }//Id de sesión GUID

        public string Enviroment { get; set; } //Id del ambiente
        public string StoreId { get; set; }//Número de la tienda

    }
}
