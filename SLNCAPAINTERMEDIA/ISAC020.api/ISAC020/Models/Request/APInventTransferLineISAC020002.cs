using System;
using System.Collections.Generic;
using System.Text;

namespace ISAC020.Models.Request
{
    class APInventTransferLineISAC020002
    {
        public string ItemId { get; set; }
        public decimal Qty { get; set; }
        public string InventLocationIdFrom { get; set; }
        public string InventLocationIdTo { get; set; }
        public string WMSLocationIdFrom { get; set; }
        public string WMSLocationIdTo { get; set; }

    }
}
