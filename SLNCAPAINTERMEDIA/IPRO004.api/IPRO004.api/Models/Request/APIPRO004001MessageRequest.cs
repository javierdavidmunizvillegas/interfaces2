using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO004.Models
{
    public class APIPRO004001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }

        public string Enviroment { get; set; }     
        
        public string SessionId { get; set; }

        [Required]
        public string ItemId { get; set; }

        [Required]
        public string BusinessUnit { get; set; }

        [Required]
        public StatusReceipt StatusReceipt { get; set; }
    }

    public enum StatusReceipt { Purchased = 1, Received = 2, Registered = 3, Arrived = 4, Ordered = 5, QuotationReceipt = 6 }

}
