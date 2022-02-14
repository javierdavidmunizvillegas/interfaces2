using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ003.Models._002.Request
{
    public class APDCAL003002MessageRequest
    {

        public string DataAreaId { get; set; }         
        public string SessionId { get; set; }         
        public string Enviroment { get; set; }         
        public APInformationReturn APInformationReturn { get; set; }
    }
}
