using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC021.api.Models._001.Request
{
    public class APSalesTable
    {
        
        public string PurchOrderFormNum { get; set; }        
        public string CustAccount { get; set; }        
        public string InvoiceAccount { get; set; }        
        public string SequenceGroup { get; set; }        
        public APSalesLine[] APSalesLineList { get; set; }
    }
}
