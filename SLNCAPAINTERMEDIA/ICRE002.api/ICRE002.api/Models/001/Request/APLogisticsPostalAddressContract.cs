using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE002.api.Models
{
    public class APLogisticsPostalAddressContract
    {
        
        public string Description { get; set; }
        public LogisticsLocationRoleType Role { get; set; }        
        public bool IsPrimary { get; set; }        
        public string CountryRegionId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string District { get; set; }
        public string StreetNumber { get; set; }
        public List<APContactInfoContract> APContactInfoList { get; set; }
        public Int64 LocationId { get; set; }
        public Int64 RecId { get; set; }
    }

    public enum LogisticsLocationRoleType { Delivery = 2, Business = 8 }
}
