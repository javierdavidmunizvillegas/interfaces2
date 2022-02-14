using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC020.Models.Response
{
    public class APISAC020001MessageResponse
    {
        
        public string SessionId { get; set; }

        public string TransferId { get; set; }

        
        public Boolean StatusId { get; set; }

        public string TimeStartEnd { get; set; }

        public List<string> ErrorList { get; set; }
    }
}
