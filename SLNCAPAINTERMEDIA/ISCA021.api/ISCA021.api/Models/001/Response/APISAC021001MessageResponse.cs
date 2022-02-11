using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC021.api.Models._001.Response
{
    public class APISAC021001MessageResponse
    {
        
        public string SessionId { get; set; }
        public string SalesId { get; set; }
        //public string InvoiceId { get; set; }
        //public string TimeStartEnd { get; set; }

        
        public Boolean StatusId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
