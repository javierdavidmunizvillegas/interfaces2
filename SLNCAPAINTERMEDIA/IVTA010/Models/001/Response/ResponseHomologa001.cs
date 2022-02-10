using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA010.Models._001.Response
{
    class ResponseHomologa001
    {
        public int StatusCode { get; set; }//Http Status Code
        public string Response { get; set; }//Codigo Siac
        public string DescripcionId { get; set; }//“OK”, “ERROR”
        public List<string> ErrorList { get; set; }//Descripción
    }
}
