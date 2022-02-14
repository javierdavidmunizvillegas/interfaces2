using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO004.Models
{
    public class APInventTableIPRO004001
    {
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public string ProductLifecycleStateId { get; set; }
        public Boolean IsActiveForPlanning { get; set; }
        public DateTime DateLastPurchase { get; set; }
        public List<APInventoryAvailableIPRO004001> APInventoryAvailableList { get; set; }
    }
}
