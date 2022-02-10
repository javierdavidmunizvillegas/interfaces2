using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA005.api.Models._001.Request
{
    public class APIVTA005001MessageRequest
    {
        [Required]
        public string SalesId { get; set; }

        public string CustomerRef { get; set; }

        [Required]
        public string DataAreaId { get; set; }

       
        public string Enviroment { get; set; }

        
        public string SessionId { get; set; }
    }
}
