using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO012.Models._002
{
    public class APIPRO012002MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }

        public string Enviroment { get; set; }

        public string SessionId { get; set; }

        public string[] ItemIdList { get; set; }
        public bool GetRelationProductList { get; set; }
        public bool GetAttributeList { get; set; }
    }
   
}
