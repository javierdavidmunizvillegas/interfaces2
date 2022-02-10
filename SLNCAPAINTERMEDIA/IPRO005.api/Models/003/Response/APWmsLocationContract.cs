using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._003.Response
{
    public class APWmsLocationContract
    {
      
        public string LocationId { get; set; }
      
      //  public string LocationDescription { get; set; }
        public string TypeLocationWHS { get; set; }
        public string WMSLocationId { get; set; }

        public WMSLocationType TypeLocation { get; set; }

    }
    public enum WMSLocationType
    {
        Buffer = 0,
        Pick = 1,
        InputPort = 2,
        OutputPort = 3,
        ProductionInput = 4,
        InspectionLocation = 5,
        KanbanSupermarket = 6
    }
}
