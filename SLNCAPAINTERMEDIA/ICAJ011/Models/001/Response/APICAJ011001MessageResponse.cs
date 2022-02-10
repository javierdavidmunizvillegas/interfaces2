using System;
using System.Collections.Generic;
using System.Text;

namespace ICAJ011.Models._001.Response
{
    public class APICAJ011001MessageResponse
    {
        public string SessionId  { get; set; }
        public Boolean StatusId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
