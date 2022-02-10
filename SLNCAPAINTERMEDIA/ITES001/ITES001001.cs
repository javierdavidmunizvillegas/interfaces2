using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ITES001.Infraestructura.Configuracion;
using ITES001.Infraestructura.Servicios;
using ITES001.Models._001.Request;
using ITES001.Models._001.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static ITES001.Infraestructura.Servicios.ConsumoWebService;

namespace ITES001
{
    public static class ITES001001
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

        [FunctionName("ITES001001")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")]string myQueueItem, ILogger log)
        {
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                Logger.FileLogger("APITES001001", "Request Recibido de Dynamics : " + myQueueItem);
                var APITES001001Request = JsonConvert.DeserializeObject<APITES001001MessageRequest>(myQueueItem);
                if (APITES001001Request.SessionId == null)
                {
                    Logger.FileLogger("APITES001001", $"FUNCION WS Valida campo: Session null, se asignará vacío");
                    APITES001001Request.SessionId = "";
                }
                //Homologacion a Siac
                string DataAreaId = APITES001001Request.DataAreaId;
                //medir tiempo transcurrido en ws
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;
                long start = 0, end = 0;
                double diff = 0;
                ResponseHomologa001 ResuldatoHomologa = null;
                string responseHomologa = string.Empty;
                string responseWS001 = string.Empty;
                APITES001001MessageResponse objResponse = new APITES001001MessageResponse();
                APITES001001MessageResponse APITES001001Response = new APITES001001MessageResponse();
                APITES001001Response.StatusId = true;
                APITES001001Response.ErrorList = null;
                APITES001001Response.APVendTransRegistrationList = null; 

                ConsumoWebService <ResponseHomologa001> cwsHomologa = new ConsumoWebService<ResponseHomologa001>();
                string responseLog = JsonConvert.SerializeObject(objResponse);
                Logger.FileLogger("APITES001001", "FUNCION: Resultado en ejecucion WS MATRICULA MULTA DE MOTO: " + responseLog);
                // mapear la clase response del metodo con respuesta del WS
                //sesionid = Guid.NewGuid().ToString(); // hasta mientras, se debe borrar

                string sesionid = APITES001001Request.SessionId.ToString();
                start = Stopwatch.GetTimestamp();
                int cont = 0;
                for (int i = 1; i < vl_IntentosWS; i++)
                {
                    try
                    { 
                         cont = i;
                         ResuldatoHomologa = await cwsHomologa.GetHomologacion(sbUriConsumoWebServiceHomologa, sbMetodoWsUriConsumowebServiceHomologaAx, DataAreaId);
                        // Validacion el objeto recibido
                        // Condicion para salir
                        if (ResuldatoHomologa != null && (string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                        {
                            Logger.FileLogger("APITES001001", $"FUNCION WS HOMOLOGACION: Empresa No existe");
                            break;
                           
                        }
                        if (ResuldatoHomologa != null && (ResuldatoHomologa.DescripcionId != ""))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            APITES001001Request.DataAreaId = responseHomologa;
                            Logger.FileLogger("APITES001001", $"FUNCION WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APITES001001", $"FUNCION WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa != null && ! (string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                {

                    // asignar campo ambiente
                    APITES001001Request.Enviroment = Entorno; //para mapear si es necesario
                                                              // ejecuta api, 
                    
                    string jsonData = JsonConvert.SerializeObject(APITES001001Request);
                    ConsumoWebService<APITES001001MessageResponse> cws = new ConsumoWebService<APITES001001MessageResponse>();

                    start = Stopwatch.GetTimestamp();
                    cont = 0;
                    for (int i = 1; i < vl_IntentosWS; i++)
                    {
                        try
                        {
                            cont = i;
                            objResponse = cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData);
                            if (objResponse != null && (string.IsNullOrEmpty(objResponse.DataAreaId) || string.IsNullOrWhiteSpace(objResponse.DataAreaId)))
                            {
                                break;
                            }
                            if (objResponse != null) //&& "OK".Equals(objResponse.DataAreaId))
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
                    Logger.FileLogger("APITES001001", $"FUNCION WS MATRICULA ,MULTA DE MOTO: Número de intentos realizados : {cont} En Ws MATRICULA MULTA DE MOTO,Tiempo Transcurrido : {diff} ms.");

                    if (objResponse == null)
                    {
                        Logger.FileLogger("APITES001001", $"FUNCION WS MATRICULA MULTA DE MOTO: Error en el Servicio de MATRICULA MULTA DE MOTO");
                        APITES001001Response.StatusId = false;
                        APITES001001Response.ErrorList = new List<string>();
                        APITES001001Response.ErrorList.Add("Error en el Servicio de MATRICULA MULTA DE MOTO ");
                       //return;
                    }
                    else
                    {
                        APITES001001Response.StatusId = true;
                        APITES001001Response.SessionId = sesionid;
                        APITES001001Response.DataAreaId = APITES001001Request.DataAreaId;
                        APITES001001Response.APVendTransRegistrationList = objResponse.APVendTransRegistrationList;
                    }
                }
                else { 
                    Logger.FileLogger("APITES001001", $"FUNCION WS HOMOLOGACION: Error en el Servicio Homologación");
                    APITES001001Response.StatusId = false;
                    APITES001001Response.ErrorList = new List<string>();
                    APITES001001Response.ErrorList.Add("Error en Homologacion");
                }
                
                // fin mapeo

                ManejadorRequestQueue<APITES001001MessageResponse> _manejadorRequestQueue = new ManejadorRequestQueue<APITES001001MessageResponse>();
                await _manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolaresponse, APITES001001Response);
                responseLog = JsonConvert.SerializeObject(APITES001001Response);
                Logger.FileLogger("APITES001001", "Response a Dynamics : " + responseLog);

            }
            catch (Exception ex)
            {
                Logger.FileLogger("APITES001001", "Error por Exception: " + ex.Message);
                log.LogError($"Exception ITES001001: {ex.Message}");
            }

        }
    }
}
