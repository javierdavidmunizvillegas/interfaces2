using IVTA014.Models._001.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA014.Models._001.Response
{
    class APIVTA014001MessageResponse
    {
        public string SessionId { get; set; } //Guid . Id de sesion
        public List<APCustInvoiceJourIVTA014001> APCustInvoiceJourList { get; set; } // ID de asiento
        public bool StatusId { get; set; } // Estado true = ok y False = Error
        public List<string> ErrorList { get; set; } // Listado de errores 
    }
}
