using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ015.api.Models.Response
{
    public class APFinancialPlanICAJ015001
    {
        public string CodeFinancialPlanId { get; set; }
        public string DescriptionFinancialPlan { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TypeFinancialPlanId { get; set; }
        public string DescriptionTypeFinancialPlan { get; set; }
        public string GroupFinancialPlanId { get; set; }
        public string DescriptionGroupFinancialPlan { get; set; }
        public List<APBankAdquiringICAJ015001> BankAcquiringList { get; set; }
        public List<APAssistanceFacilitatesICAJ015001> AssistanceFacilitatesList { get; set; }
        public List<APBusinessUnit> APBusinessUnitList { get; set; }
        public List<APChannel> APChannelList { get; set; }
    }
}
