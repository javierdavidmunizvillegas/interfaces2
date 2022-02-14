using ILOG002_002.Models._001.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG002_001.Models._001.Request
{
    public class DeliveryChanges
    {
        public List<PackageShipify> packages { get; set; }  
        public int cod { get; set; }
        public string referenceId { get; set; }
        public decimal block { get; set; }
    }
   
}
