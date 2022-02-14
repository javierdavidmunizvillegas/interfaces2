using System;
using System.Collections.Generic;
using System.Text;

namespace ICAJ018.api.Models._001.Response
{
    public class APICAJ018001MessageResponse
    {
        public string SessionId { get; set; }
        public Boolean StatusId { get; set; }
        public List<string> ErrorList { get; set; }
        public APTypeTransactionResponse[] TypeTransactionList { get; set; }
    }
}
