using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class GeneracionCarteraResponse: Base
    {
        public string StatusCode { get; set; }
        public List<string> MessageError { get; set; }
        public List<ResponseCarteraDetalle> OperationList { get; set; }

        public GeneracionCarteraResponse()
        {
            OperationList = new List<ResponseCarteraDetalle>();
        }
    }
    public partial class ResponseCarteraDetalle
    {
        public string OperationId { get; set; }
        public List<ResponseCarteraFactura> InvoiceList { get; set; }

        public ResponseCarteraDetalle()
        {
            InvoiceList = new List<ResponseCarteraFactura>();
        }
    }

    public partial class ResponseCarteraFactura
    {
        public string InvoiceId { get; set; }
        public string FatherInvoice { get; set; }
    }
}
