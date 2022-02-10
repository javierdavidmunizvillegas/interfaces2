using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ICOB007.Infraestructura.Configuracion;
using ICOB007.Infraestructura.Servicios;
using ICOB007.Models._001.Request;
using ICOB007.Models._001.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System.Net;
using System.Threading.Tasks;

using static ICOB007.Infraestructura.Servicios.ManejadorRequestQueue;

namespace ICOB007
{
    public static class APICOB007001
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
        private static string respuesta_ws001;
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32(Environment.GetEnvironmentVariable("LongAllenar"));
        private static RegistroLog Logger = new RegistroLog();


        [FunctionName("APICOB007001")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")]string myQueueItem, ILogger log)
        {
            
         


            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                Logger.FileLogger("APICOB007001", "Request Recibido de Dynamics :  " + myQueueItem);
                var APICOB007001Request = JsonConvert.DeserializeObject<APICOB007001MessageRequest>(myQueueItem);
                if (string.IsNullOrEmpty(APICOB007001Request.SessionId) || string.IsNullOrWhiteSpace(APICOB007001Request.SessionId))
                {
                    Logger.FileLogger("APICOB007001", $"FUNCION WS Valida campo: Session null, se asignará vacío");
                    APICOB007001Request.SessionId = "";
                }
                //Homologacion a Siac
                string DataAreaId = APICOB007001Request.DataAreaId;
                //medir tiempo transcurrido en ws
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;
                long start = 0, end = 0;
                double diff = 0;
                ResponseHomologa001 ResuldatoHomologa = null;
                string responseHomologa = string.Empty;
                RespuestaWSOT objResponse = null;
                APICOB007001MessageResponse APICOB007001Response = new APICOB007001MessageResponse();
                ConsumoWebServiceOT cwsOT = new ConsumoWebServiceOT();
                ConsumoWebService<ResponseHomologa001> cwsHomologa = new ConsumoWebService<ResponseHomologa001>();
                //sesionid = Guid.NewGuid().ToString(); // hasta mientras, se debe borrar
                string sesionid = APICOB007001Request.SessionId.ToString();
                APICOB007001MessageRequest APICOB007001MessageRequestW = new APICOB007001MessageRequest();




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
                            Logger.FileLogger("APICOB007001", $"FUNCION WS HOMOLOGACION: Empresa No existe");
                            cont = i;
                            break;
                        }
                        if (ResuldatoHomologa != null && (ResuldatoHomologa.DescripcionId != ""))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            APICOB007001Request.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICOB007001", $"FUNCION WS HOMOLOGACION: Código de empresa a enviarse a Legado es : {responseHomologa}");
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
                Logger.FileLogger("APICOB007001", $"FUNCION WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa != null && !(string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                {


                    // asignar campo ambiente
                    APICOB007001Request.Enviroment = Entorno; //para mapear si es necesario
                    /*APICOB007001MessageRequestW.DataAreaId = APICOB007001Request.DataAreaId;
                    APICOB007001MessageRequestW.Enviroment = APICOB007001Request.Enviroment;
                    APICOB007001MessageRequestW.SessionId = APICOB007001Request.SessionId;
                    APICOB007001MessageRequestW.Sucursal = APICOB007001Request.Sucursal;
                    APICOB007001MessageRequestW.TypeAttention = APICOB007001Request.TypeAttention;
                    APICOB007001MessageRequestW.PlaceAttention = APICOB007001Request.PlaceAttention;
                    APICOB007001MessageRequestW.CodeWorkshop = APICOB007001Request.CodeWorkshop;
                    APICOB007001MessageRequestW.IdentificationType = APICOB007001Request.IdentificationType;
                    APICOB007001MessageRequestW.IdentificationNum = APICOB007001Request.IdentificationNum;
                    APICOB007001MessageRequestW.NameClient = APICOB007001Request.NameClient;
                    APICOB007001MessageRequestW.FirstName = APICOB007001Request.FirstName;
                    APICOB007001MessageRequestW.SecondName = APICOB007001Request.SecondName;
                    APICOB007001MessageRequestW.LastName = APICOB007001Request.Enviroment;
                    APICOB007001MessageRequestW.LastSecondName = APICOB007001Request.LastSecondName;
                    APICOB007001MessageRequestW.Country = APICOB007001Request.Country;
                    APICOB007001MessageRequestW.State = APICOB007001Request.State;
                    APICOB007001MessageRequestW.City = APICOB007001Request.City;
                    APICOB007001MessageRequestW.Street = APICOB007001Request.Street;
                    APICOB007001MessageRequestW.NumberStreet = APICOB007001Request.NumberStreet;
                    APICOB007001MessageRequestW.Phone = APICOB007001Request.Phone;
                    APICOB007001MessageRequestW.Email = APICOB007001Request.Email;
                    APICOB007001MessageRequestW.Warehouse = APICOB007001Request.Warehouse;
                    APICOB007001MessageRequestW.Invoice = APICOB007001Request.Invoice;
                    APICOB007001MessageRequestW.InvoiceSRI = APICOB007001Request.InvoiceSRI;
                    APICOB007001MessageRequestW.InvoiceDate = APICOB007001Request.InvoiceDate;
                    APICOB007001MessageRequestW.Dacion = APICOB007001Request.Dacion;
                    APICOB007001MessageRequestW.ItemId = APICOB007001Request.ItemId;
                    APICOB007001MessageRequestW.Serie = APICOB007001Request.Serie;
                    APICOB007001MessageRequestW.ModelItem = APICOB007001Request.ModelItem;
                    APICOB007001MessageRequestW.DescriptionItem = APICOB007001Request.DescriptionItem;
                    APICOB007001MessageRequestW.Mark = APICOB007001Request.Mark;
                    APICOB007001MessageRequestW.Line = APICOB007001Request.Line;
                    APICOB007001MessageRequestW.Group = APICOB007001Request.Group;
                    APICOB007001MessageRequestW.SubGroup = APICOB007001Request.SubGroup;
                    APICOB007001MessageRequestW.Capacity = APICOB007001Request.Capacity;
                    APICOB007001MessageRequestW.EntAssetObjectID = APICOB007001Request.EntAssetObjectID;
                    APICOB007001MessageRequestW.Origen = APICOB007001Request.Origen;
                    APICOB007001MessageRequestW.RecId = APICOB007001Request.RecId;
                    APICOB007001MessageRequestW.RecIdLine = APICOB007001Request.RecIdLine;*/
                    if (APICOB007001Request.CustAccount != null)
                      APICOB007001Request.CustAccount = CodigoCliente.DynamicAcrecos(APICOB007001Request.CustAccount, "APICOB007001");
                   
                    string jsonData = JsonConvert.SerializeObject(APICOB007001Request);
                    Logger.FileLogger("APICOB007001", "jsonData:  " + jsonData);
                    start = Stopwatch.GetTimestamp();
                    for (int i = 1; i < vl_IntentosWS; i++)
                    {
                        try
                        {
                            objResponse = cwsOT.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData);
                           //var respuesta_ws002 = cws2.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData);
                           
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
                    Logger.FileLogger("APICOB007001", $"FUNCION WS Creación Orden de Trabajo: Número de intentos realizados : {cont} En Ws Creación Orden de Trabajo,Tiempo Transcurrido : {diff} ms.");

                    if (objResponse == null)
                    {
                        Logger.FileLogger("APICOB007001", $"FUNCION WS Creación Orden de Trabajo: Error en el Servicio de Creación Orden de Trabajo");
                        APICOB007001Response.SessionId = sesionid;
                        APICOB007001Response.StatusId = false;
                        APICOB007001Response.ErrorList = new List<string>();
                        APICOB007001Response.ErrorList.Add("Error  WS Creación Orden de Trabajo: Error en el Servicio de Creación Orden de Trabajo");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(objResponse.NumberOT))
                        {
                            //retorna json porque hay error
                           // mapear la clase response del metodo con respuesta del WS
                            APICOB007001Response.SessionId = sesionid;
                            APICOB007001Response.StatusId = false; // objResponse.;
                            APICOB007001Response.ErrorList = new List<string>();
                            if (objResponse.Respuesta != null)
                            {
                                APICOB007001Response.ErrorList.Add(objResponse.Respuesta.DescripcionTransaccion); 
                            }
                            else 
                            {
                                APICOB007001Response.ErrorList.Add("Error , retorna vacion , No content");
                            }
                            APICOB007001Response.NumberOT = "";// objResponse.;
                            APICOB007001Response.Dacion = APICOB007001Request.Dacion;
                            APICOB007001Response.RecIdLine = APICOB007001Request.RecIdLine;
                        }
                        else
                        {
                           
                            // mapear la clase response del metodo con respuesta del WS
                            APICOB007001Response.SessionId = sesionid;
                            APICOB007001Response.StatusId = true; // objResponse.;
                            APICOB007001Response.ErrorList = new List<string>();
                            // APICOB007001Response.ErrorList.Add(objResponse.EstadoTransaccion);
                            APICOB007001Response.NumberOT = objResponse.NumberOT;// objResponse.;
                            APICOB007001Response.Dacion = APICOB007001Request.Dacion;
                            APICOB007001Response.RecIdLine = APICOB007001Request.RecIdLine;

                        }

                    }
                }
                else
                {
                    Logger.FileLogger("APICOB007001", $"FUNCION WS HOMOLOGACION: Error en el Servicio Homologación");
                    APICOB007001Response.SessionId = sesionid;
                    APICOB007001Response.StatusId = false;
                    APICOB007001Response.ErrorList = new List<string>();
                    APICOB007001Response.ErrorList.Add("Error  WS HOMOLOGACION: Error en el Servicio Homologación");
                }

                string responseLog = JsonConvert.SerializeObject(objResponse);
                Logger.FileLogger("APICOB007001", "FUNCION: Resultado en ejecucion WS Creación Orden de Trabajo: " + responseLog);
                ManejadorRequestQueue<APICOB007001MessageResponse> _manejadorRequestQueue = new ManejadorRequestQueue<APICOB007001MessageResponse>();
                await _manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolaresponse, APICOB007001Response);
                responseLog = JsonConvert.SerializeObject(APICOB007001Response);
                Logger.FileLogger("APICOB007001", "Response a Dynamics : " + responseLog);

            }
            catch (Exception ex)
            {
                Logger.FileLogger("APICOB007001", "Error por Exception: " + ex.Message);
                log.LogError($"Exception APICOB007001:  {ex.Message}");
            }
        }
    }
}
