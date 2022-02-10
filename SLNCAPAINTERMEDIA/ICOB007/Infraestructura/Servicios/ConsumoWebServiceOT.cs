using ICOB007.Infraestructura.Configuracion;
using ICOB007.Models._001.Response;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;

namespace ICOB007.Infraestructura.Servicios
{
    public interface IConsumoWebServiceOT

    {
        RespuestaWSOT PostWebService(string uri, string method, string jsonData);

        Task<RespuestaWSOT> GetWebService(string uri, string method, string urlRequest);
    }
    public class ConsumoWebServiceOT : IConsumoWebServiceOT
    {
        private static RegistroLog Logger = new RegistroLog();
        private RestClient client;
        public RespuestaWSOT PostWebService(string uri, string method, string jsonData)
        {
           

            client = new RestClient(uri);

            var request = new RestRequest(method, Method.POST, DataFormat.Json);

            request.AddParameter("application/json", jsonData, ParameterType.RequestBody);
            var response = client.Post(request);

            RespuestaWSOT resultadoApi = new RespuestaWSOT();
            RespuestaWS001 responseInterna = null;
            if (response.Content == null)
                throw new Exception("Respuesta Nulo");

            if (response.StatusCode == HttpStatusCode.OK)
            {
               
                var content = response.Content;
                resultadoApi.NumberOT = JsonConvert.DeserializeObject<string>(content);
               
                /*  if (responseInterna == null)
                  {
                      resultadoApi.NumberOT = content;
                  }
                  else
                  {
                      responseInterna = JsonConvert.DeserializeObject<RespuestaWS001>(content);
                      resultadoApi.Respuesta = responseInterna;
                  }*/

            }
            else
            {
                
                if (!(string.IsNullOrEmpty(response.ErrorMessage) || string.IsNullOrWhiteSpace(response.ErrorMessage)))
                    throw new Exception(typeof(ConsumoWebService<RespuestaWSOT>).ToString() + response.ErrorMessage);
                else
                {
                    var content = response.Content;
                    responseInterna = JsonConvert.DeserializeObject<RespuestaWS001>(content);
                 /*   if (responseInterna == null)
                    {
                        resultadoApi.NumberOT = content;
                    }
                    else
                    {*/
                       
                     resultadoApi.Respuesta = responseInterna;
                   // }
                }

            }

            client = null;
            request = null;
            return resultadoApi;

        }
        public async Task<RespuestaWSOT> GetWebService(string uri, string method, string urlRequest)
        {

            RespuestaWSOT resultadoApi = null;
            client = new RestClient(uri);


            string finalPath = $"{method}/{urlRequest}";
            var request = new RestRequest(finalPath, Method.GET);
            ;
            var response = client.Execute(request);
            if (response == null)
                throw new Exception("Respuesta Nulo");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                resultadoApi = JsonConvert.DeserializeObject<RespuestaWSOT>(content);
            }
            else
            {
                if (!(string.IsNullOrEmpty(response.ErrorMessage) || string.IsNullOrWhiteSpace(response.ErrorMessage)))
                    throw new Exception(typeof(ConsumoWebService<RespuestaWSOT>).ToString() + response.ErrorMessage);

            }
            client = null;
            request = null;
            return resultadoApi;

        }
        public async Task<RespuestaWSOT> GetHomologacion(string uri, string method, string urlRequest)
        {
            RespuestaWSOT resultadoApi = new RespuestaWSOT();
            client = new RestClient(uri);


            string finalPath = $"{method}/{urlRequest}";
            var request = new RestRequest(finalPath, Method.GET);
            ;
            var response = client.Execute(request);
            if (response == null)
                throw new Exception("Respuesta Nulo");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                resultadoApi = JsonConvert.DeserializeObject<RespuestaWSOT>(content);
            }
            else
            {
                if (!(string.IsNullOrEmpty(response.ErrorMessage) || string.IsNullOrWhiteSpace(response.ErrorMessage)))
                    throw new Exception(typeof(ConsumoWebService<RespuestaWSOT>).ToString() + response.ErrorMessage);

            }
            client = null;
            request = null;
            return resultadoApi;
        }
    }
}
