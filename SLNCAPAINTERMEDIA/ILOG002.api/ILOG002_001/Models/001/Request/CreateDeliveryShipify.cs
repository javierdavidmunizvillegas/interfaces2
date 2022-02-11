using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG002_001.Models._001.Request
{
    public class CreateDeliveryShipify
    {
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }
        public List<DeliveryShipify> deliveries { get; set; }
        public Boolean flexible { get; set; }
        public string groupId { get; set; }
    }
}
