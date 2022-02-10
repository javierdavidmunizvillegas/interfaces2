using ICRE007.api.Models._001.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE007.api.Models._002.Request
{
    public class APPostingProfile
    {
                
        public string PostingProfile { get; set; }
        public string Description { get; set; }
        public List<APCustGroup> CustGroupList { get; set; }
    }
}

