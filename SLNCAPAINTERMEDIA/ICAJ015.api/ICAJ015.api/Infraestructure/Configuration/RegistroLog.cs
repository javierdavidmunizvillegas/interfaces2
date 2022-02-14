using ICAJ015.api.Infraestructure.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ015.api.Infraestructure.Configuration
{
    public class RegistroLog
    {
        public void FileLogger(string Interfaz, string mensajeError)
        {
            Configuracion conf = new Configuracion();
            var configuracion = conf.GetConfiguration();

            string strDate = Convert.ToDateTime(DateTime.Now).ToString("yyyyMMdd");
            DateTime vlFechaHoraActualIngreso = DateTime.Now;
            int vlHoraActualIngreso = vlFechaHoraActualIngreso.Hour;


            string NombreCadena = "LOG" + "_" + Interfaz + "_" + strDate + "_Hora_" + vlHoraActualIngreso + "00 ";



            string cadena = configuracion.GetSection("Data").GetSection("RutaLog").Value.ToString() + "" + NombreCadena + @".log";


            if (!File.Exists(cadena))
            {
                GenerarTXT(cadena, mensajeError);
            }
            else
            {
                DateTime vlUltimaFechaIngreso = File.GetLastWriteTime(cadena);
                int vlUltimaHoraiIngreso = vlUltimaFechaIngreso.Hour;

                DateTime vlFechaHoraActual = DateTime.Now;

                int vlHoraActual = vlFechaHoraActual.Hour;

                if (vlHoraActual == vlUltimaHoraiIngreso)
                {
                    GenerarTXT(cadena, mensajeError);

                }
                else
                {
                    GenerarTXT(cadena, mensajeError);
                }
            }

        }

        private static void GenerarTXT(string cadena, string mensajeError)
        {



            using (StreamWriter mylogs = File.AppendText(cadena))
            {
                DateTime dateTime = new DateTime();
                dateTime = DateTime.Now;
                string vlfecha = DateTime.Now.ToString("yyyy-MM-dd T HH:mm:ssss");

                string texto = vlfecha + " || " + mensajeError;


                mylogs.WriteLine(texto);
                mylogs.Close();
                mylogs.Dispose();

            }
        }
    }
}
