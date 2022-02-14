using ILOG001.api.Infraestructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILOG001.api.Infraestructure.Configuration
{
    public class ConvierteCodigo
    {
        private static RegistroLog Logger = new RegistroLog();
        public string Obtenervalor(string codigodyn, string nombreInterface)
        {
            int at;
            String value = codigodyn.Trim();
            String codigo = string.Empty; ;
            //value = value.Trim(new Char[] { ' ', ' \' });

            at = value.IndexOf("comment", 0, value.Length);


            if (at == -1)
            {
                Logger.FileLogger(nombreInterface, "Campo notes vacío " + codigodyn);
                return codigodyn;
            }
            int length = value.Length - (at + 1);
            codigo = value.Substring(at + 11, length);
            Logger.FileLogger(nombreInterface, "Recorta cadena notes " + codigo);
            return codigo;
        }
       
    }
}
