using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE008.api.Models._001.Response
{
    public class APLogisticsPostalAddress
    {   
        public string City { get; set; }

        [Required]
        public string CountryRegionId { get; set; }
        [Required]
        public string Description { get; set; }
        public string State { get; set; }        
        public string Street { get; set; }
    }
}
