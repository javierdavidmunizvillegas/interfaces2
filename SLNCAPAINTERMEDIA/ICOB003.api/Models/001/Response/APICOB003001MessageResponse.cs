using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB003.api.Models._001.Response
{
    public class APICOB003001MessageResponse
    {
        public string APIdentificationList { get; set; }//Identificador de lista
        public string VoucherReverse { get; set; }//ID asiento del reverso
        public string CustAccount { get; set; }//Código de cliente
        public decimal Amount { get; set; }//Monto
        public string VoucherOriginal { get; set; }//ID asiento original
        public bool StatusId { get; set; }//Estado true = ok y False = Error Boolean
        public List<string> ErrorList { get; set; }//Listado de errores


    }
}
