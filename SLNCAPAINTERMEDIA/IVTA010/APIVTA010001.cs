using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using IVTA010.Infraestructura.Configuracion;
using IVTA010.Infraestructura.Servicios;
using IVTA010.Models._001;
using IVTA010.Models._001.Request;
using IVTA010.Models._001.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static IVTA010.Infraestructura.Servicios.ManejadorRequestQueue;

namespace IVTA010
{
    public static class APIVTA010001

    {
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService");
        private static string sbMetodoWsUriConsumowebService1 = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService1");
        private static string nombrecolaresponse = Environment.GetEnvironmentVariable("QueueResponse001");
        private static string sbConenctionStringEnvio = Environment.GetEnvironmentVariable("ConectionStringResponse001");
        private static string sbUriConsumoWebServiceHomologa = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriConsumowebServiceHomologaAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string Entorno = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static int vl_IntentosWS = Convert.ToInt32(Environment.GetEnvironmentVariable("IntentosWS"));
        private static int vl_TimeWS = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleepWS"));
        private static string sbClaveWebService = Environment.GetEnvironmentVariable("Passwordws");
        private static string sbUsuarioWebService = Environment.GetEnvironmentVariable("Usuariodws");

        private static RegistroLog Logger = new RegistroLog();
        private static EncriptaDato Encriptar = new EncriptaDato();
        const string Signo  = "|";
        const string SignoL = "#";
        const string signoF = ";";
        private static  string sbClaveWebService1 = string.Empty;
        private static string sbUsuarioWebService1 = string.Empty;




        [FunctionName("APIVTA010001")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")] string myQueueItem, ILogger log)
        {
            
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                var APIVTA010001Request = JsonConvert.DeserializeObject<APIVTA010001MessageRequest>(myQueueItem);
                Logger.FileLogger("APIVTA010001", "Request Recibido de Dynamics : " + myQueueItem);
                //mapeo inicio texto
                List<APCustInvoiceJourIVTA010001> APCustInvoiceJourIVTA010001List = new List<APCustInvoiceJourIVTA010001>();
                APCustInvoiceJourIVTA010001List = APIVTA010001Request.APCustInvoiceJourList;
  
                string Registro = String.Empty;
                foreach (APCustInvoiceJourIVTA010001 Work in APCustInvoiceJourIVTA010001List)
                {
                    Registro += Work.APStoreId + Signo;
                    Registro += Work.InvoiceId + Signo;
                    Registro += Convert.ToDateTime(Work.InvoiceDate).ToString("yyyyMMdd") + Signo;
                    Registro += Work.CustAccount + Signo;
                    Registro += Work.VATNumCust + Signo;
                    Registro += Work.Email + Signo;
                    Registro += Work.CityId + Signo;
                    Registro += Work.VATNumEI + Signo;
                    Registro += Work.AmountIVA + Signo;
                    Registro += Work.PaymType + Signo;
                    Registro += Convert.ToString(Work.AmountSinIVA) + Signo;
                    Registro += Convert.ToString(Work.AmountSinIVA) + Signo;
                    foreach (APCustInvoiceTransIVTA010001 WorkInt in Work.CustInvoiceTrans)
                    {
                        Registro += WorkInt.ItemId + SignoL;
                        Registro += Convert.ToString(WorkInt.Qty) + SignoL;
                        Registro += WorkInt.ItemName + SignoL;
                        Registro += Convert.ToString(WorkInt.LineAmount) + SignoL;
                        Registro += WorkInt.InvoiceId + SignoL;
                    }
                    Registro = Registro.Remove(Registro.Length-1, 1);
                    Registro += signoF;
                }
               /* Registro = Registro.Replace("á", "a");
                Registro = Registro.Replace("ú", "u");
                Registro = Registro.Replace("ñ", "n");
                Registro = Registro.Replace("í", "i");
                Registro = Registro.Replace("ó", "o");
                Registro = Registro.Replace("é", "e"); */
                //fin mapeo para texto

                Logger.FileLogger("APIVTA010001", "Registro de Datos a Enviar : " + Registro);
                //Homologacion a Siac
                string DataAreaId = APIVTA010001Request.DataAreaId;
                //medir tiempo transcurrido en ws
                const int SegAMiliS = 1000;
                const int NanoAMiliS = 10000;
                long start = 0, end = 0;
                double diff = 0;
                ResponseHomologa001 ResuldatoHomologa = null;
                string responseHomologa = string.Empty;
                APIVTA010001MessageResponse APIVTA010001Response = new APIVTA010001MessageResponse();
                RespuestaWS objResponse = new RespuestaWS();
                ConsumoWebService<ResponseHomologa001> cwsHomologa = new ConsumoWebService<ResponseHomologa001>();
                start = Stopwatch.GetTimestamp();
                int cont = 0;
                // preguntar si el campo enviroment se llena con lo que se lee del SETTiNG
                // o con lo que nos envia del request, no se hace nada
                APIVTA010001Request.Enviroment = Entorno;

                string sesionid = APIVTA010001Request.SessionId.ToString();
                APIVTA010001Response.SessionId = sesionid;
                for (int i = 1; i < vl_IntentosWS; i++)
                 {
                     try
                     {
                         ResuldatoHomologa = await cwsHomologa.GetHomologacion(sbUriConsumoWebServiceHomologa, sbMetodoWsUriConsumowebServiceHomologaAx, DataAreaId);
                         // Validacion el objeto recibido
                         // Condicion para salir
                         if (ResuldatoHomologa != null && (string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                         {
                            Logger.FileLogger("APIVTA010001", $"FUNCION WS HOMOLOGACION: Empresa No existe");
                            cont = i;
                            break;
                            //return;
                         }
                         if (ResuldatoHomologa != null && (ResuldatoHomologa.DescripcionId != ""))
                        {
                             responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                             APIVTA010001Request.DataAreaId = responseHomologa;
                             Logger.FileLogger("APIVTA010001", $"FUNCION WS HOMOLOGACION: Código de empresa a enviarse a Legado es : {responseHomologa}");
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
                Logger.FileLogger("APIVTA010001", $"FUNCION WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa != null && !(string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                {


                    // ejecuta web service
                    ConsumoWebService2 cws = new ConsumoWebService2();
                    Request Parametros = new Request();
                    Parametros.datos = Registro;
                    Parametros.passwordws = sbClaveWebService;
                    Parametros.usuariows = sbUsuarioWebService;
                    sbClaveWebService1= Encriptar.Encriptar(sbClaveWebService);
                    sbUsuarioWebService1=Encriptar.Encriptar(sbUsuarioWebService);
                    Logger.FileLogger("APIVTA010001", $"FUNCION Encriptar password:" + Parametros.passwordws);
                    Logger.FileLogger("APIVTA010001", $"FUNCION Encriptar usuario:" + Parametros.usuariows);
                    Logger.FileLogger("APIVTA010001", $"FUNCION Encriptar password:" + sbClaveWebService1);
                    Logger.FileLogger("APIVTA010001", $"FUNCION Encriptar usuario:" + sbUsuarioWebService1);

                    string jsonData = JsonConvert.SerializeObject(Parametros);
                    start = Stopwatch.GetTimestamp();
                    
                    string url = sbUriConsumoWebService; //"http://104.131.164.242/configuracion/serverCrecos.php?wsdl";

                    //string xml = "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:serverCrecos\">";
                    string xml = "<soapenv:Envelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:" + sbMetodoWsUriConsumowebService + "\">";
                    xml = xml + "<soapenv:Header/>";
                    xml = xml + "<soapenv:Body>";
                    //xml = xml + "<urn:registraDocumentos soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">";
                    xml = xml + "<urn:" + sbMetodoWsUriConsumowebService1 + " soapenv:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">";
                    xml = xml + "<usuariows xsi:type=\"xsd: string\">" + sbUsuarioWebService1 + "</usuariows>";
                    xml = xml + "<passwordws xsi:type=\"xsd: string\">" + sbClaveWebService1 + "</passwordws>";
                    xml = xml + "<datos xsi:type=\"xsd: string\">" + Registro + "</datos>";
                    xml = xml + "</urn:registraDocumentos>";
                    xml = xml + "</soapenv:Body>";
                    xml = xml + "</soapenv:Envelope>";

                    cont = 0;
                    //RespuestaWS respuestaWS = null;
                    //for (int i = 1; i < vl_IntentosWS; i++)
                    
                    try
                    {
                        objResponse = cws.PostData(url, xml);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    await Task.Delay(vl_TimeWS);
                    
                    end = Stopwatch.GetTimestamp();
                    diff = start > 0 ? (end - start) / NanoAMiliS : 0;
                    Logger.FileLogger("APIVTA010001", $"FUNCION WS Netzen Registro Documento: Número de intentos realizados : {cont} En Ws Registra Documentos,Tiempo Transcurrido : {diff} ms.");

                    if (objResponse == null)
                    {
                        Logger.FileLogger("APIVTA010001", $"FUNCION WS Netzen Registro Documento: Error en el Servicio de Netzen Registro Documento");
                        APIVTA010001Response.StatusId = false;
                        APIVTA010001Response.ErrorList = new List<string>();
                        APIVTA010001Response.AprobList = new List<string>();
                        APIVTA010001Response.ErrorList.Add("Error en el Servicio de Netzen Registro Documento ");
                        //return;
                    }
                    else
                    {
                        APIVTA010001Response.StatusId = true ;
                        APIVTA010001Response.ErrorList = new List<string>();
                        APIVTA010001Response.AprobList = new List<string>();

                        if (objResponse.DetalleDocumentosNoGenerados != null && objResponse.DetalleDocumentosNoGenerados.Count > 0)
                        {
                            foreach (var elem in objResponse.DetalleDocumentosNoGenerados)
                            {
                                APIVTA010001Response.ErrorList.Add(elem);

                            }

                        }
                        if (objResponse.DetalleDocumentosSiGenerados != null && objResponse.DetalleDocumentosSiGenerados.Count > 0)
                        {
                            foreach (var elem in objResponse.DetalleDocumentosSiGenerados)
                            {
                                APIVTA010001Response.AprobList.Add(elem);

                            }

                        }
                    }
                }
                else
                {
                    Logger.FileLogger("APIVTA010001", $"FUNCION WS HOMOLOGACION: Error en el Servicio Homologación");
                    APIVTA010001Response.StatusId = false;
                    APIVTA010001Response.AprobList = new List<string>();
                    APIVTA010001Response.ErrorList = new List<string>();
                    APIVTA010001Response.ErrorList.Add("Error en Homologacion");

                }

                string responseLog = JsonConvert.SerializeObject(objResponse);
                Logger.FileLogger("APIVTA010001", "FUNCION: Resultado en ejecucion WS Netzen Registro Documento: " + responseLog);
                IManejadorRequestQueue<APIVTA010001MessageResponse> _manejadorRequestQueue = new ManejadorRequestQueue<APIVTA010001MessageResponse>();
                await _manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolaresponse, APIVTA010001Response);
                responseLog = JsonConvert.SerializeObject(APIVTA010001Response);
                Logger.FileLogger("APIVTA010001", "FUNCION: Respuesta a Dynamics : " + responseLog);
            }
            catch (Exception ex)
            {
                Logger.FileLogger("APIVTA010001", "FUNCION: Error por Exception: " + ex.Message);
                log.LogError($"Exception IVTA010001: {ex.Message}");
            }
        }
    }
}
    
    
