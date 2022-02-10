using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CONTICREDENVIO.Servicios
{

    public interface IContLOG
    {
        Task FileLogger(string Interfaz, string mensajeError);
    }
    public class ContLOG : IContLOG
    {
        private readonly IConfigurationRoot _config;

        public ContLOG(IConfigurationRoot config)
        {
            _config = config;

        }

        public async Task FileLogger(string Interfaz, string mensajeError)
        {

            string strDate = Convert.ToDateTime(DateTime.Now).ToString("yyyyMMdd");
            DateTime vlFechaHoraActualIngreso = DateTime.Now;
            int vlHoraActualIngreso = vlFechaHoraActualIngreso.Hour;
            string NombreCadena = "LOG" + "_" + Interfaz + "_" + strDate + "_Hora_" + vlHoraActualIngreso + "00 ";
            string cadena = _config.GetSection("RutaLog").Value + "" + NombreCadena + @".log"; 
            //string cadena = Environment.GetEnvironmentVariable("RutaLog") + "" + NombreCadena + @".log";  //configuracion.GetSection("Data").GetSection("RutaLog").Value.ToString() + "" + NombreCadena + @".log";
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
