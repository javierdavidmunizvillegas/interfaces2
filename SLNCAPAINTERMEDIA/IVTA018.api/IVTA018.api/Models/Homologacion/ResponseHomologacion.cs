using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA018.api.Models.ResponseHomologacion
{
    public class ResponseHomologacion
    {
        public int StatusCode { get; set; }
        public string Response { get; set; }
        public string DescripcionId { get; set; }
        public List<string> ErrorList { get; set; }
    }
}
