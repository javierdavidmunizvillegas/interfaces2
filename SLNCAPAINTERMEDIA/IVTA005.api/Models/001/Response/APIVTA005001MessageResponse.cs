using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA005.api.Models._001.Response
{
    public class APIVTA005001MessageResponse
    {
       
        public bool StatusId { get; set; }

        public List<string> ErrorList { get; set; }

        
        public APSalesOrderCanceled SalesOrderCanceled { get; set; }

       // public string TimeStartEnd { get; set; }

    }
}
