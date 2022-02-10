using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ012.api.Models._002.Response
{
    public class APICAJ012002MessageResponse
    {
       
        public string SessionId { get; set; }
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }
          
        public List<APReasonIngressEgressTable> APReasonIngressEgressTableList { get; set; }
    }
}
