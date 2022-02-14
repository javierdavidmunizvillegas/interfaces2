using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG002_001.Models._001.Request
{
    public class ConfirmedDeliveryShipify
    {
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }        
        public string deliveryIds { get; set; }
        public DeliveryChanges deliveryChanges { get; set; }
        public string[] removeTags { get; set; }
        public string[] newTags { get; set; }
    }
}
