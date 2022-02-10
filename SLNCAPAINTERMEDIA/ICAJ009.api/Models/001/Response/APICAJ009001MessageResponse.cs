using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ009.api.Models._001.Response
{
    public class APICAJ009001MessageResponse
    {
        public List<APCustInvoiceServiceResponse> APCustInvoiceServiceResponseList { get; set; } //Listado de facturas de servicios
        public bool StatusId { get; set; } // Estado del registro 1=OK; 0=ERROR string
        public string SessionId { get; set; }
        public List<string> ErrorList { get; set; } //Detalle del error
    }

}
