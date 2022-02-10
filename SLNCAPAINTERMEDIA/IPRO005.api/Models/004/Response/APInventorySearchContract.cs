using IPRO005.api.Models._003.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._004.Response
{
    public class APInventorySearchContract
    {
        public string BusinessUnit { get; set; }
        public string Warehouse { get; set; }
        public string Location { get; set; }
        public string Zone { get; set; }
        public string TypeLocationWHS { get; set; }
        public WMSLocationType TypeLocation { get; set; }
        public string Configuration { get; set; }
        public decimal AvailablePhysical { get; set; }
        public decimal PhysicalInventory { get; set; }
        public string DatetimeExecution { get; set; } //DateTime
    }
   
}
