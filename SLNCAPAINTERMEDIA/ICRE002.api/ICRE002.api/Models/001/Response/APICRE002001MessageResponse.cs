using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE002.api.Models.Response
{
    public class APICRE002001MessageResponse
    {
        
        public string SessionId { get; set; }

        public APCustTableContract APCustTable { get; set; }

        
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }
    }
}
