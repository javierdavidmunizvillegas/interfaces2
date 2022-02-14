using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ003.Models._001.Request
{
    public class APDCAJ003001MessageRequest
    { 
        public string DataAreaId { get; set; }
        public string SessionId { get; set; }         
        public string Enviroment { get; set; }         
        public APInformationSale InformationSale { get; set; }
    }
}
