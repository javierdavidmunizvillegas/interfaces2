using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA006.Models._001.Request
{
    public class APIVTA006001MessageRequest
    {
        public string Enviroment { get; set; }
        public List<APInvoiceIVTA006001> APInvoiceIVTA006001 { get; set; }
        public string SessionId { get; set; }
        public string DataAreaId { get; set; }

    }
}
