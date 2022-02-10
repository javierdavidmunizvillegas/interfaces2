using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.api.Models._002.Response
{
    public class APISAC017002MessageResponse
    {
        
        public string SessionId { get; set; }
        public string SalesId { get; set; }
        public string DescriptionId { get; set; }
       // public string TimeStartEnd { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
