using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._006.Response
{
    public class APIPRO005006MessageResponse
    {
       
        public string SessionId { get; set; }
        
        public List<APMovementSearchContract> APMovementSearchContractList { get; set; }
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }
       // public string TimeStartEnd { get; set; }

    }
}
