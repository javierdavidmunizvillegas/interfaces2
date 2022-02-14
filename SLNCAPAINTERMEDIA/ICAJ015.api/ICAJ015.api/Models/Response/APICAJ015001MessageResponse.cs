using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ015.api.Models.Response
{
    public class APICAJ015001MessageResponse
    {
        
        public string SessionId { get; set; }        
        public Boolean StatusId { get; set; }
        public List<string> ErrorList { get; set; }
        public List<APFinancialPlanICAJ015001> FinancialPlanList { get; set; }
    }
}
