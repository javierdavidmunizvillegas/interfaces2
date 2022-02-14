using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ015.api.Models.Response
{
    public class APBankAdquiringICAJ015001
    {
        public string BankAcquiringId { get; set; }
        public string DescriptionBankAcquiring { get; set; }
        public int MonthsTerms { get; set; }
        public decimal FinancialRate { get; set; }
        public string SequenceId { get; set; }
        public int DaysGrace { get; set; }
    }
}
