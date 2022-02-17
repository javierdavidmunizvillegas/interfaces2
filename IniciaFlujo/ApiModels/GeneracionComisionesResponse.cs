using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class GeneracionComisionesResponse
    {
        public int statusCode { get; set; }
        public string response { get; set; }
        public string descripcionId { get; set; }
        public List<string> errorList { get; set; }
    }
}
