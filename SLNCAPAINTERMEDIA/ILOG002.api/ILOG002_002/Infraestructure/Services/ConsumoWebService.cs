using ILOG002_002.Infraestructure.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace ILOG002_002.Infraestructure.Services
{
    public interface IConsumoWebService<T>

    {
        Task<T> PostWebService(string uri, string method, string jsonData, int segundos, int intentos, string vl_varAutorizacion, string vl_valAutorizacion);

        Task<T> PatchWebService(string uri, string method, string jsonData, int segundos, int intentos, string token, string vl_varAutorizacion, string vl_valAutorizacion, string vl_valToken);

        Task<T> GetWebService(string uri, string method, string urlRequest);
    }
    public class ConsumoWebService<T> : IConsumoWebService<T>
    {
        private static RegistroLog Logger = new RegistroLog();
        private RestClient client;
        public async Task<T> PostWebService(string uri, string method, string jsonData, int segundos, int intentos,string vl_varAutorizacion, string vl_valAutorizacion)
        {

            try
            {
                T resultadoApi = default(T);
                int contador = 0;

                client = new RestClient(uri);

                var request = new RestRequest(method, Method.POST, DataFormat.Json);

                request.AddParameter("application/json", jsonData, ParameterType.RequestBody);
                request.AddHeader(vl_varAutorizacion, vl_valAutorizacion);
               
                //medir tiempo transcurrido en ws
                long start = 0, end = 0;
                double diff = 0;
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;

                start = Stopwatch.GetTimestamp();

                while (contador < intentos)
                {

                    var response = client.Post(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var content = response.Content;
                        resultadoApi = JsonConvert.DeserializeObject<T>(content);
                        break;
                    }
                    else
                    {
                        var content = response.Content;
                        resultadoApi = JsonConvert.DeserializeObject<T>(content);
                        await Task.Delay(segundos * SegAMiliS);
                    }
                    contador++;
                }

                end = Stopwatch.GetTimestamp();
                diff = start > 0 ? (end - start) / NanoAMiliS : 0;
                Logger.FileLogger("APILOG002002", "RESPONSE WS: Número de  intentos realizados : " + Convert.ToString(contador) + $", Tiempo Transcurrido : {diff} ms.");


                client = null;
                request = null;
                return resultadoApi;
            }
            catch (Exception ex)
            {
                client = null;
                throw;
            }
        }

        public async Task<T> PatchWebService(string uri, string method, string jsonData, int segundos, int intentos, string token, string vl_varAutorizacion, string vl_valAutorizacion, string vl_valToken)
        {

            try
            {
                T resultadoApi = default(T);
                int contador = 0;

                client = new RestClient(uri);

                var request = new RestRequest(method, Method.PATCH, DataFormat.Json);

                request.AddParameter("application/json", jsonData, ParameterType.RequestBody);
                request.AddHeader(vl_varAutorizacion, vl_valAutorizacion);
                if (token != null)
                {
                      request.AddHeader(vl_valToken, token);
                }              

                //medir tiempo transcurrido en ws
                long start = 0, end = 0;
                double diff = 0;
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;

                start = Stopwatch.GetTimestamp();

                while (contador < intentos)
                {

                    var response = client.Patch(request);

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var content = response.Content;
                        resultadoApi = JsonConvert.DeserializeObject<T>(content);
                        break;
                    }
                    else
                    {
                        var content = response.Content;
                        resultadoApi = JsonConvert.DeserializeObject<T>(content);
                        await Task.Delay(segundos * SegAMiliS);
                    }
                    contador++;
                }

                end = Stopwatch.GetTimestamp();
                diff = start > 0 ? (end - start) / NanoAMiliS : 0;
                Logger.FileLogger("APILOG002002", "RESPONSE WS: Número de  intentos realizados : " + Convert.ToString(contador) + $", Tiempo Transcurrido : {diff} ms.");


                client = null;
                request = null;
                return resultadoApi;
            }
            catch (Exception ex)
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
