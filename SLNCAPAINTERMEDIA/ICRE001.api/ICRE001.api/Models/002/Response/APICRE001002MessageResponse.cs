using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE001.Models
{
    public class APICRE001002MessageResponse
    {
        
        public string SessionId { get; set; }        
        public APIndependetEntrepContractICRE001002 APIndependetEntrep { get; set; }        
        public Boolean StatusId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
