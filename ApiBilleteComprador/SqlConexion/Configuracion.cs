using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConexion
{
    public class Configuracion
    {
        public string Key { get; set; }
        public string Ambiente { get; set; }
        public string Url { get; set; }
        public int TimeOut { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Language { get; set; }
    }
}
