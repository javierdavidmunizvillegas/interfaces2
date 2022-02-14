using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE006.api.Models._001.Response
{
    public class APICRE006001MessageResponse
    {
        public APCustTable APCustTable { get; set; }
        public string SessionId { get; set; }
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }


    }
    
}
