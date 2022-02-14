using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA019.api.Models.Response
{
    public class APIVTA019001MessageResponse
    {
        
        public string SessionId { get; set; }        
        public Boolean StatusId { get; set; }
        public List<string> ErrorList { get; set; }
        public string SalesId { get; set; }
        public string VoucherProvision { get; set; }
    }
}
