using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE002.api.Models._002.Request
{
    public class APIndependetEntrepContractICRE001002
    {
        
        public string APCodeIndependentVend { get; set; }

        
        public string VATNum { get; set; }

        
        public string FirstName { get; set; }

        public string SecondName { get; set; }

        
        public string LastName { get; set; }

        
        public DateTime DateEntryIndependetEntrep { get; set; }

        
        public APRelationStatus APRelationStatus { get; set; }

        
        public APBlocked APBlocked { get; set; }

        
        public string APCodeReason { get; set; }

        
        public string APDescriptionReason { get; set; }
        
        public DateTime DateBlockTmpFrom { get; set; }

        public DateTime DateBlockTmpTo { get; set; }

        public string CodeParent { get; set; }

        public DateTime DateParent { get; set; }

        public DateTime DateDown { get; set; }

        public List<APContactInfoContract> APContactInfoList { get; set; }
    }
}
