using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO012.Models._007
{
    public class APEcoResProductDimensionGroup
    {
        public string EcoResProductDimensionGroupName { get; set; }
        public List<APEcoResProductDimensionGroupFldSetup> APEcoResProductDimensionGroupFldSetupList { get; set; }
    }
}
