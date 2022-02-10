using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;
using ISAC018.Models._001.Resquest;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

using ISAC018.Models._001.Response;
using NSwag.Annotations;
using System.Net.Http;
using Microsoft.Azure.WebJobs.Extensions.Http;
using ISAC018.Infraestructura.Configuracion;
using ISAC018.Infraestructura.Servicios;


namespace ISAC018
{
    public static class APISAC018001
    {
      
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService");
        //public static string sbConenctionStringReceptar = Environment.GetEnvironmentVariable("ConectionStringRequest001");
        //private static string nombrecolarequest = Environment.GetEnvironmentVariable("QueueRequest001");
        //private static string nombrecolaresponse = Environment.GetEnvironmentVariable("QueueResponse001");
        //private static string sbConenctionStringEnvio = Environment.GetEnvironmentVariable("ConectionStringResponse001");
        private static string sbUriConsumoWebServiceHomologa = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriConsumowebServiceHomologaAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string Entorno = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static int vl_IntentosWS = Convert.ToInt32(Environment.GetEnvironmentVariable("IntentosWS01"));
        private static int vl_TimeWS = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleepWS"));
        private static RegistroLog Logger = new RegistroLog();
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();


        [FunctionName("APISAC018001")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")] string myQueueItem, ILogger log)
        {
        
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                Logger.FileLogger("APISAC018001","FUNCION : Request Recibido: "+ myQueueItem);
                var APISAC018001Request = JsonConvert.DeserializeObject<APISAC018001MessageRequest>(myQueueItem);
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;
                long start = 0, end = 0;
                double diff = 0;
                RespuestaWS objResponse = new RespuestaWS();
                string respuestaLog = null;
                

                // asignar campo ambiente
                APISAC018001Request.Enviroment = Entorno; //para mapear si es necesario
                APISAC018001Request.custAccount = CodigoCliente.DynamicAcrecos(APISAC018001Request.custAccount, "APISAC018001"); //para mapear si es necesario


                //////////
                // ejecuta web service
                ////////
                ///
                string jsonData = JsonConvert.SerializeObject(APISAC018001Request);
                ConsumoWebService<RespuestaWS> cws = new ConsumoWebService<RespuestaWS>();


                start = Stopwatch.GetTimestamp();
                int cont = 0;
                for (int i = 1; i <= vl_IntentosWS; i++)
                {
                    try
                    {
                        cont = i;
                        objResponse = cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData);
                        if (objResponse != null && (string.IsNullOrEmpty(objResponse.codigoTransaccion) || string.IsNullOrWhiteSpace(objResponse.codigoTransaccion)))
                        {
                            break;
                        }
                        if (objResponse != null && "OK".Equals(objResponse.estadoTransaccion))
                        {
                            break;
                        }

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    await Task.Delay(vl_TimeWS);
                } // fin for
                end = Stopwatch.GetTimestamp();
                diff = start > 0 ? (end - start) / NanoAMiliS : 0;
                Logger.FileLogger("APISAC018001", $"FUNCION WS Registra Nota de Crédito : Número de intentos realizados : {cont} En Ws Registra Nota de Crédito,Tiempo Transcurrido : {diff} ms.");

                if (objResponse == null)
                {
                    Logger.FileLogger("APISAC018001", $"FUNCION WS Registra Nota de Crédito Error en el Servicio de Registra Nota de Crédito");
                   
                }
                else
                {
                    
                    respuestaLog = JsonConvert.SerializeObject(objResponse);
                    Logger.FileLogger("APISAC018001", "Responde WS Registra Nota de Crédito: " +respuestaLog);
                }

   

            }
            catch (Exception ex)
            {
                Logger.FileLogger("APISAC018001","FUNCION : Error por Excepcion: " + ex.ToString());
                log.LogError($"Exception APISAC018001: {ex.Message}");

            }

    }
        }
}
