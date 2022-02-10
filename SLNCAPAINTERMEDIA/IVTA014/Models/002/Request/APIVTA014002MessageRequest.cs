using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA014.Models._002.Request
{
    class APIVTA014002MessageRequest
    {
        public List<APCustSettlementIVTA014002> APCustSettlemenList { get; set; } // Listado factura 

        public string DataAreaId { get; set; } //Id de la compañía 
        public string Enviroment { get; set; } // Ambiente
        public string SessionId { get; set; } //Id de Sesion

    }
}
