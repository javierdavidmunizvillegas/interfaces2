using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA005.api.Models._001.Response
{
    public class APSalesOrderCanceled
    {
        public string SalesId { get; set; }
        public string SalesStatus { get; set; }
        public string DateCanceled { get; set; } // datetime
    }
}
