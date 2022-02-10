using ICAJ002.api.Models._001.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ002.api.Models._001.Response
{
    public class APICAJ002001MessageResponse
    {
        public List<APCustPaymModeTable> CustPaymModeTableList { get; set; } // Listado de Formas de Pago
        public string SessionId { get; set; } //Id de sesión
        public Boolean StatusId { get; set; } //Estado del registro 1=OK; 0=ERROR
        public List<string> ErrorList { get; set; } //Detalle del error
        
    }
}
