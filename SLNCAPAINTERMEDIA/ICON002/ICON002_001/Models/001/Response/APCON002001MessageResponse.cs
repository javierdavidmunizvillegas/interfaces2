using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICON002_001.Models._001.Response
{
    public class APCON002001MessageResponse
    {
        public string FacturaId { get; set; }
        public string SessionId { get; set; }
        public bool StatusId { get; set; }
        public string[] ErrorList { get; set; }
    }
}
