using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.api.Models._001.Response
{
    public class APISAC017001MessageResponse
    {
       
        public string SessionId { get; set; }
        public string ReturnItemNum { get; set; } // Número de orden de devolución

        public bool StatusId { get; set; }
       // public string TimeStartEnd { get; set; } 
        public List<string> ErrorList { get; set; }
    }
}
