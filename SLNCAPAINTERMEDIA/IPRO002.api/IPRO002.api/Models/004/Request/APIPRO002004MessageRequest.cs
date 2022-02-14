using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO002.api.Models._004.Request
{
    public class APIPRO002004MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }

        public string Enviroment { get; set; }

        public string SessionId { get; set; }

        public List<string> InventLocationIdList { get; set; }

        public List<string> ItemIdList { get; set; }

        public List<string> LineNameList { get; set; }

        public string BusinessUnit { get; set; }
    }
}
