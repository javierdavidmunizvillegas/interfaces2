using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG002_001.Models._001.Request
{
    public class PackageShipify
    {
        public string id { get; set; }
        public string name { get; set; }
        public decimal size { get; set; }
        public decimal qty { get; set; }
        public decimal weight { get; set; }
        public PackageExtras extras { get; set; }
    }
}
