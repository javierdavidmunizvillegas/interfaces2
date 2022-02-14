using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO012.Models
{
    public class APIPRO012001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }

        public string Enviroment { get; set; }

        public string SessionId { get; set; }

        public TAMFundStatus Status { get; set; }

        public DateTime TransDate { get; set; }
    }
     public enum TAMFundStatus { Planning = 0, Approved = 2, Closed = 4 }

}
