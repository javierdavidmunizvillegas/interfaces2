
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.api.Models._004.Request
{
    public class APISAC017004MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; } // Id de la compañía 
       
        public string SessionId { get; set; } // Id de sesión
        public string Enviroment { get; set; }
        [Required]
        public string NumberSequenceGroup { get; set; } // Conjunto de secuencias númericas
        [Required]
        public APSalesTableReturn004 ReturnTable { get; set; } // Objeto de orden de devolución
        [Required]
        public List<APReturnTableDisposition004> ReturnTableDispositionList { get; set; } //Lista de Linea de orden de devolución a registrar
        


    }
}
