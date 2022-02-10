
using ICAJ008.api.Infraestructura.Configuracion;
using ICAJ008.api.Infraestructura.Servicios;
using ICAJ008.api.Models._001.Request;
using ICAJ008.api.Models._001.Response;
using ICAJ008.api.Models.Homologa;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ICAJ008.api.Controllers
{
    
    [ApiController]
    public class ICAJ008001Controller : ControllerBase
    {
        private IManejadorRequestQueue<APICAJ008001MessageRequest> manejadorRequestQueue;
        private IManejadorResponseQueue2<APICAJ008001MessageResponse> manejadorReponseQueue;
        private IHomologacionService<ResponseHomologa> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string Entorno = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        private static string sbConenctionStringEnvio = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest001").Value);
        private static string sbConenctionStringReceptar = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse001").Value);
        private static string nombrecolarequest = configuracion.GetSection("Data").GetSection("QueueRequest001").Value.ToString();
        private static string nombrecolaresponse = configuracion.GetSection("Data").GetSection("QueueResponse001").Value.ToString();
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep").Value);
        private static int vl_Intentos = Convert.ToInt32(configuracion.GetSection("Data").GetSection("IntentosREsponse").Value);
        private static int vl_TimeWS = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleepWS").Value);
        private static int vl_IntentosWS = Convert.ToInt32(configuracion.GetSection("Data").GetSection("IntentosWS").Value);
        private static int vl_TimeOutResp = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeOutResponse").Value);
        private static int longAllenar = Convert.ToInt32(configuracion.GetSection("Data").GetSection("LongAllenar").Value);
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();


        string respuestaLog;
        const int SegAMiliS = 1000;
        const int NanoAMiliS = 10000;

        public ICAJ008001Controller(IManejadorRequestQueue<APICAJ008001MessageRequest> _manejadorRequestQueue
            , IManejadorResponseQueue2<APICAJ008001MessageResponse> _manejadorReponseQueue
            , IHomologacionService<ResponseHomologa> _homologacionRequest)
        {
            // _logger = logger;
            manejadorRequestQueue = _manejadorRequestQueue;
            manejadorReponseQueue = _manejadorReponseQueue;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APICAJ008001")]
        [ServiceFilter(typeof(ValidationFilter001Attribute))]
        public async Task<ActionResult<APICAJ008001MessageResponse>> APICAJ008001(APICAJ008001MessageRequest parametrorequest)
        {

            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger("APICAJ008001", "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }




            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APICAJ008001MessageResponse respuesta = null;
            ResponseHomologa ResuldatoHomologa = null;
            APDocumentInvoiceLinesICAJ008001 APDocumentInvoiceLinesICAJ008001List = null;
            APDocumentInvoiceRequestProvisionNCList APDocumentInvoiceRequestProvisionNCListW = null;
            APDocumentInvoiceRequestLinesICAJ008001 APDocumentInvoiceRequestLinesICAJ008001List = null;
            APDocumentInvoiceTableICAJ008001 APDocumentInvoiceTableICAJ008001List = null;
            APDocumentInvoiceRequestTableICAJ008001 APDocumentInvoiceRequestTableICAJ008001List = null;
            APICAJ008001MessageRequest APICAJ008001MessageRequestW = new APICAJ008001MessageRequest();
            APICAJ008001MessageResponse APICAJ008001MessageResponseW = new APICAJ008001MessageResponse();
            List<APDocumentInvoiceTableICAJ008001> APDocumentInvoiceTableICAJ008001Cod = null;
            List<APDocumentInvoiceRequestTableICAJ008001> APDocumentInvoiceRequestTableICAJ008001Cod = null;
            ItemListResponse ItemListResponseW = null;

            try
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ008001", "CONTROLADOR: Request Recibido : " + respuestaLog);
                string DataAreaId = parametrorequest.DataAreaId;


                if (string.IsNullOrEmpty(DataAreaId) || string.IsNullOrWhiteSpace(DataAreaId))
                    throw new Exception("CONTROLADOR WS HOMOLOGACION: El código de empresa no debe ser nulo.");

                start = Stopwatch.GetTimestamp();
                int cont = 0;
                for (int i = 1; i < vl_IntentosWS; i++)
                {
                    try
                    {

                        ResuldatoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId);

                        // Validacion el objeto recibido
                        // Condicion para salir
                        if (ResuldatoHomologa != null && (string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                        {
                            respuesta = new APICAJ008001MessageResponse();
                            respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("ICAJ008:E000|EMPRESA NO EXISTE");
                            return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                        }
                        if (ResuldatoHomologa != null && "OK".Equals(ResuldatoHomologa.DescripcionId))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            parametrorequest.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICAJ008001", $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APICAJ008001", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICAJ008001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ008:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                }

                // asignar campo ambiente
                parametrorequest.Enviroment = Entorno;
                // asigna session
                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                // convertir codigo cliente crecos a Dynamics
                APICAJ008001MessageRequestW.DataAreaId = parametrorequest.DataAreaId;
                APICAJ008001MessageRequestW.SessionId= parametrorequest.SessionId;
                APICAJ008001MessageRequestW.Enviroment = parametrorequest.Enviroment;
                APICAJ008001MessageRequestW.APDocumentInvoiceTableICAJ008001 = new List<APDocumentInvoiceTableICAJ008001>();
                if (parametrorequest.APDocumentInvoiceTableICAJ008001 != null)
                {
                    foreach (var elem in parametrorequest.APDocumentInvoiceTableICAJ008001)
                    {
                        APDocumentInvoiceTableICAJ008001List = new APDocumentInvoiceTableICAJ008001();
                        APDocumentInvoiceTableICAJ008001List.CustAccount = elem.CustAccount;
                        APDocumentInvoiceTableICAJ008001List.SalesOrigin = elem.SalesOrigin;
                        APDocumentInvoiceTableICAJ008001List.SalesId = elem.SalesId;
                        APDocumentInvoiceTableICAJ008001List.InvoiceId = elem.InvoiceId;
                        APDocumentInvoiceTableICAJ008001List.InvoiceDate = elem.InvoiceDate;
                        APDocumentInvoiceTableICAJ008001List.NumberSecuence = elem.NumberSecuence;
                        APDocumentInvoiceTableICAJ008001List.DocumentDate = elem.DocumentDate;
                        APDocumentInvoiceTableICAJ008001List.PostingProfile = elem.PostingProfile;
                        APDocumentInvoiceTableICAJ008001List.DocumentInvoiceLinesList = new List<APDocumentInvoiceLinesICAJ008001>(); ;
                        if (elem.DocumentInvoiceLinesList != null)
                        {
                            foreach (var elem1 in elem.DocumentInvoiceLinesList)
                            {
                                APDocumentInvoiceLinesICAJ008001List = new APDocumentInvoiceLinesICAJ008001();
                                APDocumentInvoiceLinesICAJ008001List.ItemId = elem1.ItemId;
                                APDocumentInvoiceLinesICAJ008001List.Serial = elem1.Serial;
                                APDocumentInvoiceLinesICAJ008001List.Qty = elem1.Qty;
                               // APDocumentInvoiceLinesICAJ008001List.PostingProfile = elem1.PostingProfile;
                                APDocumentInvoiceTableICAJ008001List.DocumentInvoiceLinesList.Add(APDocumentInvoiceLinesICAJ008001List);
                            }
                        }
                        APICAJ008001MessageRequestW.APDocumentInvoiceTableICAJ008001.Add(APDocumentInvoiceTableICAJ008001List);
                    }
               
                APDocumentInvoiceTableICAJ008001Cod =  APICAJ008001MessageRequestW.APDocumentInvoiceTableICAJ008001;
                foreach (APDocumentInvoiceTableICAJ008001 ListElem in APDocumentInvoiceTableICAJ008001Cod)
                {
                    ListElem.CustAccount = CodigoCliente.CrecosAdynamics(ListElem.CustAccount, longAllenar, "APICAJ008001"); //para mapear si es necesario
                }
                }
                //

                await manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolarequest, APICAJ008001MessageRequestW);

                respuesta = await manejadorReponseQueue.RecibeMensajeSesion(sbConenctionStringReceptar, nombrecolaresponse, sesionid, vl_Time, vl_Intentos, "APICAJ008001", vl_TimeOutResp);

                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(APICAJ008001MessageRequestW);
                    Logger.FileLogger("APICAJ008001", "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger("APICAJ008001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICAJ008001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ008:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);  //StatusCode(StatusCodes.Status204NoContent, "No se retorno resultado de dynamics");
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(APICAJ008001MessageRequestW);
                    Logger.FileLogger("APICAJ008001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger("APICAJ008001", "CONTROLADOR: Response desde Dynamics antes convertir: " + respuestaLog);
                    // convertir codigo cliente Dynamics a Crecos
                    APICAJ008001MessageResponseW.SessionId = respuesta.SessionId;
                    APICAJ008001MessageResponseW.DataAreaId = respuesta.DataAreaId;
                    APICAJ008001MessageResponseW.StatusId = respuesta.StatusId;
                    APICAJ008001MessageResponseW.ErrorList = respuesta.ErrorList;
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
                                    APDocumentInvoiceRequestLinesICAJ008001List.ItemList =new List<ItemListResponse>();
                                    if (elem1.ItemList != null)
                                        foreach (var elem3 in elem1.ItemList)
                                        {
                                            ItemListResponseW = new ItemListResponse();
                                            ItemListResponseW.ItemId = elem3.ItemId;
                                            ItemListResponseW.Qty = elem3.Qty;
                                            ItemListResponseW.AmountLine= elem3.AmountLine;
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
                    }
                    //
                    respuestaLog = JsonConvert.SerializeObject(APICAJ008001MessageResponseW);
                    Logger.FileLogger("APICAJ008001", "CONTROLADOR: Response desde Dynamics despues convertir: " + respuestaLog);
                    return Ok(APICAJ008001MessageResponseW);
                }
            }
            catch (Exception ex)
            {

                respuestaLog = JsonConvert.SerializeObject(APICAJ008001MessageRequestW);
                respuesta = new APICAJ008001MessageResponse();
                respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICAJ008:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger("APICAJ008001", "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger("APICAJ008001", "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }
    }
}
