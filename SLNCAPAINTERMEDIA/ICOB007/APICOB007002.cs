using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ICOB007.Infraestructura.Configuracion;
using ICOB007.Infraestructura.Servicios;


using ICOB007.Models._002.Request;
using ICOB007.Models._002.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static ICOB007.Infraestructura.Servicios.ManejadorRequestQueue;

namespace ICOB007
{
    public static class APICOB007002
    {
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService002");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService002");
        public static string sbConenctionStringReceptar = Environment.GetEnvironmentVariable("ConectionStringRequest002");
        private static string nombrecolaresponse = Environment.GetEnvironmentVariable("QueueResponse002");
        private static string sbConenctionStringEnvio = Environment.GetEnvironmentVariable("ConectionStringResponse002");
        private static string sbUriConsumoWebServiceHomologa = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriConsumowebServiceHomologaAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string Entorno = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static int vl_IntentosWS = Convert.ToInt32(Environment.GetEnvironmentVariable("IntentosWS01"));
        private static int vl_TimeWS = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleepWS"));
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32(Environment.GetEnvironmentVariable("LongAllenar"));
        private static RegistroLog Logger = new RegistroLog();
        [FunctionName("APICOB007002")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest002%", Connection = "ConectionStringRequest002")]string myQueueItem, ILogger log)
        {
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                Logger.FileLogger("APICOB007002", "Request Recibido de Dynamics :  " + myQueueItem);
                var APICOB007002Request = JsonConvert.DeserializeObject<APICOB007002MessageRequest>(myQueueItem);
                if (string.IsNullOrEmpty(APICOB007002Request.SessionId) || string.IsNullOrWhiteSpace(APICOB007002Request.SessionId))
                {
                    Logger.FileLogger("APICOB007002", $"FUNCION WS Valida campo: Session null, se asignará vacío");
                    APICOB007002Request.SessionId = "";
                }
                //Homologacion a Siac
                string DataAreaId = APICOB007002Request.DataAreaId;
                //medir tiempo transcurrido en ws
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;
                long start = 0, end = 0;
                double diff = 0;
                ResponseHomologa002 ResuldatoHomologa = null;
                string responseHomologa = string.Empty;
                RespuestaWS002 objResponse = new RespuestaWS002();
                APICOB007002MessageResponse APICOB007002Response = new APICOB007002MessageResponse();
                ConsumoWebService<RespuestaWS002> cws = new ConsumoWebService<RespuestaWS002>();
                List<APDacionQualifiedRequest> APDacionQualifiedRequestList = null;
                APDacionQualifiedRequest APDacionQualifiedRequestW = null;
                APDacionItemQualifiedRequest APDacionItemQualifiedRequestW = null;

                ConsumoWebService<ResponseHomologa002> cwsHomologa = new ConsumoWebService<ResponseHomologa002>();
                //sesionid = Guid.NewGuid().ToString(); // hasta mientras, se debe borrar
                string sesionid = APICOB007002Request.SessionId.ToString();
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
                            Logger.FileLogger("APICOB007002", $"FUNCION WS HOMOLOGACION: Empresa No existe");
                            cont = i;
                            break;
                        }
                        if (ResuldatoHomologa != null && (ResuldatoHomologa.DescripcionId != ""))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            APICOB007002Request.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICOB007002", $"FUNCION WS HOMOLOGACION: Código de empresa a enviarse a Legado es : {responseHomologa}");
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
                Logger.FileLogger("APICOB007002", $"FUNCION WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa != null && !(string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                {


                    // asignar campo ambiente
                    APICOB007002Request.Enviroment = Entorno; //para mapear si es necesario
                    //List<APDacionQualifiedRequest> RequestWs = new List<APDacionQualifiedRequest>();
                    //RequestWs. = APICOB007002Request.DocItemList;
                    APDacionQualifiedRequestList = new List<APDacionQualifiedRequest>();
                   if (APICOB007002Request.DocItemList != null)
                    foreach (var elem in APICOB007002Request.DocItemList) 
                    {
                        APDacionQualifiedRequestW = new APDacionQualifiedRequest();
                        APDacionQualifiedRequestW.Dacion = elem.Dacion;
                        APDacionQualifiedRequestW.TransDate = elem.TransDate;
                        APDacionQualifiedRequestW.DocumentRetire = elem.DocumentRetire;
                        APDacionQualifiedRequestW.UserName = elem.UserName;
                        APDacionQualifiedRequestW.Invoice = elem.Invoice;
                        APDacionQualifiedRequestW.InvoiceDate = elem.InvoiceDate;
                        APDacionQualifiedRequestW.CustAccount = CodigoCliente.DynamicAcrecos(elem.CustAccount, "APICOB007002");
                        APDacionQualifiedRequestW.NameAccount = elem.NameAccount;
                        APDacionQualifiedRequestW.Status = elem.Status;
                        APDacionQualifiedRequestW.ItemList = new List<APDacionItemQualifiedRequest>();
                        if (elem.ItemList != null)
                        foreach (var elem1 in elem.ItemList)
                        {
                            APDacionItemQualifiedRequestW = new APDacionItemQualifiedRequest();
                            APDacionItemQualifiedRequestW.ItemId = elem1.ItemId;
                            APDacionItemQualifiedRequestW.Serie = elem1.Serie;
                            APDacionItemQualifiedRequestW.ItemName = elem1.ItemName;
                            APDacionItemQualifiedRequestW.Mark = elem1.Mark;
                            APDacionItemQualifiedRequestW.Line = elem1.Line;
                            APDacionItemQualifiedRequestW.Group = elem1.Group;
                            APDacionItemQualifiedRequestW.SubGroup = elem1.SubGroup;
                            APDacionItemQualifiedRequestW.Capacity = elem1.Capacity;
                            APDacionItemQualifiedRequestW.NumberOT = elem1.NumberOT;
                            APDacionItemQualifiedRequestW.ItemIdQualified = elem1.ItemIdQualified;
                            APDacionItemQualifiedRequestW.Qty = elem1.Qty;
                            APDacionItemQualifiedRequestW.PriceUnit = elem1.PriceUnit;
                            APDacionItemQualifiedRequestW.Qualified = elem1.Qualified;
                            APDacionItemQualifiedRequestW.Observation = elem1.Observation;
                            APDacionQualifiedRequestW.ItemList.Add(APDacionItemQualifiedRequestW);
                        }
                        APDacionQualifiedRequestList.Add(APDacionQualifiedRequestW);
                    }

                    
                    string jsonData = JsonConvert.SerializeObject(APICOB007002Request);
                    Logger.FileLogger("APICOB007002", "APICOB007002Request:  " + jsonData);
                    jsonData = JsonConvert.SerializeObject(APDacionQualifiedRequestList);
                    Logger.FileLogger("APICOB007002", "jsonData:  " + jsonData);
                    start = Stopwatch.GetTimestamp();
                    for (int i = 1; i < vl_IntentosWS; i++)
                    {
                        try
                        {
                            objResponse = cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData);
                            if (objResponse != null) //&& (objResponse.StatusId))
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
                    Logger.FileLogger("APICOB007002", $"FUNCION WS Registra productos en Dación: Número de intentos realizados : {cont} En Ws Recepción de Factura,Tiempo Transcurrido : {diff} ms.");

                    if (objResponse == null)
                    {
                        Logger.FileLogger("APICOB007002", $"FUNCION WS Registra productos en Dación: Error en el Servicio de Registra productos en Dación");
                        APICOB007002Response.SessionId = sesionid;
                        APICOB007002Response.StatusId = false;
                        APICOB007002Response.ErrorList = new List<string>();
                        APICOB007002Response.ErrorList.Add("Error  WS Registra productos en Dación: Error en el Servicio de Registra productos en Dación");
                    }
                    else
                    {
                        // mapear la clase response del metodo con respuesta del WS
                        APICOB007002Response.SessionId = sesionid;
                        APICOB007002Response.StatusId = true; // objResponse.;
                        APICOB007002Response.ErrorList = new List<string>();
                        APICOB007002Response.ErrorList.Add(objResponse.descripcionTransaccion);
                        APICOB007002Response.NumberOTList = new List<string>();
                      
                        if (objResponse.ordenesTrabajo != null &&  objResponse.ordenesTrabajo.Count > 0)
                        {
                            foreach (var elem in objResponse.ordenesTrabajo)
                            {
                                APICOB007002Response.NumberOTList.Add(elem);
                            }
                        }
                    }
                }
                else
                {
                    Logger.FileLogger("APICOB007002", $"FUNCION WS HOMOLOGACION: Error en el Servicio Homologación");
                    APICOB007002Response.SessionId = sesionid;
                    APICOB007002Response.StatusId = false;
                    APICOB007002Response.ErrorList = new List<string>();
                    APICOB007002Response.ErrorList.Add("Error  WS HOMOLOGACION: Error en el Servicio Homologación");
                }

                string responseLog = JsonConvert.SerializeObject(objResponse);
                Logger.FileLogger("APICOB007002", "FUNCION: Resultado en ejecucion WS Registra productos en Dación: " + responseLog);
                ManejadorRequestQueue<APICOB007002MessageResponse> _manejadorRequestQueue = new ManejadorRequestQueue<APICOB007002MessageResponse>();
                await _manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolaresponse, APICOB007002Response);
                responseLog = JsonConvert.SerializeObject(APICOB007002Response);
                Logger.FileLogger("APICOB007002", "Response a Dynamics : " + responseLog);

            }
            catch (Exception ex)
            {
                Logger.FileLogger("APICOB007002", "Error por Exception: " + ex.Message);
                log.LogError($"Exception APICOB007002:  {ex.Message}");
            }
        }
    }
    }

