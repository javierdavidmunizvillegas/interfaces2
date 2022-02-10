using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using DSAC003001.Infraestructura.Configuracion;
using DSAC003001.Infraestructura.Servicios;
using DSAC003001.Models._001.Request;
using DSAC003001.Models._001.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace DSAC003001
{
    public static class DSAC003001
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
        
        

        [FunctionName("DSAC003001")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")]string myQueueItem, ILogger log)
        {
            
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                Logger.FileLogger("APDSAC003001", "Request Recibido de Dynamics : " + myQueueItem);
                var APDSAC003001Request = JsonConvert.DeserializeObject<APDSAC003001MessageRequest>(myQueueItem);

                //Homologacion a Siac
                string DataAreaId = APDSAC003001Request.DataAreaId;
                //medir tiempo transcurrido en ws
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;
                long start = 0, end = 0;
                double diff = 0;
                ResponseHomologa001 ResuldatoHomologa = null;
                string responseHomologa = string.Empty;
                string responseWS001 = string.Empty;
                string sesionid = string.Empty;
                string responseLog = string.Empty;
                List<APTechnicalReport> objResponse = new List<APTechnicalReport>();
                APDSAC003001MessageResponse APDSAC003001Response = new APDSAC003001MessageResponse();
                APTechnicalReport APDSAC003001ResponseList = null;
                ConsumoWebService<ResponseHomologa001> cwsHomologa = new ConsumoWebService<ResponseHomologa001>();
                

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
                            Logger.FileLogger("APDSAC003001", $"FUNCION WS HOMOLOGACION: Empresa No existe");
                            return;
                        }
                        if (ResuldatoHomologa != null && "OK".Equals(ResuldatoHomologa.DescripcionId))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            APDSAC003001Request.DataAreaId = responseHomologa;
                            Logger.FileLogger("APDSAC003001", $"FUNCION WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APDSAC003001", $"FUNCION WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");
                sesionid = APDSAC003001Request.SessionId.ToString();
                APDSAC003001Response.SessionId = sesionid;
                // asignar campo ambiente
                APDSAC003001Request.Enviroment = Entorno; //para mapear si es necesario          // ejecuta api, 
               

                if (ResuldatoHomologa != null && !(string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                {

                    // string jsonData = JsonConvert.SerializeObject(APDSAC003001Request.ObjectValidFrom);
                    DateTime dt = APDSAC003001Request.ObjectValidFrom;
                    string jsonData = dt.ToString("yyyy-MM-dd");
                    Logger.FileLogger("APDSAC003001","" + jsonData);
                    ConsumoWebService<List<APTechnicalReport>> cws = new ConsumoWebService<List<APTechnicalReport>>();

                    start = Stopwatch.GetTimestamp();
                    //int cont = 0;
                    for (int i = 1; i < vl_IntentosWS; i++)
                    {
                        try
                        {
                            objResponse = await cws.GetWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData);
                            if (objResponse != null)  //&& (objResponse.))
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
                   // Logger.FileLogger("APDSAC003001", $"FUNCION WS REPORTES TECNICOS: Número de intentos realizados : {cont} En Ws Reportes Técnicos,Tiempo Transcurrido : {diff} ms.");
                    Logger.FileLogger("APDSAC003001", $"FUNCION WS REPORTES TECNICOS: Número de intentos realizados : {cont} En Ws Reportes Técnicos,Tiempo Transcurrido : {diff} ms.");

                    if (objResponse == null)
                    {
                        Logger.FileLogger("APDSAC003001", $"FUNCION WS FECHA: Error en el Servicio de Reportes técnicos");

                        APDSAC003001Response.APTechnicalReport = new List<APTechnicalReport>();
                        APDSAC003001ResponseList = new APTechnicalReport();
                        APDSAC003001ResponseList.WorkOrderNumber = null;
                        APDSAC003001ResponseList.AssetNumber = null;
                        APDSAC003001ResponseList.ItemCode = null;
                        APDSAC003001ResponseList.SerialNumber = null;
                        APDSAC003001ResponseList.ReportType = null;
                        APDSAC003001ResponseList.Discount = 0;
                        APDSAC003001ResponseList.Suitable = false;
                        APDSAC003001Response.APTechnicalReport.Add(APDSAC003001ResponseList);

                    }
                    else 
                    { 
                 
                        responseLog = JsonConvert.SerializeObject(objResponse);
                        Logger.FileLogger("APDSAC003001", "FUNCION: Resultado en ejecucion WS REPORTE TECNICOS: " + responseLog);
                        // mapear la clase response del metodo con respuesta del WS
                        
                       
                        APDSAC003001Response.APTechnicalReport = new List<APTechnicalReport>();
                        if (objResponse.Count > 0)
                            { 
                           foreach (var elem in objResponse)
                            {
                                APDSAC003001ResponseList = new APTechnicalReport();
                                APDSAC003001ResponseList.WorkOrderNumber = elem.WorkOrderNumber;
                                APDSAC003001ResponseList.AssetNumber = elem.AssetNumber;
                                APDSAC003001ResponseList.ItemCode = elem.ItemCode;
                                APDSAC003001ResponseList.SerialNumber = elem.SerialNumber;
                                APDSAC003001ResponseList.ReportType = elem.ReportType;
                                APDSAC003001ResponseList.Discount = elem.Discount;
                                APDSAC003001ResponseList.Suitable = elem.Suitable;
                                APDSAC003001Response.APTechnicalReport.Add(APDSAC003001ResponseList);

                            }
                        }
                       // Logger.FileLogger("APDSAC003001", "Response WS Informes Tecnicos: " + responseLog);

                        // fin mapeo

                        
                    }

                }
                else
                {
                    APDSAC003001Response.APTechnicalReport = new List<APTechnicalReport>();
                    APDSAC003001ResponseList = new APTechnicalReport();
                    APDSAC003001ResponseList.WorkOrderNumber =null;
                    APDSAC003001ResponseList.AssetNumber = null;
                    APDSAC003001ResponseList.ItemCode = null;
                    APDSAC003001ResponseList.SerialNumber = null;
                    APDSAC003001ResponseList.ReportType = null;
                    APDSAC003001ResponseList.Discount = 0;
                    APDSAC003001ResponseList.Suitable = false;
                    APDSAC003001Response.APTechnicalReport.Add(APDSAC003001ResponseList);

                    Logger.FileLogger("APDSAC003001", $"FUNCION WS HOMOLOGACION: Error en el Servicio Homologación");
                   // return;
                
                 }
                ManejadorRequestQueue<APDSAC003001MessageResponse> _manejadorRequestQueue = new ManejadorRequestQueue<APDSAC003001MessageResponse>();
                await _manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolaresponse, APDSAC003001Response);

                responseLog = JsonConvert.SerializeObject(APDSAC003001Response);
                Logger.FileLogger("APDSAC003001", "Response a Dynamics : " + responseLog);

            }
            catch (Exception ex)
            {
                Logger.FileLogger("APDSAC003001", "Error por Exception: " + ex.Message);
                log.LogError($"Exception IDSAC003001: {ex.Message}");
            }

        }
    }
}

