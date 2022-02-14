using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ILOG002.api.Models._001.Response
{
    public class APILOG002003MessageResponse
    {
        
        public string SessionId { get; set; }        
        public Boolean StatusId { get; set; }      
        public List<string> ErrorList { get; set; }    
    }
}
