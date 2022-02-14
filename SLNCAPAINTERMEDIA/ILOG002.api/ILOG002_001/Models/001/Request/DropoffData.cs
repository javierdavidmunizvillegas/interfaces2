using System;
using System.Collections.Generic;
using System.Text;

namespace ILOG002_001.Models._001.Request
{
    public class DropoffData
    {
        public DropoffContactData contact { get; set; }
        public DropoffLocationData location { get; set; }
    }
}
