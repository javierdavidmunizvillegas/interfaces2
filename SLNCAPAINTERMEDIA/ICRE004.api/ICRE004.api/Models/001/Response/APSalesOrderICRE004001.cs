using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE004.api.Models._001.Response
{
    public class APSalesOrderICRE004001
    {
        public APSalesOrderHeader APSalesOrderHeader { get; set; }
        public APDiaryJournal[] APDiaryJournalList { get; set; }
        public APBilBuyer[] APBilBuyerList { get; set; }
    }
}
