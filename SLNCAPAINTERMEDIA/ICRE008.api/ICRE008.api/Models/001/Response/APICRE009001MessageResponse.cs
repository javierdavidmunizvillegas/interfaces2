using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE008.api.Models._001.Response
{
    public class APICRE009001MessageResponse
    {
        [Required]
        public string SessionId { get; set; }
        public List<APICRE009001InvoiceOrder> apicrE009001InvoiceOrderList { get; set; }
        [Required]
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
