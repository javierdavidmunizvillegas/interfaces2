using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO012.Models._003
{
    public class APIPRO012003MessageResponse
    {
        public string SessionId { get; set; }
        public Boolean StatusId { get; set; }
        public string TimeStartEnd { get; set; }
        public List<string> ErrorList { get; set; }
        public List<APInventTableBusinessUnit> APInventTableBusinessUnitList { get; set; }
    }
}
