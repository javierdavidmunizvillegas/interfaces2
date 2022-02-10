using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IVTA014.Infraestructura.Configuracion
{
    class RegistroLog
    {
        private static RegistroLog Logger = new RegistroLog();
        public void FileLogger(string Interfaz, string mensajeError)
        {
            Configuracion conf = new Configuracion();
            var configuracion = conf.GetConfiguration();

            string strDate = Convert.ToDateTime(DateTime.Now).ToString("yyyyMMdd");
            DateTime vlFechaHoraActualIngreso = DateTime.Now;
            int vlHoraActualIngreso = vlFechaHoraActualIngreso.Hour;
            string NombreCadena = "LOG" + "_" + Interfaz + "_" + strDate + "_Hora_" + vlHoraActualIngreso + "00 ";
            string cadena = Environment.GetEnvironmentVariable("RutaLog") + "" + NombreCadena + @".log";  //configuracion.GetSection("Data").GetSection("RutaLog").Value.ToString() + "" + NombreCadena + @".log";
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
        public void FileLogger2(string Interfaz, string mensajeError)
        {
            Configuracion conf = new Configuracion();
            var configuracion = conf.GetConfiguration();

            string strDate = Convert.ToDateTime(DateTime.Now).ToString("yyyyMMdd");
            DateTime vlFechaHoraActualIngreso = DateTime.Now;
            int vlHoraActualIngreso = vlFechaHoraActualIngreso.Hour;
            string NombreCadena = "LOG" + "_" + Interfaz + "_" + strDate + "_Hora_" + vlHoraActualIngreso + "00 ";

            //          string cadena = configuracion.GetSection("Data").GetSection("RutaLog").Value.ToString() + "" + NombreCadena + @".log";
            string cadena = Environment.GetEnvironmentVariable("RutaLog") + "" + NombreCadena + @".log";  //configuracion.GetSection("Data").GetSection("RutaLog").Value.ToString() + "" + NombreCadena + @".log";

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
