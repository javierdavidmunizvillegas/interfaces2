using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA007.Models._002.Request
{
    class APIVTA007002MessageRequest
    {
        public string Enviroment { get; set; }
        public APInvetAvailWebIVTA007002[] APInventAvailWebList { get; set; }
        public string SessionId { get; set; }
    }
}
