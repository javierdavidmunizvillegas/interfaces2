using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG002_001.Models._001.Response
{
    public class Payload
    {
        public string id { get; set; }
        public int index { get; set; }
        public decimal price { get; set; }
        public string currencyCode { get; set; }
        public decimal distance { get; set; }
        public int cityId { get; set; }
        public int insurance { get; set; }
        public string statusDelivery { get; set; }
        public int vehicleCapacity { get; set; }
        public DateTime deliveryDate { get; set; }
    }
}
