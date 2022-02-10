using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA010.Infraestructura.Servicios
{
    public class EncriptaDato
    {
        public string Encriptar(string _cadenaAencriptar)
        {
           /* string result = string.Empty;
            byte[] encryted = System.Text.Encoding.Unicode.GetBytes(_cadenaAencriptar);
            result = Convert.ToBase64String(encryted);
            return result;*/

            byte[] encbuff = System.Text.Encoding.UTF8.GetBytes(_cadenaAencriptar);
            
            return Convert.ToBase64String(encbuff);
        }
    }
}
