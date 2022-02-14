using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA007.Models._002.Request
{
    class APInvetAvailWebIVTA007002
    {
        public string ItemId { get; set; }
        public string DataAreaId { get; set; }
        public string InventLocationId { get; set; }
        public string WmsLocationId { get; set; }
        public decimal InventPhisical { get; set; }
        public decimal InventReser { get; set; }
        public decimal InventAvail { get; set; }
    }
}
