using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using APICAJ008002.Infraestructura.Configuracion;
using APICAJ008002.Infraestructura.Servicios;
using APICAJ008002.Models._002.Response;
using APICAJ008002.Models._003.Request;
using APICAJ008002.Models._003.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace APICAJ008002
{
    public static class APICAJ008003
    {
        const int SegAMiliS = 1000;
        const int NanoAMiliS = 10000;
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService003");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService003");
        public static string sbConenctionStringReceptar = Environment.GetEnvironmentVariable("ConectionStringRequest003");
        //private static string nombrecolaresponse = Environment.GetEnvironmentVariable("QueueResponse003");
        private static string sbConenctionStringEnvio = Environment.GetEnvironmentVariable("ConectionStringResponse003");
        private static string sbUriConsumoWebServiceHomologa = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriConsumowebServiceHomologaAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string Entorno = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static int vl_IntentosWS = Convert.ToInt32(Environment.GetEnvironmentVariable("IntentosWS03"));
        private static int vl_TimeWS = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleepWS"));
        //private static string respuesta_ws001;
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32(Environment.GetEnvironmentVariable("LongAllenar"));
        private static RegistroLog Logger = new RegistroLog();


        [FunctionName("APICAJ008003")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest003%", Connection = "ConectionStringRequest003")] string myQueueItem, ILogger log)
        {
            
            APDocumentInvoiceRequestProvisionNCList APDocumentInvoiceRequestProvisionNCListW = null;
            APDocumentInvoiceRequestLinesICAJ008001 APDocumentInvoiceRequestLinesICAJ008001List = null;
            APDocumentInvoiceRequestTableICAJ008001 APDocumentInvoiceRequestTableICAJ008001List = null;
            List<APDocumentInvoiceRequestTableICAJ008001> APDocumentInvoiceRequestTableICAJ008001Cod = null;
            ItemListResponse ItemListResponseW = null;
            ResponseHomologa003 ResuldatoHomologa = null;
            RespuestaWS003 objResponse = null;
            ConsumoWebService<RespuestaWS003> cws = new ConsumoWebService<RespuestaWS003>();

            ConsumoWebService<ResponseHomologa003> cwsHomologa = new ConsumoWebService<ResponseHomologa003>();
            string responseHomologa = string.Empty;
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                Logger.FileLogger("APICAJ008003", "Request Recibido de Dynamics :  " + myQueueItem);
                var respuesta = JsonConvert.DeserializeObject<APICAJ008003MessageResponse>(myQueueItem);
                string DataAreaId = respuesta.DataAreaId;

                int cont = 0;
                long start = 0, end = 0;
                double diff = 0;
                start = Stopwatch.GetTimestamp();
                for (int i = 1; i < vl_IntentosWS; i++)
                {
                    try
                    {
                        ResuldatoHomologa = await cwsHomologa.GetHomologacion(sbUriConsumoWebServiceHomologa, sbMetodoWsUriConsumowebServiceHomologaAx, DataAreaId);
                        // Validacion el objeto recibido
                        // Condicion para salir
                        if (ResuldatoHomologa != null && (string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                        {
                            Logger.FileLogger("APICAJ008003", $"FUNCION WS HOMOLOGACION: Empresa No existe");
                            cont = i;
                            break;
                        }
                        if (ResuldatoHomologa != null && (ResuldatoHomologa.DescripcionId != ""))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            respuesta.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICAJ008003", $"FUNCION WS HOMOLOGACION: Código de empresa a enviarse a Legado es : {responseHomologa}");
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
                Logger.FileLogger("APICAJ008003", $"FUNCION WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                //medir tiempo transcurrido en ws

                if (ResuldatoHomologa != null && !(string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                {
                               
                APICAJ008003MessageResponse APICAJ008001MessageResponseW = new APICAJ008003MessageResponse();
                start = Stopwatch.GetTimestamp();
                
                APICAJ008001MessageResponseW.DataAreaId = respuesta.DataAreaId;
                APICAJ008001MessageResponseW.DocumentInvoiceRequestTableList = new List<APDocumentInvoiceRequestTableICAJ008001>();
                if (respuesta.DocumentInvoiceRequestTableList != null)
                {


                    foreach (var elem in respuesta.DocumentInvoiceRequestTableList)
                    {
                        APDocumentInvoiceRequestTableICAJ008001List = new APDocumentInvoiceRequestTableICAJ008001();
                        APDocumentInvoiceRequestTableICAJ008001List.CustAccount = elem.CustAccount;
                        APDocumentInvoiceRequestTableICAJ008001List.SalesId = elem.SalesId;
                        APDocumentInvoiceRequestTableICAJ008001List.SalesIdAccount = elem.SalesIdAccount;
                        APDocumentInvoiceRequestTableICAJ008001List.Store = elem.Store;
                        APDocumentInvoiceRequestTableICAJ008001List.SalesOriginId = elem.SalesOriginId;
                        APDocumentInvoiceRequestTableICAJ008001List.PostingProfile = elem.PostingProfile;
                        APDocumentInvoiceRequestTableICAJ008001List.TotalAmount = elem.TotalAmount;
                        APDocumentInvoiceRequestTableICAJ008001List.DocumentInvoiceRequestLinesList = new List<APDocumentInvoiceRequestLinesICAJ008001>();
                        if (elem.DocumentInvoiceRequestLinesList != null)
                        {
                            foreach (var elem1 in elem.DocumentInvoiceRequestLinesList)
                            {
                                APDocumentInvoiceRequestLinesICAJ008001List = new APDocumentInvoiceRequestLinesICAJ008001();
                                APDocumentInvoiceRequestLinesICAJ008001List.Voucher = elem1.Voucher;
                                APDocumentInvoiceRequestLinesICAJ008001List.InvoiceId = elem1.InvoiceId;
                                APDocumentInvoiceRequestLinesICAJ008001List.SecuenciaFacturacion = elem1.SecuenciaFacturacion;
                                APDocumentInvoiceRequestLinesICAJ008001List.InvoiceDate = elem1.InvoiceDate;
                                APDocumentInvoiceRequestLinesICAJ008001List.TotalAmount = elem1.TotalAmount;
                                APDocumentInvoiceRequestLinesICAJ008001List.ItemList = new List<ItemListResponse>();
                                if (elem1.ItemList != null)
                                    foreach (var elem3 in elem1.ItemList)
                                    {
                                        ItemListResponseW = new ItemListResponse();
                                        ItemListResponseW.ItemId = elem3.ItemId;
                                        ItemListResponseW.Qty = elem3.Qty;
                                        ItemListResponseW.AmountLine = elem3.AmountLine;
                                        ItemListResponseW.OrdenItems = elem3.OrdenItems;
                                        APDocumentInvoiceRequestLinesICAJ008001List.ItemList.Add(ItemListResponseW);
                                    }
                                APDocumentInvoiceRequestTableICAJ008001List.DocumentInvoiceRequestLinesList.Add(APDocumentInvoiceRequestLinesICAJ008001List);
                            }
                        }
                        APDocumentInvoiceRequestTableICAJ008001List.DocumentInvoiceRequestProvisionNCList = new List<APDocumentInvoiceRequestProvisionNCList>();
                        if (elem.DocumentInvoiceRequestProvisionNCList != null)
                            foreach (var elem2 in elem.DocumentInvoiceRequestProvisionNCList)
                            {
                                APDocumentInvoiceRequestProvisionNCListW = new APDocumentInvoiceRequestProvisionNCList();
                                APDocumentInvoiceRequestProvisionNCListW.AmountNC = elem2.AmountNC;
                                APDocumentInvoiceRequestProvisionNCListW.InvoiceId = elem2.InvoiceId;
                                APDocumentInvoiceRequestProvisionNCListW.VoucherNC = elem2.VoucherNC;
                                APDocumentInvoiceRequestProvisionNCListW.VoucherProvision = elem2.VoucherProvision;
                                APDocumentInvoiceRequestProvisionNCListW.InvoiceDate = elem2.InvoiceDate;
                                APDocumentInvoiceRequestTableICAJ008001List.DocumentInvoiceRequestProvisionNCList.Add(APDocumentInvoiceRequestProvisionNCListW);

                            }

                        APICAJ008001MessageResponseW.DocumentInvoiceRequestTableList.Add(APDocumentInvoiceRequestTableICAJ008001List);
                    }

                    APDocumentInvoiceRequestTableICAJ008001Cod = APICAJ008001MessageResponseW.DocumentInvoiceRequestTableList;
                    if (APDocumentInvoiceRequestTableICAJ008001Cod != null)
                        foreach (APDocumentInvoiceRequestTableICAJ008001 ListElem in APDocumentInvoiceRequestTableICAJ008001Cod)
                        {
                            ListElem.CustAccount = CodigoCliente.DynamicAcrecos(ListElem.CustAccount, "APICAJ008001"); //para mapear si es necesario
                        }

                    
                    string jsonData = JsonConvert.SerializeObject(APICAJ008001MessageResponseW);
                    Logger.FileLogger("APICAJ008003", "jsonData:  " + jsonData);
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
                    Logger.FileLogger("APICAJ008003", $"FUNCION WS Flujo de Caja: Número de intentos realizados : {cont} En Ws Flujo de Caja ,Tiempo Transcurrido : {diff} ms.");

                    if (objResponse == null)
                    {
                        Logger.FileLogger("APICAJ008003", $"FUNCION WS Flujo de Caja: Error en el Servicio de Flujo de Caja");
                    }
                    

                }
                }
                else
                {
                    Logger.FileLogger("APICAJ008003", $"FUNCION WS HOMOLOGACION: Error en el Servicio Homologación");
                }
                
                string responseLog = JsonConvert.SerializeObject(objResponse);
                Logger.FileLogger("APICAJ008003", "FUNCION: Resultado en ejecucion WS Flujo de Caja: " + responseLog);
               
            }
            catch (Exception ex)
            {
                Logger.FileLogger("APICAJ008003", "Error por Exception: " + ex.Message);
                log.LogError($"Exception APICAJ008003:  {ex.Message}");
            }
        }
    }
}
