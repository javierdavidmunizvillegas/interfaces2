using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB003.api.Models._002.Response
{
    public class APCustInvoiceTableResponse
    {
        public string APIdentificationList { get; set; }//Identificador de lista
        public string Voucher { get; set; }//ID asiento
        public string InvoiceId { get; set; }//Número de factura
        public bool StatusId { get; set; }//Estado true = ok y False = Error Boolean 
        public List<string> ErrorList { get; set; }//Listado de errores
    }
}
