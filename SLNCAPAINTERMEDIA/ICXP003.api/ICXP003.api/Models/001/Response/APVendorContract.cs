using ICXP003.api.Models._001.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICXP003.api.Models._001.Response
{
    public class APVendorContract
    {
      
        public APCreateVendorContract2 APCreateVendorContract { get; set; }      
        public Boolean StatusId { get; set; }      
        public string[] ErrorList { get; set; }
    }
}
