using ICRE004.api.Models._001.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE004.api.Models._001.Response
{
    public class APDiaryJournal
    {
        public string Voucher { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public Boolean AdvanceLiquidatedCheck { get; set; }

    }

}
