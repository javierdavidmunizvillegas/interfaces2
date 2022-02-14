using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILOG001.api.Models._002.Request
{
    public class WeHooksList2
    {
        public DateTime eventCreatedAt { get; set; }
        public string eventType { get; set; }
        public string id { get; set; }
        public string notes { get; set; }
        public string routeId { get; set; }
        public string shipperId { get; set; }
    }
}
