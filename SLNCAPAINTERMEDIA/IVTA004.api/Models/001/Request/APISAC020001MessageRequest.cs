using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA004.api.Models._001.Request
{
    public class APISAC020001MessageRequest
    {
        
        public string DataAreaId { get; set; }
        public string SessionId { get; set; }
        public string Enviroment { get; set; }
        public APSalesTableIVTA004001 APSalesTable { get; set; }
    }
}
