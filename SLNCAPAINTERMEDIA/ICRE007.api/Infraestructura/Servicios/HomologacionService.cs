using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ICRE007.api.Infraestructura.Servicios
{
    public interface IHomologacionService<T>
    {
        T PostHomologacion(string uri, string method, string jsonData);

        Task<T> GetHomologacion(string uri, string method, string urlRequest);
    }
    public class HomologacionService<T> : IHomologacionService<T>
    {
        private RestClient client;
        public T PostHomologacion(string uri, string method, string jsonData)
        {
            //try
            //{
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
                        throw new Exception(typeof(HomologacionService<T>).ToString() + response.ErrorMessage);

                }
                client = null;
                request = null;
                return resultadoApi;
            //}
            //catch (Exception e)
            //{
            //    client = null;
            //    throw e;
            //}
        }
        public async Task<T> GetHomologacion(string uri, string method, string urlRequest)
        {
            //try
            //{
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
                    throw new Exception(typeof(HomologacionService<T>).ToString() + response.ErrorMessage);
                    
            }
            client = null;
            request = null;
            return resultadoApi;
            //}
            //catch (Exception e)
            //{
            //    client = null;
            //    throw e;
            //}
        }
    }
}
