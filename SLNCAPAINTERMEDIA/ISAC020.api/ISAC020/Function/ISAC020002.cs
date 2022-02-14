using System;
using System.IO;
using System.Threading.Tasks;
using ISAC020.Infraestructure;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using ISAC020.Models.Request;
using ISAC020.Models;
using ISAC020.Infraestructure.Configuration;
using Interface.Api.Infraestructura.Configuracion;
using ISAC020.Infraestructure.Services;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ISAC020
{
    public static class APISAC020002
    {
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService");
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static int vl_Attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        [FunctionName("APISAC020002")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")] string myQueueItem, ILogger log)
        {

            try
            {
                
                Logger.FileLogger("APISAC020002", "Procesando Función");               

                var APISAC020002Request = JsonConvert.DeserializeObject<APISAC020002MessageRequest>(myQueueItem);
                APISAC020002Request.Enviroment = vl_Environment;

                string jsonData = JsonConvert.SerializeObject(APISAC020002Request);

                Logger.FileLogger("APISAC020002", "REQUEST RECIBIDO: " + jsonData);

                ConsumoWebService<ResponseWS> cws = new ConsumoWebService<ResponseWS>();
                ResponseWS objResponse = await cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData, vl_Time, vl_Attempts);

                if (objResponse==null)
                {
                  
                    Logger.FileLogger("APISAC020002", "WS LEGADO: No se retorno resultado de WS Requisición");

                }
                else
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);
                   
                    Logger.FileLogger("APISAC020002", "RESULTADO RECIBIDO WS: "+ jsonrequest);
                   
                }
              
                
            }
            catch (Exception ex)
            {
                Logger.FileLogger("APISAC020002", "ERROR: " + ex.ToString());
            }

        }
    }
}
