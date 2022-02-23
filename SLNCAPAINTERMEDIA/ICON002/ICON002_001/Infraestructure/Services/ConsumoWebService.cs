using ICON002.Infraestructure.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace ICON002.Infraestructure.Services
{
    public interface IConsumoWebService<T>
     
    {
        Task<T> PostWebService(string uri, string method, string jsonData, int segundos, int intentos);

        Task<T> GetWebService(string uri, string method, string urlRequest);
    }
    public class ConsumoWebService<T> : IConsumoWebService<T>
    {
        private static RegistroLog Logger = new RegistroLog();
        private RestClient client;
        public async Task<T> PostWebService(string uri, string method, string jsonData, int segundos, int intentos)
        {

            try
            {
                T resultadoApi = default(T);
                int contador = 0;

                client = new RestClient(uri);

                var request = new RestRequest(method, Method.POST, DataFormat.Json);

                request.AddParameter("application/json", jsonData, ParameterType.RequestBody);

                //medir tiempo transcurrido en ws
                long start = 0, end = 0;
                double diff = 0;
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;

                start = Stopwatch.GetTimestamp();

                while (contador < intentos)
                {

                    var response = client.Post(request);

                    //if (response.StatusCode == HttpStatusCode.OK)
                    //{
                    //    var content = response.Content;
                    //    resultadoApi = JsonConvert.DeserializeObject<T>(content);
                    //    break;
                    //}
                    //else
                    //{
                    //    var content = response.Content;
                    //    resultadoApi = JsonConvert.DeserializeObject<T>(content);
                    //    await Task.Delay(segundos * SegAMiliS);
                    //}


                  
                    if (response == null)
                        throw new Exception("Respuesta Nulo");

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var content = response.Content;
                        resultadoApi = JsonConvert.DeserializeObject<T>(content);
                        break;
                    }
                    else
                    {
                        if (!(string.IsNullOrEmpty(response.ErrorMessage) || string.IsNullOrWhiteSpace(response.ErrorMessage)))
                            throw new Exception(typeof(ConsumoWebService<T>).ToString() + response.ErrorMessage);

                    }

                    contador++;
                }

                end = Stopwatch.GetTimestamp();
                diff = start > 0 ? (end - start) / NanoAMiliS : 0;
                Logger.FileLogger("APICON002001", "RESPONSE WS: Número de  intentos realizados : " + Convert.ToString(contador) + $", Tiempo Transcurrido : {diff} ms.");


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
        public async Task<T> GetWebService(string uri, string method, string urlRequest)
        {
            try
            {
                T resultadoApi = default(T);
                client = new RestClient(uri);


                string finalPath = $"{method}/{urlRequest}";
                var request = new RestRequest(finalPath, Method.GET);
                ;
                var response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = response.Content;
                    resultadoApi = JsonConvert.DeserializeObject<T>(content);
                }
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
