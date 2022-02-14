using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE001.api.Infraestructure.Configuration
{
    public class ConvierteCodigo
    {
        private static RegistroLog Logger = new RegistroLog();
        public string DynamicAcrecos(string codigodyn, string nombreInterface)
        {
            int at;
            String value = codigodyn;
            String codigo = string.Empty; ;
            value = value.Replace("c", "C");
            at = value.IndexOf("C", 0, value.Length);
            if (at == -1)
            {
                Logger.FileLogger(nombreInterface, "Convierte Código DynamicAcrecos: No contiene C " + codigodyn);
                return codigodyn;
            }
            int length = value.Length - (at + 1);
            codigo = value.Substring(at + 1, length);
            Logger.FileLogger(nombreInterface, "Convierte Código DynamicAcrecos: Codigo a Crecos " + codigo);
            return codigo;
        }
        public string CrecosAdynamics(string codigocre, int longcodigo, string nombreInterface) // longcodigo= longitud del campo codigo cliente a rellenar de ceros  menos uno, porque va primero la C
        {
            String codigo = string.Empty; ;
            int length = codigocre.Length;
            if (length > longcodigo)
            {
                Logger.FileLogger(nombreInterface, "Convierte Código CrecosAdynamics: Longitud de código es mayor al limite " + codigocre);
                return codigocre;
            }
            else if (length == 0)
            {
                Logger.FileLogger(nombreInterface, "Convierte Código CrecosAdynamics: Código vacío " + codigocre);
                return codigocre;
            }
            codigo = "C" + codigocre.PadLeft(longcodigo, '0');
            Logger.FileLogger(nombreInterface, "Convierte Código CrecosAdynamics: Código a Crecos " + codigo);
            return codigo;
        }
    }
}
