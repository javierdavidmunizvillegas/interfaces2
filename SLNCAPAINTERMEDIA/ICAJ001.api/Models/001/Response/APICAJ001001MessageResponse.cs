using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ001.api.Models._001.Response
{
    public class APICAJ001001MessageResponse
    {
        public List<APBankAccountTable> BankAccountTableList { get; set; }
        public bool StatusId { get; set; }
        public string TimeStartEnd { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
