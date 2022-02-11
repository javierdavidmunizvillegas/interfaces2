using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA007.Models._001.Request
{
    class APIVTA007001MessageRequest
    {
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public APInventTableWebIVTA007001[] APInventTableWebList { get; set; }
        public string SessionId { get; set; }
     
    }
}
