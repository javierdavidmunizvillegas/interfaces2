using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG002_002.Models._002.Response
{
    public class DataLoginResponse
    {
        public CourierResponse courier { get; set; }
        public string token { get; set; }
    }
}
