using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE012.api.Models._001.Response
{
    public class APJournalTable
    {
        public string JournalNum { get; set; }
        public string Name { get; set; }
        public List<APJournalTrans> APJournalTransList { get; set; }

    }
}
