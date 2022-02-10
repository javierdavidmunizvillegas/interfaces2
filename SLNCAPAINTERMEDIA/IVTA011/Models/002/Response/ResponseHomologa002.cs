using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA011.Models._002.Response
{
    class ResponseHomologa002
    {
        public int StatusCode { get; set; }//Http Status Code
        public string Response { get; set; }//Codigo Siac
        public string DescripcionId { get; set; }//“OK”, “ERROR”
        public List<string> ErrorList { get; set; }//Descripción
    }
}
