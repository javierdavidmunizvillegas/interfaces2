using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILOG001.api.Models._002.Request
{
    public class APILOG001002MessageRequest
    {
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }
        public List<WeHooksList2> weHooks { get; set; }
    }
}
