using ICAJ005.api.Infraestructure.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace ICAJ005.api.Infraestructura.Servicios
{
    public interface IManejadorHomologacion<T>
    {
        T PostHomologacion(string uri, string method, string jsonData);

        Task<T> GetHomologacion(string uri, string method, string urlRequest);
    }
    public class ManejadorHomologacion<T> : IManejadorHomologacion<T>
    {
        private static RegistroLog Logger = new RegistroLog();
        private RestClient client;
        public T PostHomologacion(string uri, string method, string jsonData)
        {
            client = new RestClient(uri);

            var request = new RestRequest(method, Method.POST, DataFormat.Json);

            request.AddParameter("application/json", jsonData, ParameterType.RequestBody);
            var response = client.Post(request);
            //var content = response.Content;
            //var resultadoApi = JsonConvert.DeserializeObject<T>(content); OJO

            T resultadoApi = default(T);

            if (response == null)
                throw new Exception("Respuesta Nulo");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                resultadoApi = JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                if (!(string.IsNullOrEmpty(response.ErrorMessage) || string.IsNullOrWhiteSpace(response.ErrorMessage)))
                    throw new Exception(typeof(ManejadorHomologacion<T>).ToString() + response.ErrorMessage);

            }
            client = null;
            request = null;
            return resultadoApi;
        }
        public async Task<T> GetHomologacion(string uri, string method, string urlRequest)
        {
            T resultadoApi = default(T);
            client = new RestClient(uri);


            string finalPath = $"{method}/{urlRequest}";
            var request = new RestRequest(finalPath, Method.GET);
            ;
            var response = await client.ExecuteAsync(request);
            if (response == null)
                throw new Exception("Respuesta Nulo");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                resultadoApi = JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                if (!(string.IsNullOrEmpty(response.ErrorMessage) || string.IsNullOrWhiteSpace(response.ErrorMessage)))
                    throw new Exception(typeof(ManejadorHomologacion<T>).ToString() + response.ErrorMessage);

            }
            client = null;
            request = null;
            return resultadoApi;
        }
    }

}
