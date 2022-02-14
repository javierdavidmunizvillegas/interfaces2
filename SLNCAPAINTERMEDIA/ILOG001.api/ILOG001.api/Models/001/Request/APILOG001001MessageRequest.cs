using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILOG001.api.Models._001.Request
{
    public class APILOG001001MessageRequest
    {
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }
        public List<WeHooksList> weHooks { get; set; }
    }
}
