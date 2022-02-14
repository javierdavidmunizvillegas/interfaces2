using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG002_004.Models._001.Response
{
    public class ShipifyResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public Payload payload { get; set; }
    }
}
