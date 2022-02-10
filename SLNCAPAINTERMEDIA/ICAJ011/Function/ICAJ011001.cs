using ICAJ011.Infraestructura.Configuracion;
using ICAJ011.Infraestructura.Service;
using ICAJ011.Infraestructura.Servicios;
using ICAJ011.Models;
using ICAJ011.Models._001.Request;
using ICAJ011.Models._001.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ICAJ011.Function
{
    public static class ICAJ011001
    {

        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService");
        private static string nombrecolaresponse = Environment.GetEnvironmentVariable("QueueResponse001");
        private static string sbUriConsumoWebServiceHomologa = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriConsumowebServiceHomologaAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string sbConenctionStringEnvio = Environment.GetEnvironmentVariable("ConectionStringResponse001");
        private static string Entorno = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static int vl_IntentosWS = Convert.ToInt32(Environment.GetEnvironmentVariable("IntentosWS"));
        private static int vl_TimeWS = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleepWS"));
        private static int vl_Intentos = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static RegistroLog Logger = new RegistroLog();

        [FunctionName("ICAJ011001")]
            public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")] string myQueueItem, ILogger log)
            {

            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                Logger.FileLogger("APICAJ011001", "FUNCION : Request Recibido: " + myQueueItem);
                var APICAJ011001Request = JsonConvert.DeserializeObject<APICAJ011001MessageRequest>(myQueueItem);
                if (string.IsNullOrEmpty(APICAJ011001Request.SessionId) || string.IsNullOrWhiteSpace(APICAJ011001Request.SessionId))
                {
                    Logger.FileLogger("APICAJ011001", $"FUNCION WS Valida campo: Session null, se asignará vacío");
                    APICAJ011001Request.SessionId = "";
                }
                //Homologacion a Siac
                string DataAreaId = APICAJ011001Request.DataAreaId;
                //medir tiempo transcurrido en ws
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;
                long start = 0, end = 0;
                double diff = 0;
                ResponseHomologa001 ResuldatoHomologa = null;
                string responseHomologa = string.Empty;
                string responseWS001 = string.Empty;
                ResponseWS objResponse = new ResponseWS();
                string respuestaLog;
                ConsumoWebService<ResponseWS> cws = new ConsumoWebService<ResponseWS>();
                APICAJ011001MessageResponse APICAJ011001Response = new APICAJ011001MessageResponse();


                ConsumoWebService<ResponseHomologa001> cwsHomologa = new ConsumoWebService<ResponseHomologa001>();
                //sesionid = Guid.NewGuid().ToString(); // hasta mientras, se debe borrar
                string sesionid = APICAJ011001Request.SessionId.ToString();
                start = Stopwatch.GetTimestamp();
                int cont = 0;
                for (int i = 1; i < vl_Intentos; i++)
                {
                    try
                    {
                        ResuldatoHomologa = await cwsHomologa.GetHomologacion(sbUriConsumoWebServiceHomologa, sbMetodoWsUriConsumowebServiceHomologaAx, DataAreaId);
                        // Validacion el objeto recibido
                        // Condicion para salir
                        if (ResuldatoHomologa != null && (string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                        {
                            Logger.FileLogger("APICAJ011001", $"FUNCION WS HOMOLOGACION: Empresa No existe");
                            cont = i;
                            break;
                        }
                        if (ResuldatoHomologa != null && (ResuldatoHomologa.DescripcionId != ""))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            APICAJ011001Request.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICAJ011001", $"FUNCION WS HOMOLOGACION: Código de empresa a enviarse a Legado es : {responseHomologa}");
                            cont = i;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    await Task.Delay(vl_Time);
                } // fin for
                end = Stopwatch.GetTimestamp();
                diff = start > 0 ? (end - start) / NanoAMiliS : 0;
                Logger.FileLogger("APICAJ011001", $"FUNCION WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa != null && !(string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                {

                    string jsonData = JsonConvert.SerializeObject(APICAJ011001Request);



                    start = Stopwatch.GetTimestamp();
                    cont = 0;
                    for (int i = 1; i <= vl_IntentosWS; i++)
                    {
                        try
                        {
                            cont = i;
                            objResponse = cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData);
                            if (objResponse != null && (objResponse.statusCode != null))
                            {
                                break;
                            }
                            if (objResponse != null && "OK".Equals(objResponse.descripcionId))
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
                    Logger.FileLogger("APICAJ011001", $"FUNCION WS ActualizaOrdenDevolucion : Número de intentos realizados : {cont} En Ws ActualizaOrdenDevolucion,Tiempo Transcurrido : {diff} ms.");

                    if (objResponse == null)
                    {
                        Logger.FileLogger("APICAJ011001", $"FUNCION WS ActualizaOrdenDevolucion. Error en el Servicio de ActualizaOrdenDevolucion");
                        APICAJ011001Response.StatusId = false;
                        APICAJ011001Response.SessionId = sesionid;
                        APICAJ011001Response.ErrorList = new List<string>();
                        APICAJ011001Response.ErrorList.Add("Error  WS  ActualizaOrdenDevolucion: Error en el Servicio de  ActualizaOrdenDevolucion");

                    }
                    else
                    {
                        respuestaLog = JsonConvert.SerializeObject(objResponse);
                        Logger.FileLogger("APICAJ011001", "Responde WS ActualizaOrdenDevolucion: " + respuestaLog);
                        if (string.IsNullOrEmpty(objResponse.response) || string.IsNullOrWhiteSpace(objResponse.response) || (objResponse.response == "ERROR"))
                        { APICAJ011001Response.StatusId = false; }
                        else {
                            APICAJ011001Response.StatusId = true;
                               };
                        
                         APICAJ011001Response.SessionId = sesionid;
                        APICAJ011001Response.ErrorList = new List<string>();
                        if (objResponse.errorList != null)
                        {foreach(var elem in objResponse.errorList)
                            APICAJ011001Response.ErrorList.Add(elem);
                        }
                         
                       
                    }
                }
                else
                {
                    Logger.FileLogger("APICAJ011001", $"FUNCION WS HOMOLOGACION: Error en el Servicio Homologación");
                    APICAJ011001Response.StatusId = false;
                    APICAJ011001Response.SessionId = sesionid;
                    APICAJ011001Response.ErrorList = new List<string>();
                    APICAJ011001Response.ErrorList.Add("Error  WS  Homologación: Error en el Servicio de  Homologación");

                }
                string responseLog = JsonConvert.SerializeObject(objResponse);
                Logger.FileLogger("APICAJ011001", "FUNCION: Resultado en ejecucion WS  ActualizaOrdenDevolucion: " + responseLog);
                IManejadorRequestQueue<APICAJ011001MessageResponse> _manejadorRequestQueue = new ManejadorRequestQueue<APICAJ011001MessageResponse>();
                await _manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolaresponse, APICAJ011001Response);
                responseLog = JsonConvert.SerializeObject(APICAJ011001Response);
                Logger.FileLogger("APICAJ011001", "Response a Dynamics : " + responseLog);
            }
            catch (Exception ex)
            {
                Logger.FileLogger("APICAJ011001", "FUNCION : Error por Excepcion: " + ex.ToString());
                log.LogError($"Exception APICAJ011001: {ex.Message}");

            }

        }
     }
}
