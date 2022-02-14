using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG002_001.Models._001.Request
{
    public class PickupData
    {
        public PickupContactData contact { get; set; }
        public PickupLocationData location { get; set; }
    }
}
