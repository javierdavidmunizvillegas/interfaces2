using System;
using System.Collections.Generic;
using System.Text;

namespace ICAJ011.Models._001.Request
{
    public class APICAJ011001MessageRequest
    {
        public string DataAreaId { get; set; }
        public string SessionId { get; set; }
        public string Enviroment { get; set; }
        public APDocumentElectronic DocumentElectronic { get; set; }
    }
}
