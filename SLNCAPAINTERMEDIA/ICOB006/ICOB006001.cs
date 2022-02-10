using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using ICOB006.Infraestructura.Configuracion;
using ICOB006.Infraestructura.Servicios;
using ICOB006.Models._001.Request;
using ICOB006.Models._001.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ICOB006
{
    public static class ICOB006001
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
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32(Environment.GetEnvironmentVariable("LongAllenar"));


        [FunctionName("ICOB006001")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")]string myQueueItem, ILogger log)
        {
           
            APICOB006001MessageRequest APICOB006001MessageRequestW = null;  
            APInvoiceListRequest tmp = null;
            APDocumentTaxRequest tmp2 = null;
            string responseLog = null;
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                Logger.FileLogger("APICOB006001", "Request Recibido de Dynamics : " + myQueueItem);
                var APICOB006001Request = JsonConvert.DeserializeObject<APICOB006001MessageRequest>(myQueueItem);


                if (string.IsNullOrEmpty(APICOB006001Request.SessionId) || string.IsNullOrWhiteSpace(APICOB006001Request.SessionId))
                {
                    Logger.FileLogger("APICOB006001", $"FUNCION WS Valida campo: Session null, se asignará vacío");
                    APICOB006001Request.SessionId = "";
                }


                //Homologacion a Siac
                string DataAreaId = APICOB006001Request.DataAreaId;
                //medir tiempo transcurrido en ws
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;
                long start = 0, end = 0;
                double diff = 0;
                ResponseHomologa001 ResuldatoHomologa = null;
                RespuestaWS001 objResponse = new RespuestaWS001();
                string responseHomologa = string.Empty;
                string sesionid = APICOB006001Request.SessionId.ToString();
                APICOB006001MessageRequestW = APICOB006001Request;
                ConsumoWebService<ResponseHomologa001> cwsHomologa = new ConsumoWebService<ResponseHomologa001>();
                ConsumoWebService<RespuestaWS001> cws = new ConsumoWebService<RespuestaWS001>();
                APICOB006001MessageResponse APICOB006001Response = new APICOB006001MessageResponse();

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
                            Logger.FileLogger("APICOB006001", $"FUNCION WS HOMOLOGACION: Empresa No existe");
                            cont = i;
                            break;
                        }
                        if (ResuldatoHomologa != null && (ResuldatoHomologa.DescripcionId != ""))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            APICOB006001Request.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICOB006001", $"FUNCION WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APICOB006001", $"FUNCION WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

              //  List<APCustInvoiceJourIVTA014001> APCustInvoiceJourListCod = null;
                if (ResuldatoHomologa != null && !(string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                {


                    // asignar campo ambiente
                    APICOB006001Request.Enviroment = Entorno; //para mapear si es necesario

                    // Respaldar Request Copiar Variables
                    APICOB006001MessageRequestW = new APICOB006001MessageRequest();
                    APICOB006001MessageRequestW.DataAreaId = APICOB006001Request.DataAreaId;
                    APICOB006001MessageRequestW.Enviroment = APICOB006001Request.Enviroment;
                    APICOB006001MessageRequestW.SessionId = APICOB006001Request.SessionId;
                    if (APICOB006001Request.CustAccount != null)
                        APICOB006001MessageRequestW.CustAccount = CodigoCliente.DynamicAcrecos(APICOB006001Request.CustAccount, "APICOB006001"); 
                    APICOB006001MessageRequestW.JournalNumPaym = APICOB006001Request.JournalNumPaym;
                    APICOB006001MessageRequestW.VoucherPaym = APICOB006001Request.VoucherPaym;
                    APICOB006001MessageRequestW.DocumentNum = APICOB006001Request.DocumentNum;
                    APICOB006001MessageRequestW.AmountCheque = APICOB006001Request.AmountCheque;
                    APICOB006001MessageRequestW.NumCheque = APICOB006001Request.NumCheque;
                    APICOB006001MessageRequestW.NumCtaCheque = APICOB006001Request.NumCtaCheque;
                    APICOB006001MessageRequestW.BankCheque = APICOB006001Request.BankCheque;
                    APICOB006001MessageRequestW.MotiveCheque = APICOB006001Request.MotiveCheque;
                    APICOB006001MessageRequestW.DateProtest = APICOB006001Request.DateProtest;
                    if (APICOB006001Request.DocumentTax != null)
                    {
                        APICOB006001MessageRequestW.DocumentTax = new APDocumentTaxRequest();
                        APICOB006001MessageRequestW.DocumentTax.InvoiceTax = APICOB006001Request.DocumentTax.InvoiceTax;
                        APICOB006001MessageRequestW.DocumentTax.VoucherTax = APICOB006001Request.DocumentTax.VoucherTax;
                        APICOB006001MessageRequestW.DocumentTax.AmountTax = APICOB006001Request.DocumentTax.AmountTax;
                        APICOB006001MessageRequestW.DocumentTax.DateTax = APICOB006001Request.DocumentTax.DateTax;
                        APICOB006001MessageRequestW.DocumentTax.DescriptionTax = APICOB006001Request.DocumentTax.DescriptionTax;
                        APICOB006001MessageRequestW.DocumentTax.NumChequeTax = APICOB006001Request.DocumentTax.NumChequeTax;
                    }
                    
                    if (APICOB006001Request.InvoiceList != null)
                    {
                        APICOB006001MessageRequestW.InvoiceList = new List<APInvoiceListRequest>();
                        foreach (var det in APICOB006001Request.InvoiceList)
                        {
                            tmp = new APInvoiceListRequest();

                            tmp.InvoiceId = det.InvoiceId;
                            tmp.Voucher = det.Voucher;
                            tmp.AmountInvoice = det.AmountInvoice;

                            APICOB006001MessageRequestW.InvoiceList.Add(tmp);
                        }

                       /* APCustInvoiceJourListCod = APICOB006001MessageRequestW.APCustInvoiceJourList;
                        foreach (APCustInvoiceJourIVTA014001 ListElem in APCustInvoiceJourListCod)
                        {
                            ListElem.CustAccount = CodigoCliente.DynamicAcrecos(ListElem.CustAccount, "APICOB006001"); //para mapear si es necesario
                        }*/
                    }

                    responseLog = JsonConvert.SerializeObject(APICOB006001Request);
                    Logger.FileLogger("APICOB006001", "Request con código cliente No convertido: " + responseLog);
                    responseLog = JsonConvert.SerializeObject(APICOB006001MessageRequestW);
                    Logger.FileLogger("APICOB006001", "Request con código cliente convertido: " + responseLog);

                    string jsonData = JsonConvert.SerializeObject(APICOB006001MessageRequestW);

                    for (int i = 1; i < vl_IntentosWS; i++)
                    {
                        try
                        {
                            objResponse = cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData);
                            if (objResponse != null)  //&& (objResponse.response != null))
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
                    Logger.FileLogger("APICOB006001", $"FUNCION WS Registra Cheque Protesatado: Número de intentos realizados : {cont} En Ws Registra Cheque Protestado,Tiempo Transcurrido : {diff} ms.");
                    
                    if (objResponse == null)
                    {
                        Logger.FileLogger("APICOB006001", $"FUNCION WS Registra Cheque Protesatado: Error en el Servicio de Registra Cheque Protestado");
                        APICOB006001Response.DescriptionId = false;
                        APICOB006001Response.SessionId = sesionid;
                        APICOB006001Response.ErrorList = new List<string>();
                        APICOB006001Response.ErrorList.Add("Error  WS  Registra Cheque Protesatado: Error en el Servicio de  Registra Cheque Protestado");
                        APICOB006001Response.NumCheque = "";
                    }
                    else
                    {
                        // mapear la clase response del metodo con respuesta del WS
                        if (objResponse.estadoTransaccion == "OK")
                        { APICOB006001Response.DescriptionId = true; }
                        else { APICOB006001Response.DescriptionId = false; }
                        APICOB006001Response.SessionId = sesionid;
                        APICOB006001Response.Response = objResponse.descripcionTransaccion;
                        APICOB006001Response.NumCheque = objResponse.numeroCheque;
                        
                    }

                }
                else
                {
                    Logger.FileLogger("APICOB006001", $"FUNCION WS HOMOLOGACION: Error en el Servicio Homologación");
                    APICOB006001Response.DescriptionId = false;
                    APICOB006001Response.SessionId = sesionid;
                    APICOB006001Response.ErrorList = new List<string>();
                    APICOB006001Response.ErrorList.Add("Error  WS  Homologacion: Error en el Servicio Homologación");

                }

                responseLog = JsonConvert.SerializeObject(objResponse);
                Logger.FileLogger("APICOB006001", "FUNCION: Resultado en ejecucion WS  Cheque Protesatado: " + responseLog);
                IManejadorRequestQueue<APICOB006001MessageResponse> _manejadorRequestQueue = new ManejadorRequestQueue<APICOB006001MessageResponse>();
                await _manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolaresponse, APICOB006001Response);
                responseLog = JsonConvert.SerializeObject(APICOB006001Response);
                Logger.FileLogger("APICOB006001", "Response a Dynamics : " + responseLog);

            }
            catch (Exception ex)
            {
                Logger.FileLogger("APICOB006001", "Error por Exception: " + ex.Message);
                log.LogError($"Exception ICOB006001: {ex.Message}");
            }

        }
    }
}
