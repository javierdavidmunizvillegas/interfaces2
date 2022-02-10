using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ009.api.Models._001.Request
{
    public class APICAJ009001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; } //Id de la compañía 
        public string SessionId { get; set; } // Id de sesión

        public string Enviroment { get; set; } // Id del ambiente
        [Required]
        public List<APCustInvoiceServicesContract> APCustInvoiceServicesContractList { get; set; } // Listado de facturas de servicios

    }
}
