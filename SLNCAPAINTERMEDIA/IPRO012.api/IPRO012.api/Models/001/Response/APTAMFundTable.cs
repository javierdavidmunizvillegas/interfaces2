using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO012.Models._001
{
    public class APTAMFundTable
    {
        public string FundID { get; set; }
        public string Description { get; set; }
        public decimal TotalFundAmt { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public TAMFundStatus Status { get; set; }
    }
   
}
