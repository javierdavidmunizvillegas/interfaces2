using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO012.Models._007
{
    public class APIPRO012007MessageResponse
    {
        [Required]
        public string SessionId { get; set; }

        [Required]
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }

        public List<APEcoResProductDimensionGroup> APEcoResProductDimensionGroupList { get; set; }
    }
}
