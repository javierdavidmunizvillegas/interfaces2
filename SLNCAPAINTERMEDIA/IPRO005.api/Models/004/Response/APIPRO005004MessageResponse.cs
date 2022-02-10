using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace IPRO005.api.Models._004.Response
{
    public class APIPRO005004MessageResponse
    {
        
        public string SessionId { get; set; }
       
        public List<APItemsContractIPRO005004> apInventTableList { get; set; }
        public bool StatusId { get; set; }
       // public string TimeStartEnd { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
