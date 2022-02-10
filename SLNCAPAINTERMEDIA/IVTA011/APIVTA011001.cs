using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using IVTA011.Infraestructura.Configuracion;
using Microsoft.Extensions.Configuration;
using IVTA011.Models._001.Request;
using Newtonsoft.Json;

using IVTA011.Infraestructura.Servicios;
using IVTA011.Models._001.Response;
using System.Diagnostics;
using IVTA011.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace IVTA011
{
    public static class APIVTA011001
    {
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService001");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService001");
        public static string sbConenctionStringReceptar = Environment.GetEnvironmentVariable("ConectionStringRequest001");
        private static string nombrecolaresponse = Environment.GetEnvironmentVariable("QueueResponse001");
        private static string sbConenctionStringEnvio = Environment.GetEnvironmentVariable("ConectionStringResponse001");
        private static string sbUriConsumoWebServiceHomologa = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriConsumowebServiceHomologaAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string Entorno = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static int vl_IntentosWS = Convert.ToInt32(Environment.GetEnvironmentVariable("IntentosWS01"));
        private static int vl_TimeWS = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleepWS"));
        private static RegistroLog Logger = new RegistroLog();

        
        [FunctionName("IVTA011001")]
        
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")] string myQueueItem, ILogger log)
        {
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                Logger.FileLogger("APIVTA011001","Request Recibido de Dynamics : "+  myQueueItem);
                var APIVTA011001Request = JsonConvert.DeserializeObject<APIVTA011001MessageRequest>(myQueueItem);
                if (string.IsNullOrEmpty(APIVTA011001Request.SessionId) || string.IsNullOrWhiteSpace(APIVTA011001Request.SessionId))
                {
                    Logger.FileLogger("APIVTA011001", $"FUNCION WS Valida campo: Session null, se asignará vacío");
                    APIVTA011001Request.SessionId = "";
                }

                //Homologacion a Siac
                string DataAreaId = APIVTA011001Request.DataAreaId;
                //medir tiempo transcurrido en ws
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;
                long start = 0, end = 0;
                double diff = 0;
                ResponseHomologa001 ResuldatoHomologa = null;
                string responseHomologa = string.Empty;
                string responseWS001 = string.Empty;
                RespuestaWS001 objResponse = new RespuestaWS001();
                ConsumoWebService<RespuestaWS001> cws = new ConsumoWebService<RespuestaWS001>();
                APIVTA011001MessageResponse APIVTA011001Response = new APIVTA011001MessageResponse();

                ConsumoWebService<ResponseHomologa001> cwsHomologa = new ConsumoWebService<ResponseHomologa001>();
                //sesionid = Guid.NewGuid().ToString(); // hasta mientras, se debe borrar
                string sesionid = APIVTA011001Request.SessionId.ToString();
                start = Stopwatch.GetTimestamp();
                int cont = 0;
                for (int i = 1; i < vl_IntentosWS; i++)
                {
                    try
                    {
                         ResuldatoHomologa = await cwsHomologa.GetHomologacion(sbUriConsumoWebServiceHomologa, sbMetodoWsUriConsumowebServiceHomologaAx, DataAreaId);
                        // Validacion el objeto recibido
                        // Condicion para salir
                        if (ResuldatoHomologa != null && (string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                        {
                            Logger.FileLogger("APIVTA011001", $"FUNCION WS HOMOLOGACION: Empresa No existe");
                            cont = i;
                            break;
                        }
                        if (ResuldatoHomologa != null && (ResuldatoHomologa.DescripcionId != ""))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            APIVTA011001Request.DataAreaId = responseHomologa;
                            Logger.FileLogger("APIVTA011001", $"FUNCION WS HOMOLOGACION: Código de empresa a enviarse a Legado es : {responseHomologa}");
                            cont = i;
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
                Logger.FileLogger("APIVTA011001", $"FUNCION WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa != null && !(string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                {
                    
                        // asignar campo ambiente
                        APIVTA011001Request.Enviroment = Entorno; //para mapear si es necesario
                        // ejecuta api, 
                        ///OJO VALIDAR SESION QUE NO SEA NULO
                        string jsonData = JsonConvert.SerializeObject(APIVTA011001Request);
               

                        start = Stopwatch.GetTimestamp();
                        //int cont = 0;
                        for (int i = 1; i < vl_IntentosWS; i++)
                        {
                            try
                            {
                                objResponse = await cws.GetWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, "");
                                if ((objResponse != null)  && (objResponse.StatusId ))
                                {
                                    cont = i;
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
                        Logger.FileLogger("APIVTA011001", $"FUNCION WS PERIODO: Número de intentos realizados : {cont} En Ws Periodos,Tiempo Transcurrido : {diff} ms.");

                        if (objResponse == null)
                        {
                                Logger.FileLogger("APIVTA011001", $"FUNCION WS PERIODO: Error en el Servicio de Períodos");
                                APIVTA011001Response.PeriodoId = null;
                                APIVTA011001Response.StartDate = null;
                                APIVTA011001Response.EndDate = null;
                    }
                        else
                        {
                                // mapear la clase response del metodo con respuesta del WS
                                APIVTA011001Response.SessionId = sesionid;
                                APIVTA011001Response.PeriodoId = objResponse.PeriodoId;
                                APIVTA011001Response.StartDate = objResponse.StartDate;
                                APIVTA011001Response.EndDate = objResponse.EndDate;
                        }
                }
                else
                {
                    Logger.FileLogger("APIVTA011001", $"FUNCION WS HOMOLOGACION: Error en el Servicio Homologación");
                    APIVTA011001Response.PeriodoId = null;
                    APIVTA011001Response.StartDate = null;
                    APIVTA011001Response.EndDate = null;

                }

                string responseLog = JsonConvert.SerializeObject(objResponse);
                Logger.FileLogger("APIVTA011001", "FUNCION: Resultado en ejecucion WS PERIODO: "+ responseLog);
                ManejadorRequestQueue<APIVTA011001MessageResponse> _manejadorRequestQueue = new ManejadorRequestQueue<APIVTA011001MessageResponse>();
                await _manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolaresponse, APIVTA011001Response);
                responseLog = JsonConvert.SerializeObject(APIVTA011001Response);
                Logger.FileLogger("APIVTA011001", "Response a Dynamics : " + responseLog);
            }
            catch (Exception ex)
            {
                Logger.FileLogger("APIVTA011001", "Error por Exception: " + ex.Message);
                log.LogError($"Exception IVTA011001: {ex.Message}");
            }
            
        }
    }
}

