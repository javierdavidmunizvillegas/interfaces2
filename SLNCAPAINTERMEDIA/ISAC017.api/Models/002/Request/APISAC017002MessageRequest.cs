using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.api.Models._002.Request
{
    public class APISAC017002MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }
       
        public string SessionId { get; set; }
       
        public string Enviroment { get; set; }
        [Required]
        public List<APReturnTableDisposition> APReturnTableDispositionList { get; set; }

    }
}
