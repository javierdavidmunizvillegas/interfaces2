using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using IVTA011.Infraestructura.Configuracion;
using Microsoft.Extensions.Configuration;
using IVTA011.Models._002.Request;
using Newtonsoft.Json;
using IVTA011.Infraestructura.Servicios;
using IVTA011.Models._002.Response;
using System.Diagnostics;
using IVTA011.Servicios;

namespace IVTA011
{
    public static class APIVTA011002
    {
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService002");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService002");
        public static string sbConenctionStringReceptar = Environment.GetEnvironmentVariable("ConectionStringRequest002");
        private static string nombrecolarequest = Environment.GetEnvironmentVariable("QueueRequest002");
        private static string nombrecolaresponse = Environment.GetEnvironmentVariable("QueueResponse002");
        private static string sbConenctionStringEnvio = Environment.GetEnvironmentVariable("ConectionStringResponse002");
        private static string sbUriConsumoWebServiceHomologa = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriConsumowebServiceHomologaAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string Entorno = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static int vl_IntentosWS = Convert.ToInt32(Environment.GetEnvironmentVariable("IntentosWS02"));
        private static int vl_TimeWS = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleepWS"));


        private static RegistroLog Logger = new RegistroLog();

        [FunctionName("IVTA011002")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest002%", Connection = "ConectionStringRequest002")] string myQueueItem, ILogger log)
        {
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                Logger.FileLogger("APIVTA011002","Request Recibido de Dynamics :  "+ myQueueItem);
                var APIVTA011002Request = JsonConvert.DeserializeObject<APIVTA011002MessageRequest>(myQueueItem);
                if (string.IsNullOrEmpty(APIVTA011002Request.SessionId) || string.IsNullOrWhiteSpace(APIVTA011002Request.SessionId))
                {
                    Logger.FileLogger("APIVTA011002", $"FUNCION WS Valida campo: Session null, se asignará vacío");
                    APIVTA011002Request.SessionId = "";
                }
                //Homologacion a Siac
                string DataAreaId = APIVTA011002Request.DataAreaId;
                //medir tiempo transcurrido en ws
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;
                long start = 0, end = 0;
                double diff = 0;
                ResponseHomologa002 ResuldatoHomologa = null;
                string responseHomologa = string.Empty;
                RespuestaWS002 objResponse = new RespuestaWS002();
                APIVTA011002MessageResponse APIVTA011002Response = new APIVTA011002MessageResponse();
                ConsumoWebService<RespuestaWS002> cws = new ConsumoWebService<RespuestaWS002>();

                ConsumoWebService<ResponseHomologa002> cwsHomologa = new ConsumoWebService<ResponseHomologa002>();
                //sesionid = Guid.NewGuid().ToString(); // hasta mientras, se debe borrar
                string sesionid = APIVTA011002Request.SessionId.ToString();
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
                            Logger.FileLogger("APIVTA011002", $"FUNCION WS HOMOLOGACION: Empresa No existe");
                            cont = i;
                            break;
                        }
                        if (ResuldatoHomologa != null && (ResuldatoHomologa.DescripcionId != ""))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            APIVTA011002Request.DataAreaId = responseHomologa;
                            Logger.FileLogger("APIVTA011002", $"FUNCION WS HOMOLOGACION: Código de empresa a enviarse a Legado es : {responseHomologa}");
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
                Logger.FileLogger("APIVTA011002", $"FUNCION WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa != null && !(string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                {


                    // asignar campo ambiente
                    APIVTA011002Request.Enviroment = Entorno; //para mapear si es necesario
                    List<APCustInvoiceHeader> RequestWs = new List<APCustInvoiceHeader>();
                    RequestWs = APIVTA011002Request.ApCustInvoiceHeaderList;
                    string jsonData = JsonConvert.SerializeObject(RequestWs);
                    Logger.FileLogger("APIVTA011002","jsonData:  "+ jsonData);
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
                    Logger.FileLogger("APIVTA011002", $"FUNCION WS RECEPCION FACTURA: Número de intentos realizados : {cont} En Ws Recepción de Factura,Tiempo Transcurrido : {diff} ms.");

                    if (objResponse == null)
                    {
                        Logger.FileLogger("APIVTA011002", $"FUNCION WS RECEPCION FACTURA: Error en el Servicio de Recepción de Facturas");
                        APIVTA011002Response.SessionId = sesionid;
                        APIVTA011002Response.StatusId = false;
                        APIVTA011002Response.ErrorList = new List<string>();
                        APIVTA011002Response.ErrorList.Add("Error  WS PERIODO: Error en el Servicio de Períodos");
                    }
                    else
                    {
                        // mapear la clase response del metodo con respuesta del WS
                        APIVTA011002Response.SessionId = sesionid;
                        APIVTA011002Response.StatusId = objResponse.StatusId;
                       // APIVTA011002Response.ErrorList = new List<string>();
                        APIVTA011002Response.ErrorList = objResponse.ErrorList;

                    }
            }
            else
            {
                    Logger.FileLogger("APIVTA011002", $"FUNCION WS HOMOLOGACION: Error en el Servicio Homologación");
                    APIVTA011002Response.SessionId = sesionid;
                    APIVTA011002Response.StatusId = false;
                    APIVTA011002Response.ErrorList = new List<string>();
                    APIVTA011002Response.ErrorList.Add("Error  WS HOMOLOGACION: Error en el Servicio Homologación");
                }
            
            string responseLog = JsonConvert.SerializeObject(objResponse);
            Logger.FileLogger("APIVTA011002", "FUNCION: Resultado en ejecucion WS RECEPCION FACTURA: " + responseLog);
            ManejadorRequestQueue<APIVTA011002MessageResponse> _manejadorRequestQueue = new ManejadorRequestQueue<APIVTA011002MessageResponse>();
            await _manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolaresponse, APIVTA011002Response);
            responseLog = JsonConvert.SerializeObject(APIVTA011002Response);
            Logger.FileLogger("APIVTA011002", "Response a Dynamics : " + responseLog);

            }
            catch (Exception ex)
            {
                Logger.FileLogger("APIVTA011002", "Error por Exception: " + ex.Message);
                log.LogError($"Exception APIVTA011002:  {ex.Message}");
            }
        }
    }
}
