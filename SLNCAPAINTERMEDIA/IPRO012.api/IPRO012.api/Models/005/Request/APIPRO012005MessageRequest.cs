using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO012.Models._005
{
    public class APIPRO012005MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }

        public string Enviroment { get; set; }

        public string SessionId { get; set; }
        public string ItemPackagingGroupId { get; set; }
    }
}
