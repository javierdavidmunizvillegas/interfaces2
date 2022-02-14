﻿using Interface.Api.Infraestructura.Configuracion;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace ICXP003.Infraestructura.Servicios
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
                client = new RestClient(uri);
                int contador = 0;


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

                    var response = client.Execute(request);

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
                Logger.FileLogger("APICXP003003", "WS FUNCION: Número de  intentos realizados : " + Convert.ToString(contador) + $", Tiempo Transcurrido : {diff} ms.");

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
