using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class GTM004Response: Base
    {
        public int statusCode { get; set; }
        public string descripcionId { get; set; }
        public List<string> errorList { get; set; }

        //public int codigoTransaccion { get; set; }
        //public string estadoTransaccion { get; set; }
        //public string descripcionId { get; set; }
        //public string descripcionTransaccion { get; set; }

    }
}
