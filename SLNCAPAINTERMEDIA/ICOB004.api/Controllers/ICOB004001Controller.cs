using ICOB004.api.Infraestructura.Configuracion;
using ICOB004.api.Infraestructura.Servicios;
using ICOB004.api.Models._001.Request;
using ICOB004.api.Models._001.Response;
using ICOB004.api.Models.Homologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static ICOB004.api.Validator;

namespace ICOB004.api.Controllers
{
    [ApiController]
    public class ICOB004001Controller : Controller
    {
        private IManejadorRequestQueue<APICOB004001MessageRequest> manejadorRequestQueue;
        private IManejadorResponseQueue2<APICOB004001MessageResponse> manejadorReponseQueue;
        private IConsumoWebService<ResponseHomologa> homologacionRequest;
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
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32(configuracion.GetSection("Data").GetSection("LongAllenar").Value);


        string respuestaLog;
        const int SegAMiliS = 1000;
        const int NanoAMiliS = 10000;

        public ICOB004001Controller(IManejadorRequestQueue<APICOB004001MessageRequest> _manejadorRequestQueue
            , IManejadorResponseQueue2<APICOB004001MessageResponse> _manejadorReponseQueue
            , IConsumoWebService<ResponseHomologa> _homologacionRequest)
        {
            manejadorRequestQueue = _manejadorRequestQueue;
            manejadorReponseQueue = _manejadorReponseQueue;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APICOB004001")]
        [ServiceFilter(typeof(ValidationFilter001Attribute))]
        public async Task<ActionResult<APICOB004001MessageResponse>> APICOB004001(APICOB004001MessageRequest parametrorequest)
        {
            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger("APICOB004001", "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }

            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APICOB004001MessageResponse respuesta = null;
            ResponseHomologa ResuldatoHomologa = null;
            APICOB004001MessageRequest APICOB004001MessageRequestW = new APICOB004001MessageRequest();
            APICOB004001MessageResponse APICOB004001MessageResponseW = new APICOB004001MessageResponse();
            APDocumentNCRequest APDocumentNCRequestList = null;
            APCreditNoteResponse APCreditNoteResponseList = null;

            try
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICOB004001", "CONTROLADOR: Request Recibido : " + respuestaLog);
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
                            respuesta = new APICOB004001MessageResponse();
                            respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("ICOB004:E000|EMPRESA NO EXISTE");
                            return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                        }
                        if (ResuldatoHomologa != null && "OK".Equals(ResuldatoHomologa.DescripcionId))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            parametrorequest.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICOB004001", $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APICOB004001", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICOB004001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICOB004:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                }

                // asignar campo ambiente
                parametrorequest.Enviroment = Entorno;

                // asigna session
                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                APICOB004001MessageRequestW.DataAreaId = parametrorequest.DataAreaId;
                APICOB004001MessageRequestW.Enviroment = parametrorequest.Enviroment;
                APICOB004001MessageRequestW.SessionId = parametrorequest.SessionId;
                APICOB004001MessageRequestW.DocumentNCList = new List<APDocumentNCRequest>();
                if (parametrorequest.DocumentNCList != null)
                foreach (var elem in parametrorequest.DocumentNCList)
                {
                    APDocumentNCRequestList = new APDocumentNCRequest();
                    APDocumentNCRequestList.APIdentificationList = elem.APIdentificationList;
                    APDocumentNCRequestList.CustAccount = CodigoCliente.CrecosAdynamics(elem.CustAccount, longAllenar, "APICOB004001");
                    APDocumentNCRequestList.TransDate = elem.TransDate;
                    APDocumentNCRequestList.NumberSequenceGroup = elem.NumberSequenceGroup;
                    APDocumentNCRequestList.DocumentApplies = elem.DocumentApplies;
                    APDocumentNCRequestList.PostingProfile = elem.PostingProfile;
                    APDocumentNCRequestList.APRubroSIAC = elem.APRubroSIAC;
                    APDocumentNCRequestList.Qty = elem.Qty;
                    APDocumentNCRequestList.PriceUnit = elem.PriceUnit;
                    APDocumentNCRequestList.Neto = elem.Neto;
                    APDocumentNCRequestList.MotiveNC = elem.MotiveNC;
                        APDocumentNCRequestList.TypeOfDocument = elem.TypeOfDocument;
                        APDocumentNCRequestList.OutsideSytem = elem.OutsideSytem;
                        APICOB004001MessageRequestW.DocumentNCList.Add(APDocumentNCRequestList);
                }
           
                await manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolarequest, APICOB004001MessageRequestW);
                respuestaLog = JsonConvert.SerializeObject(APICOB004001MessageRequestW);
                Logger.FileLogger("APICOB004001", "CONTROLADOR: Request Recibido Convertido : " + respuestaLog);
                
                respuesta = await manejadorReponseQueue.RecibeMensajeSesion(sbConenctionStringReceptar, nombrecolaresponse, sesionid, vl_Time, vl_Intentos, "APICOB004001", vl_TimeOutResp);

                // convierte
                if (respuesta != null)
                {
                    APICOB004001MessageResponseW.SessionId = respuesta.SessionId;
                    APICOB004001MessageResponseW.StatusId = respuesta.StatusId;
                    APICOB004001MessageResponseW.ErrorList = new List<string>();
                    if (APICOB004001MessageResponseW.ErrorList != null)
                    {
                        foreach (var elem in respuesta.ErrorList)
                        {
                            APICOB004001MessageResponseW.ErrorList.Add(elem);
                        }
                    }
                    APICOB004001MessageResponseW.CreditNoteList = new List<APCreditNoteResponse>();
                    if (respuesta.CreditNoteList != null)
                    {
                        foreach (var elem1 in respuesta.CreditNoteList)
                        {
                            APCreditNoteResponseList = new APCreditNoteResponse();
                            APCreditNoteResponseList.APIdentificationList = elem1.APIdentificationList;
                            APCreditNoteResponseList.DocumentNum = elem1.DocumentNum;
                            if (elem1.CustAccount != null)
                            APCreditNoteResponseList.CustAccount = CodigoCliente.DynamicAcrecos(elem1.CustAccount, "APICOB004001");
                            APCreditNoteResponseList.DocumentDate = elem1.DocumentDate;
                            APCreditNoteResponseList.DocumentApply = elem1.DocumentApply;
                            APCreditNoteResponseList.Amount = elem1.Amount;
                           // APCreditNoteResponseList.Voucher = elem1.Voucher;
                            APCreditNoteResponseList.Asiento = elem1.Asiento;
                            APICOB004001MessageResponseW.CreditNoteList.Add(APCreditNoteResponseList);
                        }

                        
                    }
                }
                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(APICOB004001MessageRequestW);
                    Logger.FileLogger("APICOB004001", "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger("APICOB004001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICOB004001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICOB004:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);  //StatusCode(StatusCodes.Status204NoContent, "No se retorno resultado de dynamics");
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(APICOB004001MessageRequestW);
                    Logger.FileLogger("APICOB004001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(APICOB004001MessageResponseW);
                    Logger.FileLogger("APICOB004001", "CONTROLADOR: Response desde Dynamics: " + respuestaLog);

                    return Ok(APICOB004001MessageResponseW);
                }
            }
            catch (Exception ex)
            {

                respuestaLog = JsonConvert.SerializeObject(APICOB004001MessageRequestW);
                respuesta = new APICOB004001MessageResponse();
                respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICOB004:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger("APICOB004001", "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger("APICOB004001", "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }
    }
}
