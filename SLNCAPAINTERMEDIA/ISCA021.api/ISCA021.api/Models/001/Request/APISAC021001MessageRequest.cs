using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC021.api.Models._001.Request
{
    public class APISAC021001MessageRequest
    {
        public string DataAreaId { get; set; }
        public string SessionId { get; set; }
        public string Enviroment { get; set; }        
        public APSalesTable APSalesTable { get; set; }
    }
}
