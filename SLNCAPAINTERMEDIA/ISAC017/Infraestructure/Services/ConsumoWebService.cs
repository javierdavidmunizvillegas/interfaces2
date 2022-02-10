﻿using ISAC017.Infraestructure.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace IVTA017.Infraestructure.Services
{
    public interface IConsumoWebService<T>
     
    {
        T PostWebService(string uri, string method, string jsonData);

        Task<T> GetWebService(string uri, string method, string urlRequest);
        Task<T> GetHomologacion(string uri, string method, string urlRequest);
    }
    public class ConsumoWebService<T> : IConsumoWebService<T>
    {
        private static RegistroLog Logger = new RegistroLog();
        private RestClient client;

        public T PostWebService(string uri, string method, string jsonData)
        {

            try
            {
                client = new RestClient(uri);

                var request = new RestRequest(method, Method.POST, DataFormat.Json);

                request.AddParameter("application/json", jsonData, ParameterType.RequestBody);
                var response = client.Post(request);
                // var content = response.Content;
                // var resultadoApi = JsonConvert.DeserializeObject<T>(content);
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
                        throw new Exception(typeof(ConsumoWebService<T>).ToString() + response.ErrorMessage);

                }

                client = null;
                request = null;
                return resultadoApi;
            }
            catch (Exception e)
            {
                client = null;
                throw e;
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
                        throw new Exception(typeof(ConsumoWebService<T>).ToString() + response.ErrorMessage);

                }
                client = null;
                request = null;
                return resultadoApi;
            }
            catch (Exception e)
            {
                client = null;
                throw e;
            }
        }
        public async Task<T> GetHomologacion(string uri, string method, string urlRequest)
        {
            T resultadoApi = default(T);
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
                resultadoApi = JsonConvert.DeserializeObject<T>(content);
            }
            else
            {
                if (!(string.IsNullOrEmpty(response.ErrorMessage) || string.IsNullOrWhiteSpace(response.ErrorMessage)))
                    throw new Exception(typeof(ConsumoWebService<T>).ToString() + response.ErrorMessage);

            }
            client = null;
            request = null;
            return resultadoApi;
        }
    }
}
