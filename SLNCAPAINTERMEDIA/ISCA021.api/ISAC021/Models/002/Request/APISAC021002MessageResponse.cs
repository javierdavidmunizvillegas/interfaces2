using System;
using System.Collections.Generic;
using System.Text;

namespace ISAC021.Models._002.Request
{
    public class APISAC021002MessageResponse
    {
        public string SalesId { get; set; }
        public string InvoiceId { get; set; }
        public string NumAuthorization { get; set; }
        public DateTime DateAuthorization { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }
        public string Status { get; set; }
        public string DescriptionId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
