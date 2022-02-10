using ICRE007.api.Models._001.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE007.api.Models._002.Response
{
    public class APICRE007002MessageResponseLegado
    {
          public string PostingProfile { get; set; } //Código del perfil contable
          public string Description { get; set; } //Descripción del perfil contable
          public string SessionId { get; set; } //Id de sesión

          public List<APCustGroup> CustGroupListt { get; set; }//Lista de grupos de clientes asociados al perfil contable
        /*
        public string DataAreaId { get; set; }//Id de la compañía 

        public string prioridad { get; set; }//Prioridad tipo 

        public string SessionId { get; set; }//Id de sesión */
    }
}
