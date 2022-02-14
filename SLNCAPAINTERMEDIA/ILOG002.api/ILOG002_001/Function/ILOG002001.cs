using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using ILOG002_001.Infraestructure.Configuration;
using ILOG002_001.Infraestructure.Services;
using ILOG002_001.Models;
using ILOG002_001.Models._001.Request;
using ILOG002_001.Models._001.Response;
using ILOG002_001.Models.Homologacion.ResponseHomologacion;
using Interface.Api.Infraestructura.Configuracion;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static ILOG002_001.Services.ManejadorRequest;

namespace ILOG002_001
{
    public static class ILOG002001
    {
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService001");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService001");
        private static string sbUriHomologacionDynamic = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriSiac = Environment.GetEnvironmentVariable("MetodoWsUriSiac");
        private static string sbMetodoWsUriAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string sbMetodoWsUriAxBodegas = Environment.GetEnvironmentVariable("MetodoWsUriAxBodegas");
        private static string sbQueueResponse = Environment.GetEnvironmentVariable("QueueResponse001");
        private static string sbConectionStringSend = Environment.GetEnvironmentVariable("ConectionStringResponse");
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static int vl_Attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static string vl_varAutorizacion = Environment.GetEnvironmentVariable("ParAuthorization");
        private static string vl_valAutorizacion = Environment.GetEnvironmentVariable("ValAuthorization");

        [FunctionName("APILOG002001")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest")] string myQueueItem, ILogger log)
        {//FUNCIONA 

            try
            {
                Logger.FileLogger("APILOG002001", "Procesando Función");


                ResponseCreateShipify objResponse = null;
                ResponseHomologacion respuesta = null;
                ResponseHomologacion respuesta2 = null;
                ResponseHomologacion respuesta3 = null;

                var APILOGO002001Request = JsonConvert.DeserializeObject<CreateDeliveryShipify>(myQueueItem);
                APILOGO002001Request.Enviroment = vl_Environment;

                string jsonData = JsonConvert.SerializeObject(APILOGO002001Request);
                Logger.FileLogger("APILOG002001", "REQUEST RECIBIDO DE DYNAMICS: " + jsonData);


                IManejadorHomologacion<ResponseHomologacion> _manejadorHomologacion = new ManejadorHomologacion<ResponseHomologacion>();
                respuesta = await _manejadorHomologacion.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriAx, APILOGO002001Request.DataAreaId, vl_Time, vl_Attempts);

                APILOGO002001Request.DataAreaId = respuesta.Response;

                if (APILOGO002001Request.deliveries.Count > 0)
                {
                    int cant = APILOGO002001Request.deliveries.Count;
                    int i = 0;

                    if (cant > 0)
                    {
                        do
                        {
                            if (APILOGO002001Request.deliveries[i].pickup != null)
                            {
                                if (APILOGO002001Request.deliveries[i].pickup.location != null)
                                {
                                    if (APILOGO002001Request.deliveries[i].pickup.location.warehouse != null && APILOGO002001Request.deliveries[i].pickup.location.warehouse != "")
                                    {
                                        IManejadorHomologacion<ResponseHomologacion> _manejadorHomologacion2 = new ManejadorHomologacion<ResponseHomologacion>();
                                        respuesta2 = await _manejadorHomologacion2.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriAxBodegas, APILOGO002001Request.deliveries[i].pickup.location.warehouse, vl_Time, vl_Attempts);

                                        APILOGO002001Request.deliveries[i].pickup.location.warehouse = respuesta2.Response;
                                    }
                                }
                            }

                            if (APILOGO002001Request.deliveries[i].dropoff != null)
                            {
                                if (APILOGO002001Request.deliveries[i].dropoff.location != null)
                                {
                                    if (APILOGO002001Request.deliveries[i].dropoff.location.warehouse != null && APILOGO002001Request.deliveries[i].dropoff.location.warehouse != "")
                                    {
                                        IManejadorHomologacion<ResponseHomologacion> _manejadorHomologacion3 = new ManejadorHomologacion<ResponseHomologacion>();
                                        respuesta3 = await _manejadorHomologacion3.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriAxBodegas, APILOGO002001Request.deliveries[i].dropoff.location.warehouse, vl_Time, vl_Attempts);

                                        APILOGO002001Request.deliveries[i].dropoff.location.warehouse = respuesta3.Response;
                                    }
                                }
                            }

                            i++;
                        } while (i < cant);
                    }
                   
                }

                jsonData = JsonConvert.SerializeObject(APILOGO002001Request);


                Logger.FileLogger("APILOG002001", "REQUEST ENVIADO A WS: " + jsonData);

                ConsumoWebService<ResponseCreateShipify> cws = new ConsumoWebService<ResponseCreateShipify>();
                objResponse = await cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData, vl_Time, vl_Attempts, vl_varAutorizacion, vl_valAutorizacion);//concluido

                string consultado = JsonConvert.SerializeObject(objResponse);

                if (consultado == "null")
                {
                    Logger.FileLogger("APILOG002001", "WS LEGADO: No se retorno resultado de WS");
                }
                else
                {
                    objResponse.SessionId = APILOGO002001Request.SessionId;
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);                   
                    Logger.FileLogger("APILOG002001", "RESULTADO RECIBIDO WS: " + jsonrequest);

                    IManejadorRequest<ResponseCreateShipify> _manejadorRequestQueue = new ManejadorRequest<ResponseCreateShipify>();

                    await _manejadorRequestQueue.EnviarMensajeAsync(APILOGO002001Request.SessionId, sbConectionStringSend, sbQueueResponse, objResponse);
                    objResponse.SessionId = APILOGO002001Request.SessionId;
                    string mensaje = JsonConvert.SerializeObject(objResponse);
                    Logger.FileLogger("APILOG002001", "RESPONSE ENVIADO A LEGADO: " + mensaje);

                }


            }
            catch (Exception ex)
            {
                Logger.FileLogger("APILOG002001", "ERROR: " + ex.ToString());
            }

        }
    }
}
