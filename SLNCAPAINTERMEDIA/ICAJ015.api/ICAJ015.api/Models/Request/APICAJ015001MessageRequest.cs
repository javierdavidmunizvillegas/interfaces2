using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ015.api.Models
{
    public class APICAJ015001MessageRequest
    {
        
        public string DataAreaId { get; set; }
        public string Environment { get; set; }       
        public string SessionId { get; set; }        
        public string[] BusinessUnitList { get; set; }        
        public string[] ChannelList { get; set; }        
        public DateTime TransDate { get; set; }

    }
}
