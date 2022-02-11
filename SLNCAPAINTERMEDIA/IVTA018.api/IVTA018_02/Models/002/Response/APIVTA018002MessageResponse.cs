using IVTA018_02.api.Models._002.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA018_02.Models._002.Response
{
    public class APIVTA018002MessageResponse
    {
        public string SessionId { get; set; }
        public string Registrationid { get; set; }
        public string ItemId { get; set; }
        public string DataAreaId { get; set; }
        public string AttString { get; set; }
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }
        public APPriceDetails[] APPriceDetails { get; set; }
    }
}
