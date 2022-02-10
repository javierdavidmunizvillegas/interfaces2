using System;
using System.Collections.Generic;
using System.Text;

namespace APICAJ008002.Models._003.Response
{
    class ResponseHomologa003
    {
        public int StatusCode { get; set; }//Http Status Code
        public string Response { get; set; }//Codigo Siac
        public string DescripcionId { get; set; }//“OK”, “ERROR”
        public List<string> ErrorList { get; set; }//Descripción
    }
}
