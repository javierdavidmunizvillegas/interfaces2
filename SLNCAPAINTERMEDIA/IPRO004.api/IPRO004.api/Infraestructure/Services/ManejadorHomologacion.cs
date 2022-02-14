using IPRO004.api.Infraestructure.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace IPRO004.api.Infraestructura.Servicios
{
    public interface IManejadorHomologacion<T>
    {
        T PostHomologacion(string uri, string method, string jsonData);

        Task<T> GetHomologacion(string uri, string method, string urlRequest, int segundos, int intentos, string nombre);
    }
    public class ManejadorHomologacion<T> : IManejadorHomologacion<T>
    {
        private static RegistroLog Logger = new RegistroLog();
        private RestClient client;
        public T PostHomologacion(string uri, string method, string jsonData)
        {
            try
            {
                client = new RestClient(uri);

                var request = new RestRequest(method, Method.POST, DataFormat.Json);

                request.AddParameter("application/json", JsonConvert.SerializeObject(jsonData), ParameterType.RequestBody);
                var response = client.Post(request);
                var content = response.Content;

                var resultadoApi = JsonConvert.DeserializeObject<T>(content);

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
        public async Task<T> GetHomologacion(string uri, string method, string urlRequest, int segundos, int intentos, string nombre)
        {
            try
            {
                T resultadoApi = default(T);
                client = new RestClient(uri);
                int contador = 0;

                string finalPath = $"{method}/{urlRequest}";
                var request = new RestRequest(finalPath, Method.GET);

                //medir tiempo transcurrido en ws
                long start = 0, end = 0;
                double diff = 0;
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;

                start = Stopwatch.GetTimestamp();

                while (contador < intentos)
                {

                    var response = client.Execute(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var content = response.Content;
                        resultadoApi = JsonConvert.DeserializeObject<T>(content);
                        break;
                    }
                    else
                    {
                        await Task.Delay(segundos * SegAMiliS);
                    }

                    contador++;

                }

                end = Stopwatch.GetTimestamp();
                diff = start > 0 ? (end - start) / NanoAMiliS : 0;
                Logger.FileLogger(nombre, "WS HOMOLOGACIÓN : Número de  intentos realizados : " + Convert.ToString(contador) + $", Tiempo Transcurrido : {diff} ms.");

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
