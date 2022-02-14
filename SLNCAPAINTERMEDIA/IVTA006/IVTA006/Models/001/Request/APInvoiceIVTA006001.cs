using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA006.Models._001.Request
{
    public class APInvoiceIVTA006001
    {
        public string InvoiceId { get; set; }
        public string UserId { get; set; }
        public DateTime DateTimeCreation { get; set; }
        public string BusinessUnit { get; set; }
        public string APStoreId { get; set; }
        public string Observacion { get; set; }
    }
}
