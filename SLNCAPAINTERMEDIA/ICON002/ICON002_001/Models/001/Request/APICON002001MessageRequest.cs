using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_001.Models._001.Request
{
    public class APICON002001MessageRequest
    {
        public string DataAreaId { get; set; }        
        public string Enviroment { get; set; }
        public string SessionId { get; set; }
        public FacturaView ReturnTable { get; set; }
    }
}
