using IVTA006.Models._001.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA006.Models._001.Response
{
    public class APIVTA006001MessageResponse
    {
        public string SessionId { get; set; }
        public List<APInvoiceIVTA006001> APInvoiceList { get; set; }
        public bool StatusId { get; set; }
        public string[] ErrorList { get; set; }
    }
}
