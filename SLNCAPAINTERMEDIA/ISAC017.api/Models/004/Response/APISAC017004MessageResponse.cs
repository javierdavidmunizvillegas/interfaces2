using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.api.Models._004.Response
{
    public class APISAC017004MessageResponse
    {
       
        public string SessionId { get; set; } // Id de sesión
      
        public Boolean StatusId { get; set; } // Estado true = ok y False = Error

        public List<string> ErrorList { get; set; } // Listado de errores

        public APCreditNoteHeader CreditNote { get; set; } // Cabecera nota de credito

      
    }
}
