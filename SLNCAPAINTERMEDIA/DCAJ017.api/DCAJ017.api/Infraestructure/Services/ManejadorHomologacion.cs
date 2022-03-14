/*
 Objetivo: Contiene los métodos get y post para consumir web services de homologación de crecos a dynamics o de dynamics a crecos
 Archivo: ManejadorHomologacion.cs
 Versión: 1.0
 Creación: 07/03/2022
 Autor: Solange Moncada
*/

using DCAJ017.api.Infraestructure.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace DCAJ017.api.Infraestructura.Servicios
{
    public interface IManejadorHomologacion<T>
    {    
        Task<T> GetHomologacion(string uri, string method, string urlRequest, int segundos, int intentos, string nombre);
    }
    public class ManejadorHomologacion<T> : IManejadorHomologacion<T>
    {
        /*Objeto de la clase RegistroLog*/
        private static RegistroLog Logger = new RegistroLog();
        /*Variable de la clase RestSharp*/
        private RestClient client;

        /*
          Descripción: Este método recibe una url de web services de Crecos para realizar la conversión del código de empresa para ser enviada a Dynamics
          Parámetros de entrada: uri(url principal), method(método de web services), urlRequest(Valor a homologar), segundos, intentos, nombre(nombre de la interfaz)
          Parámetros de salida: No aplica
          Último cambio: 08/03/2022
          Autor de último cambio: Solange Moncada
        */
        public async Task<T> GetHomologacion(string uri, string method, string urlRequest, int segundos, int intentos, string nombre)
        {
            try
            {
                T resultadoApi = default(T);
                client = new RestClient(uri);

                /*Variable para contador*/
                int contador = 0;
                /*Variable para la url final*/
                string finalPath = $"{method}/{urlRequest}";
                var request = new RestRequest(finalPath, Method.GET);

                /*medir tiempo transcurrido en ws*/
                /*Variable para inicio y fin del proceso*/
                long start = 0, end = 0;
                /*Variable restar el fin con el inicio*/
                double diff = 0;
                /*Constante para Segundos*/
                const int SegAMiliS = 1000;
                /*Constante para Milisegundos*/
                const int NanoAMiliS = 10000;

                /*Inicio del conteo*/
                start = Stopwatch.GetTimestamp();

                while (contador < intentos)
                {
                    /*Respuesta recibida del web services de homologación*/
                    var response = client.Execute(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var content = response.Content;
                        /*Respuesta deserializada*/
                        resultadoApi = JsonConvert.DeserializeObject<T>(content);
                        /*Si se recibió respuesta sale*/
                        break;
                    }
                    else
                    {
                        /*Si no responde sigo hasta que se terminen los intentos y tiempo configurados*/
                        await Task.Delay(segundos * SegAMiliS);
                    }

                    contador++;

                }

                /*Fin del conteo*/
                end = Stopwatch.GetTimestamp();
                /*Resta del fin menos el inicio*/
                diff = start > 0 ? (end - start) / NanoAMiliS : 0;
                Logger.FileLogger(nombre, "WS HOMOLOGACION : Número de  intentos realizados : " + Convert.ToString(contador) + $", Tiempo Transcurrido : {diff} ms.");

                client = null;
                request = null;
                return resultadoApi;
            }
            catch (Exception)
            {
                client = null;
                throw;
            }
        }
    }

}
