using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA004.api.Models._001.Response
{
    public class APIVTA004001MessageResponse
    {
        public string SessionId { get; set; }//Id de sesión
        public string SalesId { get; set; }//Listado de artículos
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }//Detalle del error
        //public string TimeStartEnd { get; set; }
    

    }
}
