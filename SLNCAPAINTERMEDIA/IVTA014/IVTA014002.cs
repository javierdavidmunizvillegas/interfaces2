using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using IVTA014.Infraestructura.Configuracion;
using IVTA014.Infraestructura.Servicios;
using IVTA014.Models._002.Request;
using IVTA014.Models._002.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IVTA014
{
    public static class IVTA014002
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
        private static int vl_IntentosWS = Convert.ToInt32(Environment.GetEnvironmentVariable("IntentosWS"));
        private static int vl_TimeWS = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleepWS"));
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32(Environment.GetEnvironmentVariable("LongAllenar"));



        private static RegistroLog Logger = new RegistroLog();

        [FunctionName("IVTA014002")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest002%", Connection = "ConectionStringRequest002")]string myQueueItem, ILogger log)
        {
            // APCustSettlementIVTA014002 apCustInvJourResp = null;
            // APCustInvoiceTransIVTA014002 apCustInvTransResp = null;
            
            string responseLog = null;
            APIVTA014002MessageRequest APIVTA014002RequestW = null; // new APIVTA014001MessageRequestWork();
            APCustSettlementIVTA014002 tmp = null;
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                Logger.FileLogger("APIVTA014002", "Request Recibido de Dynamics : " + myQueueItem);
                var APIVTA014002Request = JsonConvert.DeserializeObject<APIVTA014002MessageRequest>(myQueueItem);


                if (string.IsNullOrEmpty(APIVTA014002Request.SessionId) || string.IsNullOrWhiteSpace(APIVTA014002Request.SessionId))
                {
                    Logger.FileLogger("APIVTA014002", $"FUNCION WS Valida campo: Session null, se asignará vacío");
                    APIVTA014002Request.SessionId = "";
                }
                // APIVTA014002MessageRequest APIVTA014002Request = new APIVTA014002MessageRequest();

                //Homologacion a Siac
                string DataAreaId = APIVTA014002Request.DataAreaId;
                //medir tiempo transcurrido en ws
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;
                long start = 0, end = 0;
                double diff = 0;
                ResponseHomologa002 ResuldatoHomologa = null;
                RespuestaWS002 objResponse = new RespuestaWS002();
                string responseHomologa = string.Empty;
                string sesionid = APIVTA014002Request.SessionId.ToString();
               
                ConsumoWebService<ResponseHomologa002> cwsHomologa = new ConsumoWebService<ResponseHomologa002>();
                ConsumoWebService<RespuestaWS002> cws = new ConsumoWebService<RespuestaWS002>();
                APIVTA014002MessageResponse APIVTA014002Response = new APIVTA014002MessageResponse();
                
                

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
                            Logger.FileLogger("APIVTA014002", $"FUNCION WS HOMOLOGACION: Empresa No existe");
                            cont = i;
                            break;
                        }
                        if (ResuldatoHomologa != null && (ResuldatoHomologa.DescripcionId != ""))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            APIVTA014002Request.DataAreaId = responseHomologa;
                            Logger.FileLogger("APIVTA014002", $"FUNCION WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APIVTA014002", $"FUNCION WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                List<APCustSettlementIVTA014002> APCustInvoiceJourListCod = null;
                if (ResuldatoHomologa != null && !(string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                {

                    APIVTA014002Request.Enviroment = Entorno; //para mapear si es necesario
                    // asignar campo ambiente
                    APIVTA014002RequestW = new APIVTA014002MessageRequest();
                    APIVTA014002RequestW.DataAreaId = APIVTA014002Request.DataAreaId;
                    APIVTA014002RequestW.Enviroment = APIVTA014002Request.Enviroment;
                    APIVTA014002RequestW.APCustSettlemenList = new List<APCustSettlementIVTA014002>();
                    if (APIVTA014002RequestW.APCustSettlemenList != null)
                    {
                        foreach (var det in APIVTA014002Request.APCustSettlemenList)
                        {
                            tmp = new APCustSettlementIVTA014002();

                            tmp.AmountCuota = det.AmountCuota;
                            tmp.AmountRecaudo = det.AmountRecaudo;
                            tmp.BusinessUnit = det.BusinessUnit;
                            tmp.CustAccount = det.CustAccount;
                            tmp.DateRecaudo = det.DateRecaudo;
                            tmp.DueDate = det.DueDate;
                            tmp.InvoiceDate = det.InvoiceDate;
                            tmp.InvoiceId = det.InvoiceId;
                            tmp.MessageError = det.MessageError;
                            tmp.StatusRegister = det.StatusRegister;
                            tmp.Voucher = det.Voucher;
                            tmp.WorkSalesResponsibleCode = det.WorkSalesResponsibleCode;

                            APIVTA014002RequestW.APCustSettlemenList.Add(tmp);
                        }
                        APCustInvoiceJourListCod = APIVTA014002RequestW.APCustSettlemenList;

                        foreach (APCustSettlementIVTA014002 ListElem in APCustInvoiceJourListCod)
                        {
                            ListElem.CustAccount = CodigoCliente.DynamicAcrecos(ListElem.CustAccount, "APIVTA014002"); //para mapear si es necesario
                        }
                    }
                    responseLog = JsonConvert.SerializeObject(APIVTA014002Request);
                    Logger.FileLogger("APIVTA014002", "Request con código cliente No convertido: " + responseLog);
                    responseLog = JsonConvert.SerializeObject(APIVTA014002RequestW);
                    Logger.FileLogger("APIVTA014002", "Request con código cliente convertido: " + responseLog);

                    string jsonData = JsonConvert.SerializeObject(APIVTA014002RequestW);

                    for (int i = 1; i < vl_IntentosWS; i++)
                    {
                        try
                        {
                            objResponse = cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData);
                            if ((objResponse != null) && (objResponse.response != null))
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
                    Logger.FileLogger("APIVTA014002", $"FUNCION WS RegistrarRecaudosFA: Número de intentos realizados : {cont} En Ws RegistrarDismayorFA,Tiempo Transcurrido : {diff} ms.");

                    if (objResponse == null)
                    {
                        Logger.FileLogger("APIVTA014002", $"FUNCION WS RegistrarRecaudosFA: Error en el Servicio de RegistrarDismayorFA");
                        APIVTA014002Response.StatusId = false;
                        APIVTA014002Response.SessionId = sesionid;
                        APIVTA014002Response.ErrorList = new List<string>();
                        APIVTA014002Response.ErrorList.Add("Error  WS  RegistrarRecaudosFA: Error en el Servicio de  RegistrarDismayorFA");
                        APIVTA014002Response.APCustSettlementList = APIVTA014002Request.APCustSettlemenList;
                    }
                    else
                    {
                        // mapear la clase response del metodo con respuesta del WS
                        if (string.IsNullOrEmpty(objResponse.descripcionId) || string.IsNullOrWhiteSpace(objResponse.descripcionId))
                        { APIVTA014002Response.StatusId = false; }
                        else { APIVTA014002Response.StatusId = true; }

                        APIVTA014002Response.SessionId = sesionid;
                        APIVTA014002Response.ErrorList = objResponse.errorList;
                        APIVTA014002Response.APCustSettlementList = APIVTA014002Request.APCustSettlemenList;
                        


                    }

                }
                else
                {
                    Logger.FileLogger("APIVTA014002", $"FUNCION WS HOMOLOGACION: Error en el Servicio Homologación");
                    APIVTA014002Response.StatusId = false;
                    APIVTA014002Response.SessionId = sesionid;
                    APIVTA014002Response.ErrorList = new List<string>();
                    APIVTA014002Response.ErrorList.Add("Error  WS  Homologacion: Error en el Servicio Homologación");

                }

                responseLog = JsonConvert.SerializeObject(objResponse);
                Logger.FileLogger("APIVTA014002", "FUNCION: Resultado en ejecucion WS  RegistrarRecaudosFA: " + responseLog);
                IManejadorRequestQueue<APIVTA014002MessageResponse> _manejadorRequestQueue = new ManejadorRequestQueue<APIVTA014002MessageResponse>();
                await _manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolaresponse, APIVTA014002Response);
                responseLog = JsonConvert.SerializeObject(APIVTA014002Response);
                Logger.FileLogger("APIVTA014002", "Response a Dynamics : " + responseLog);

            }
            catch (Exception ex)
            {
                Logger.FileLogger("APIVTA014002", "Error por Exception: " + ex.Message);
                log.LogError($"Exception IVTA014002: {ex.Message}");
            }
        }
    }
}
