using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.Models
{
    class ResponseWS
    {
        public int statusCode { get; set; }//Http Status Code
        public string response { get; set; }//Codigo Siac
        public string descripcionId { get; set; }//“OK”, “ERROR”
        public List<string> errorList { get; set; }//Descripción
    }
}
