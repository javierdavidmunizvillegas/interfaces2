using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB003.api.Models._002.Response
{
    public class APICOB003002MessageResponse
    {
       // public string APIdentificationList { get; set; }//Identificador de lista
        
        public List<APCustInvoiceTableResponse> APCustInvoiceTableResponseList { get; set; }
         
        public bool StatusId { get; set; }//Estado true = ok y False = Error Boolean 
        public List<string> ErrorList { get; set; }//Listado de errores
        public string SessionId { get; set; }//Id de sesión Guid



    }
}
