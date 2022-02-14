using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICXP003.api.Models._002.Response
{
    public class APICXP003002MessageResponse
    {
      
        public string SessionId { get; set; }

        public string DiarioDeFactura { get; set; }

        public string Voucher { get; set; }

      
        public Boolean StatusId { get; set; }

        public List<string> ErrorList { get; set; }
    }
}
