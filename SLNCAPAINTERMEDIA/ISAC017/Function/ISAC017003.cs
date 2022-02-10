using Interface.Api.Infraestructura.Configuracion;
using ISAC017.Infraestructure.Configuration;
using ISAC017.Models;
using ISAC017.Models._003.Request;
using ISAC017.Models._003.Response;
using IVTA017.Infraestructure.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace ISAC017.Function
{
    class ISAC017003
    {
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService");
        private static string sbUriConsumoWebServiceHomologa = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriConsumowebServiceHomologaAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string Entorno = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static int vl_IntentosWS = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static int vl_TimeWS = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static RegistroLog Logger = new RegistroLog();
       // private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();


        [FunctionName("APISAC017003")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")] string myQueueItem, ILogger log)
        {

            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                Logger.FileLogger("APISAC017003", "FUNCION : Request Recibido: " + myQueueItem);
                var APISAC017003Request = JsonConvert.DeserializeObject<APISAC017003MessageRequest>(myQueueItem);
                if (string.IsNullOrEmpty(APISAC017003Request.SessionId) || string.IsNullOrWhiteSpace(APISAC017003Request.SessionId))
                {
                    Logger.FileLogger("APISAC017003", $"FUNCION WS Valida campo: Session null, se asignará vacío");
                    APISAC017003Request.SessionId = "";
                }
                //Homologacion a Siac
                string DataAreaId = APISAC017003Request.DataAreaId;
                //medir tiempo transcurrido en ws
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;
                long start = 0, end = 0;
                double diff = 0;
                ResponseHomologa003 ResuldatoHomologa = null;
                string responseHomologa = string.Empty;
                string responseWS001 = string.Empty;
                ResponseWS objResponse = new ResponseWS();
                string respuestaLog;
                ConsumoWebService<ResponseWS> cws = new ConsumoWebService<ResponseWS>();


                ConsumoWebService<ResponseHomologa003> cwsHomologa = new ConsumoWebService<ResponseHomologa003>();
                //sesionid = Guid.NewGuid().ToString(); // hasta mientras, se debe borrar
                string sesionid = APISAC017003Request.SessionId.ToString();
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
                            Logger.FileLogger("APISAC017003", $"FUNCION WS HOMOLOGACION: Empresa No existe");
                            cont = i;
                            break;
                        }
                        if (ResuldatoHomologa != null && (ResuldatoHomologa.DescripcionId != ""))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            APISAC017003Request.DataAreaId = responseHomologa;
                            Logger.FileLogger("APISAC017003", $"FUNCION WS HOMOLOGACION: Código de empresa a enviarse a Legado es : {responseHomologa}");
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
                Logger.FileLogger("APISAC017003", $"FUNCION WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa != null && !(string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                {

                    string jsonData = JsonConvert.SerializeObject(APISAC017003Request);
               


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
                        Logger.FileLogger("APISAC017003", $"FUNCION WS ActualizaOrdenDevolucion : Número de intentos realizados : {cont} En Ws ActualizaOrdenDevolucion,Tiempo Transcurrido : {diff} ms.");

                        if (objResponse == null)
                        {
                            Logger.FileLogger("APISAC017003", $"FUNCION WS ActualizaOrdenDevolucion. Error en el Servicio de ActualizaOrdenDevolucion");
                            
                        }
                        else
                        {
                        respuestaLog = JsonConvert.SerializeObject(objResponse);
                        Logger.FileLogger("APISAC017003", "Responde WS ActualizaOrdenDevolucion: " + respuestaLog);
                        }
                }
                else
                {
                    Logger.FileLogger("APISAC017003", $"FUNCION WS HOMOLOGACION: Error en el Servicio Homologación");
                   
                }
            }
            catch (Exception ex)
            {
                Logger.FileLogger("APISAC017003", "FUNCION : Error por Excepcion: " + ex.ToString());
                log.LogError($"Exception APISAC017003: {ex.Message}");

            }

        }
    }
}
