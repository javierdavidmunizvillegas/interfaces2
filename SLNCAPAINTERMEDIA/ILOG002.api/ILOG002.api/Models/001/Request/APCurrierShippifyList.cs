using ILOG002.api.Models._001.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ILOG002.api.Models
{
    public class APCurrierShippifyList
    {
        public string routeId { get; set; }
        public DateTime createdAt { get; set; }
        public string courierVehicleLicensePlate { get; set; }
        public string courierDocumentId { get; set; }
        public string courierName { get; set; }
        public List<StepsList> steps { get; set; }

    }
  
}
