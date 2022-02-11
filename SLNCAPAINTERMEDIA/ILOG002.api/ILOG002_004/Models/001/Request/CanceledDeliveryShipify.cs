using ILOG002_004.Models._001.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG002_001.Models._001.Request
{
    public class CanceledDeliveryShipify
    {
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }        
        public string taskIds { get; set; }
        public Location location { get; set; }
        public int reason_id { get; set; }
        public string reason { get; set; }
        public string cancel_note { get; set; }
    }
}
