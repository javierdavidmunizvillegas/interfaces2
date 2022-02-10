using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using IVTA014.Infraestructura.Configuracion;
using IVTA014.Infraestructura.Servicios;
using IVTA014.Models._001.Request;
using IVTA014.Models._001.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IVTA014
{
    public static class IVTA014001
    {
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService001");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService001");
        public static string sbConenctionStringReceptar = Environment.GetEnvironmentVariable("ConectionStringRequest001");
        private static string nombrecolaresponse = Environment.GetEnvironmentVariable("QueueResponse001");
        private static string sbConenctionStringEnvio = Environment.GetEnvironmentVariable("ConectionStringResponse001");
        private static string sbUriConsumoWebServiceHomologa = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriConsumowebServiceHomologaAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string Entorno = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static int vl_IntentosWS = Convert.ToInt32(Environment.GetEnvironmentVariable("IntentosWS"));
        private static int vl_TimeWS = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleepWS"));
        private static RegistroLog Logger = new RegistroLog();
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32(Environment.GetEnvironmentVariable("LongAllenar"));  

        [FunctionName("IVTA014001")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")]string myQueueItem, ILogger log)
        {
            APCustInvoiceJourIVTA014001 apCustInvJourResp = null;
            APCustInvoiceTransIVTA014001 apCustInvTransResp = null;
            APIVTA014001MessageRequest APIVTA014001RequestW = null; // new APIVTA014001MessageRequestWork();
            APCustInvoiceJourIVTA014001 tmp = null;
            APCustInvoiceTransIVTA014001 tmp2 = null;
            string responseLog = null;
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                Logger.FileLogger("APIVTA014001", "Request Recibido de Dynamics : " + myQueueItem);
                var APIVTA014001Request = JsonConvert.DeserializeObject<APIVTA014001MessageRequest>(myQueueItem);
               

                if (string.IsNullOrEmpty(APIVTA014001Request.SessionId) || string.IsNullOrWhiteSpace(APIVTA014001Request.SessionId))
                {
                    Logger.FileLogger("APIVTA014001", $"FUNCION WS Valida campo: Session null, se asignará vacío");
                    APIVTA014001Request.SessionId = "";
                }
              

                //Homologacion a Siac
                string DataAreaId = APIVTA014001Request.DataAreaId;
                //medir tiempo transcurrido en ws
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;
                long start = 0, end = 0;
                double diff = 0;
                ResponseHomologa001 ResuldatoHomologa = null;
                RespuestaWS001 objResponse = new RespuestaWS001();
                string responseHomologa = string.Empty;
                string sesionid = APIVTA014001Request.SessionId.ToString();
                APIVTA014001RequestW = APIVTA014001Request;
                ConsumoWebService<ResponseHomologa001> cwsHomologa = new ConsumoWebService<ResponseHomologa001>();
                ConsumoWebService<RespuestaWS001> cws = new ConsumoWebService<RespuestaWS001>();
                APIVTA014001MessageResponse APIVTA014001Response = new APIVTA014001MessageResponse();

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
                            Logger.FileLogger("APIVTA014001", $"FUNCION WS HOMOLOGACION: Empresa No existe");
                            cont = i;
                            break;
                        }
                        if (ResuldatoHomologa != null && (ResuldatoHomologa.DescripcionId != ""))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            APIVTA014001Request.DataAreaId = responseHomologa;
                            Logger.FileLogger("APIVTA014001", $"FUNCION WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APIVTA014001", $"FUNCION WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                List<APCustInvoiceJourIVTA014001> APCustInvoiceJourListCod = null;
                if (ResuldatoHomologa != null && !(string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                {


                    // asignar campo ambiente
                    APIVTA014001Request.Enviroment = Entorno; //para mapear si es necesario

                    // Respaldar Request Copiar Variables
                    APIVTA014001RequestW = new APIVTA014001MessageRequest();
                    APIVTA014001RequestW.DataAreaId = APIVTA014001Request.DataAreaId;
                    APIVTA014001RequestW.Enviroment = APIVTA014001Request.Enviroment;
                    APIVTA014001RequestW.SessionId = APIVTA014001Request.SessionId;
                    APIVTA014001RequestW.APCustInvoiceJourList = new List<APCustInvoiceJourIVTA014001>();
                    if (APIVTA014001RequestW.APCustInvoiceJourList != null)
                    {
                        foreach (var det in APIVTA014001Request.APCustInvoiceJourList)
                        {
                            tmp = new APCustInvoiceJourIVTA014001();

                            tmp.InvoiceId = det.InvoiceId;
                            tmp.Voucher = det.Voucher;
                            tmp.InvoiceDate = det.InvoiceDate;
                            tmp.CustAccount = det.CustAccount;
                            tmp.BusinessUnit = det.BusinessUnit;
                            tmp.StatusRegister = det.StatusRegister;
                            tmp.MessageError = det.MessageError;
                            tmp.APCustInvoiceTransList = new List<APCustInvoiceTransIVTA014001>();
                            foreach (var det2 in det.APCustInvoiceTransList)
                            {
                                tmp2 = new APCustInvoiceTransIVTA014001();
                                tmp2.APCodeCapacity = det2.APCodeCapacity;
                                tmp2.APCodeGroup = det2.APCodeGroup;
                                tmp2.APCodeLines = det2.APCodeLines;
                                tmp2.APCodeSubGroup = det2.APCodeSubGroup;
                                tmp2.APPO = det2.APPO;
                                tmp2.APPVP = det2.APPVP;
                                tmp2.InvoiceDateReturn = det2.InvoiceDateReturn;
                                tmp2.InvoiceIdReturn = det2.InvoiceIdReturn;
                                tmp2.itemId = det2.itemId;
                                tmp2.LineAmount = det2.LineAmount;
                                tmp2.LineMargen = det2.LineMargen;
                                tmp2.PackingGroupId = det2.PackingGroupId;
                                tmp2.Payment = det2.Payment;
                                tmp2.PayMode = det2.PayMode;
                                tmp2.Qty = det2.Qty;
                                tmp2.NumLine = det2.NumLine;
                                tmp2.WorkSalesResponsibleCode = det2.WorkSalesResponsibleCode;
                                tmp2.WorkSalesResponsibleName = det2.WorkSalesResponsibleName;

                                tmp.APCustInvoiceTransList.Add(tmp2);
                            }
                            APIVTA014001RequestW.APCustInvoiceJourList.Add(tmp);
                        }
                        APCustInvoiceJourListCod = APIVTA014001RequestW.APCustInvoiceJourList;
                        foreach (APCustInvoiceJourIVTA014001 ListElem in APCustInvoiceJourListCod)
                        {
                            ListElem.CustAccount = CodigoCliente.DynamicAcrecos(ListElem.CustAccount, "APIVTA014001"); //para mapear si es necesario
                        }
                    }

                    responseLog = JsonConvert.SerializeObject(APIVTA014001Request);
                    Logger.FileLogger("APIVTA014001", "Request con código cliente No convertido: " + responseLog);
                    responseLog = JsonConvert.SerializeObject(APIVTA014001RequestW);
                    Logger.FileLogger("APIVTA014001", "Request con código cliente convertido: " + responseLog);

                    string jsonData = JsonConvert.SerializeObject(APIVTA014001RequestW);
                     
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
                    Logger.FileLogger("APIVTA014001", $"FUNCION WS RegistrarDismayorFA: Número de intentos realizados : {cont} En Ws RegistrarDismayorFA,Tiempo Transcurrido : {diff} ms.");

                    if (objResponse == null)
                    {
                        Logger.FileLogger("APIVTA014001", $"FUNCION WS RegistrarDismayorFA: Error en el Servicio de RegistrarDismayorFA");
                        APIVTA014001Response.StatusId = false;
                        APIVTA014001Response.SessionId = sesionid;
                        APIVTA014001Response.ErrorList = new List<string>();
                        APIVTA014001Response.ErrorList.Add("Error  WS  RegistrarDismayorFA: Error en el Servicio de  RegistrarDismayorFA");
                        APIVTA014001Response.APCustInvoiceJourList = APIVTA014001Request.APCustInvoiceJourList;
                    }
                    else
                    {
                        // mapear la clase response del metodo con respuesta del WS
                        if (string.IsNullOrEmpty(objResponse.descripcionId) || string.IsNullOrWhiteSpace(objResponse.descripcionId))
                        { APIVTA014001Response.StatusId = false; }
                        else { APIVTA014001Response.StatusId = true; }

                        APIVTA014001Response.SessionId = sesionid;
                        APIVTA014001Response.ErrorList = objResponse.errorList;
                        APIVTA014001Response.APCustInvoiceJourList = APIVTA014001Request.APCustInvoiceJourList;
                        /*
                        APIVTA014001Response.APCustInvoiceJourList = new List<APCustInvoiceJourIVTA014001>();
                        APCustInvoiceJourListCod = APIVTA014001Request.APCustInvoiceJourList;
                        foreach (APCustInvoiceJourIVTA014001 ListElem in APCustInvoiceJourListCod)
                        {
                            apCustInvJourResp = new APCustInvoiceJourIVTA014001();
                            // Llenar datos de apCustInvJourResp
                            ListElem.CustAccount = CodigoCliente.CrecosAdynamics(ListElem.CustAccount, 9, "APIVTA014001"); //para mapear si es necesario
                            apCustInvJourResp.InvoiceId = ListElem.InvoiceId;
                            apCustInvJourResp.Voucher = ListElem.Voucher;
                            apCustInvJourResp.InvoiceDate = ListElem.InvoiceDate;
                            apCustInvJourResp.CustAccount = CodigoCliente.CrecosAdynamics(ListElem.CustAccount, longAllenar, "APIVTA014001"); //para mapear si es necesario 
                            apCustInvJourResp.BusinessUnit = ListElem.BusinessUnit;
                            apCustInvJourResp.StatusRegister = ListElem.StatusRegister;
                            apCustInvJourResp.MessageError = ListElem.MessageError;
                            apCustInvJourResp.APCustInvoiceTransList = new List<APCustInvoiceTransIVTA014001>();

                            foreach (var det in ListElem.APCustInvoiceTransList)
                            {
                                apCustInvTransResp = new APCustInvoiceTransIVTA014001();

                                apCustInvTransResp.APCodeCapacity = det.APCodeCapacity;
                                apCustInvTransResp.APCodeGroup = det.APCodeGroup;
                                apCustInvTransResp.APCodeLines = det.APCodeLines;
                                apCustInvTransResp.APCodeSubGroup = det.APCodeSubGroup;
                                apCustInvTransResp.APPO = det.APPO;
                                apCustInvTransResp.APPVP = det.APPVP;
                                apCustInvTransResp.InvoiceDateReturn = det.InvoiceDateReturn;
                                apCustInvTransResp.InvoiceIdReturn = det.InvoiceIdReturn;
                                apCustInvTransResp.itemId = det.itemId;
                                apCustInvTransResp.LineAmount = det.LineAmount;
                                apCustInvTransResp.LineMargen = det.LineMargen;
                                apCustInvTransResp.PackingGroupId = det.PackingGroupId;
                                apCustInvTransResp.Payment = det.Payment;
                                apCustInvTransResp.PayMode = det.PayMode;
                                apCustInvTransResp.Qty = det.Qty;
                                apCustInvTransResp.WorkSalesResponsibleCode = det.WorkSalesResponsibleCode;
                                apCustInvTransResp.WorkSalesResponsibleName = det.WorkSalesResponsibleName;
                                apCustInvJourResp.APCustInvoiceTransList.Add(apCustInvTransResp);
                            }

                            APIVTA014001Response.APCustInvoiceJourList.Add(apCustInvJourResp);
                            
                        }*/


                    }

                }
                else 
                {
                    Logger.FileLogger("APIVTA014001", $"FUNCION WS HOMOLOGACION: Error en el Servicio Homologación");
                        APIVTA014001Response.StatusId = false;
                        APIVTA014001Response.SessionId = sesionid;
                        APIVTA014001Response.ErrorList = new List<string>();
                        APIVTA014001Response.ErrorList.Add("Error  WS  Homologacion: Error en el Servicio Homologación");

                }

                responseLog = JsonConvert.SerializeObject(objResponse);
                Logger.FileLogger("APIVTA014001", "FUNCION: Resultado en ejecucion WS  RegistrarDismayorFA: " + responseLog);
                IManejadorRequestQueue <APIVTA014001MessageResponse> _manejadorRequestQueue = new ManejadorRequestQueue<APIVTA014001MessageResponse>();
                await _manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolaresponse, APIVTA014001Response);
                responseLog = JsonConvert.SerializeObject(APIVTA014001Response);
                Logger.FileLogger("APIVTA014001", "Response a Dynamics : " + responseLog);

            }
            catch (Exception ex)
            {
                Logger.FileLogger("APIVTA014001", "Error por Exception: " + ex.Message);
                log.LogError($"Exception IVTA014001: {ex.Message}");
            }

        }
    }
}
