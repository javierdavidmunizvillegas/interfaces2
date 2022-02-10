using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._003.Response
{
    public class APWarehouseSearchContract
    {
        [Required]
        public string WarehouseCode { get; set; }
        [Required]
        public string WarehouseDescription { get; set; }
        public List<APWmsLocationContract> LocationList { get; set; }
    }
}
