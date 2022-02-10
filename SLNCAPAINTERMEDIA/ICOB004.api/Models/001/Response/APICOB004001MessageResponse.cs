using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB004.api.Models._001.Response
{
    public class APICOB004001MessageResponse
    {
        public string SessionId { get; set; }//Id de sesión
        public Boolean StatusId { get; set; }//Descripción "True" o "False"
        public List<string> ErrorList { get; set; }//Listado de errores
        public string APTypeTransaction { get; set; }//Id de sesión

        public List<APCreditNoteResponse> CreditNoteList { get; set; }//Lista del detalle de las transacciones


    }
}
