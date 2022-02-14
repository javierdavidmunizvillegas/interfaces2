using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG002_001.Models._001.Response
{
    public class ResponseCreateShipify
    {
        public string SessionId { get; set; }
        public Boolean DescriptionId { get; set; }
        public string[] ErrorList { get; set; }
        public List<Payload> payload { get; set; }
        public string message { get; set; }
        public string code { get; set; }
    }
}
