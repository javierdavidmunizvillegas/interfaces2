using System;
using System.Collections.Generic;
using System.Text;

namespace ICAJ011.Models._001.Request
{
    public class APDocumentElectronic
    {
        public string DocumentNumber { get; set; }
        public string TypeOfDocument { get; set; }
        public string RelatetDocument { get; set; }
        public string DateTimeInterface { get; set; } //datettime
        public string DateTimeInterfaceSRI { get; set; } // datetime
        public string AuthorizationNumber { get; set; }
        public string DateDocumentRegistration { get; set; } // datetime
        public string UserGeneratesInterface { get; set; }//ESTE CAMPO POR PREGUNTAR

    }
}
