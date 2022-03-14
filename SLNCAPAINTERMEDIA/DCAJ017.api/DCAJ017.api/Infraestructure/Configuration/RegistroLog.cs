/*
 Objetivo: Registro del log.
 Archivo: RegistroLog.cs
 Versión: 1.0
 Creación: 08/03/2022
 Autor: Solange Moncada
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ017.api.Infraestructure.Configuration
{
    public class RegistroLog
    {
        /*
         Descripción: Este método es llamado cada vez que se desea registrar en el log. Crea el archivo
         Parámetros de entrada: Interfaz (nombre de la interfaz para escribirla en el log)
         Parámetros de salida: mensajeError
         Último cambio: 08/03/2022
         Autor de último cambio: Solange Moncada
       */
        public void FileLogger(string Interfaz, string mensajeError)
        {
            Configuracion conf = new Configuracion();
            var configuracion = conf.GetConfiguration();

            string strDate = Convert.ToDateTime(DateTime.Now).ToString("yyyyMMdd");
            DateTime vlFechaHoraActualIngreso = DateTime.Now;
            int vlHoraActualIngreso = vlFechaHoraActualIngreso.Hour;

            string NombreCadena = "LOG" + "_" + Interfaz + "_" + strDate + "_Hora_" + vlHoraActualIngreso + "00 ";

            string cadena = configuracion.GetSection("Data").GetSection("RutaLog").Value.ToString() + "" + NombreCadena + @".log";
            try
            {

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
            catch (Exception ex)
            {
                FileLogger2(Interfaz, mensajeError);
            }

        }

        /*
        Descripción: Este método es llamado cada vez que existe una excepcion en el método FileLogger.
        Parámetros de entrada: Interfaz (nombre de la interfaz para escribirla en el log)
        Parámetros de salida: mensajeError
        Último cambio: 08/03/2022
        Autor de último cambio: Solange Moncada
       */
        public void FileLogger2(string Interfaz, string mensajeError)
        {
            Configuracion conf = new Configuracion();
            var configuracion = conf.GetConfiguration();

            string strDate = Convert.ToDateTime(DateTime.Now).ToString("yyyyMMdd");
            DateTime vlFechaHoraActualIngreso = DateTime.Now;
            int vlHoraActualIngreso = vlFechaHoraActualIngreso.Hour;


            string NombreCadena = "LOG" + "_" + Interfaz + "_" + strDate + "_Hora_" + vlHoraActualIngreso + "00 ";

            string cadena = configuracion.GetSection("Data").GetSection("RutaLog").Value.ToString() + "" + NombreCadena + @".log";
            try
            {

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
            catch (Exception ex)
            {
                FileLogger(Interfaz, mensajeError);
            }

        }

        /*
        Descripción: Este método es utilizado para registrar el texto en el archivo de log.
        Parámetros de entrada: cadena (cadena que se registrará en el archivo)
        Parámetros de salida: mensajeError
        Último cambio: 08/03/2022
        Autor de último cambio: Solange Moncada
       */
        private static void GenerarTXT(string cadena , string mensajeError)
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
