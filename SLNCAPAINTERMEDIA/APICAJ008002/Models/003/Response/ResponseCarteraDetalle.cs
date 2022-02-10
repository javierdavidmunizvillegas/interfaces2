using System;
using System.Collections.Generic;
using System.Text;

namespace APICAJ008002.Models._003.Response
{
    class ResponseCarteraDetalle
    {
        public long OperationId { get; set; }//Codigo operacion
        public List<ResponseCarteraFactura> InvoiceList { get; set; }
       


    }
}
