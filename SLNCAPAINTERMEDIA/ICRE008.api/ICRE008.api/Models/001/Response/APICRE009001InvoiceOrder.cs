using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE008.api.Models._001.Response
{
    public class APICRE009001InvoiceOrder
    {
        public string APStoreId { get; set; }
        public string APStoreName { get; set; }
        public string DescriptionEstablishment { get; set; }
        public string CustGroup { get; set; }
        public APLogisticsPostalAddress APLogisticsPostalAddress { get; set; }
        public APInvoice APInvoice { get; set; }
        public APSalesTable APSalesTable { get; set; }
        public List<APSalesLine> SalesLineList { get; set; }
    }
}
