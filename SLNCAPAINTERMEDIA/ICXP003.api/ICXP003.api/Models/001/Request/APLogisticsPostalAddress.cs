using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICXP003.api.Models._001.Request
{
    public class APLogisticsPostalAddress
    {
        public string Description { get; set; }
        public Role Role { get; set; }
        public Boolean IsPrimary { get; set; }
        public string CountryRegionId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }        
        public APStatusPostal APStatusPostal { get; set; }
    }
    public enum Role { Invoice = 1, Delivery = 2, Business = 8 }
    public enum APStatusPostal { Nuevo = 1, Actual = 2, Modificacion = 3 }
}
