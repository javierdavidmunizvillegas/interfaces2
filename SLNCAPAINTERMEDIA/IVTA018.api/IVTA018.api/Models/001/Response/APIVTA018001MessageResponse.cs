using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA018.api.Models._001.Response
{
    public class APIVTA018001MessageResponse
    {
        public string SessionId { get; set; }
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }
        public APPriceDetails[] APPriceDetails { get; set; }    
    }
}
