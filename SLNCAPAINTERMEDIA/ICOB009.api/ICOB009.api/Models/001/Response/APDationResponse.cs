using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB009.api.Models._001.Response
{
    public class APDationResponse
    {
        public string DationRequestNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public string DocumentNumber { get; set; }
        public string NameDeliveringItems { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string CustAccount { get; set; }
        public string AccountName { get; set; }
        public string Status { get; set; }       
        public List<APDationLineResponse> APDationLineResponse { get; set; }
    }
}
