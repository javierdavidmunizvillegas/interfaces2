using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG002_001.Models._001.Request
{
    public class DeliveryShipify
    {
        public PickupData pickup { get; set; }
        public DropoffData dropoff { get; set; }
        public List<PackageShipify> packages { get; set; }
        public string referenceId { get; set; }
        public decimal cod { get; set; }
        public decimal block { get; set; }
        public string metadata { get; set; }
        public List<string> tags { get; set; }
        public List<ExtraDataShipify> extraData { get; set; }
    }
   
}
