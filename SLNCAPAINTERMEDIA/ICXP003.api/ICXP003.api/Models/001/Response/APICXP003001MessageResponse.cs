using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICXP003.api.Models._001.Response
{
    public class APICXP003001MessageResponse
    {
      
        public string SessionId { get; set; }
        public List<APVendorContract> APVendorContractList { get; set; }      
        public Boolean StatusId { get; set; }    
        public List<string> ErrorList { get; set; }


    }
}
