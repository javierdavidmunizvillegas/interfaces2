using ICOB004.api.Infraestructura.Configuracion;
using ICOB004.api.Infraestructura.Servicios;
using ICOB004.api.Models._001.Request;
using ICOB004.api.Models._002.Request;
using ICOB004.api.Models._002.Response;
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
    public class ICOB004002Controller : Controller
    {
        private IManejadorRequestQueue<APICOB004002MessageRequest> manejadorRequestQueue;
        private IManejadorResponseQueue2<APICOB004002MessageResponse> manejadorReponseQueue;
        private IConsumoWebService<ResponseHomologa> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string Entorno = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        private static string sbConenctionStringEnvio = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest002").Value);
        private static string sbConenctionStringReceptar = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse002").Value);
        private static string nombrecolarequest = configuracion.GetSection("Data").GetSection("QueueRequest002").Value.ToString();
        private static string nombrecolaresponse = configuracion.GetSection("Data").GetSection("QueueResponse002").Value.ToString();
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


        public ICOB004002Controller(IManejadorRequestQueue<APICOB004002MessageRequest> _manejadorRequestQueue
            , IManejadorResponseQueue2<APICOB004002MessageResponse> _manejadorReponseQueue
            , IConsumoWebService<ResponseHomologa> _homologacionRequest)
        {
            manejadorRequestQueue = _manejadorRequestQueue;
            manejadorReponseQueue = _manejadorReponseQueue;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APICOB004002")]
        [ServiceFilter(typeof(ValidationFilter002Attribute))]
        public async Task<ActionResult<APICOB004002MessageResponse>> APICOB004002(APICOB004002MessageRequest parametrorequest)
        {
            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger("APICOB004002", "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }

            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APICOB004002MessageResponse respuesta = null;
            ResponseHomologa ResuldatoHomologa = null;
            APICOB004002MessageRequest APICOB004002MessageRequestW = new APICOB004002MessageRequest();
            APDocumentLedgerRequest APDocumentLedgerRequestList = null;
            APICOB004002MessageResponse APICOB004002MessageResponseW = new APICOB004002MessageResponse();
            APDocumentLedgerResponse APDocumentLedgerResponseList = null;

            try
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICOB004002", "CONTROLADOR: Request Recibido : " + respuestaLog);
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
                            respuesta = new APICOB004002MessageResponse();
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
                            Logger.FileLogger("APICOB004002", $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APICOB004002", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICOB004002MessageResponse();
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

                APICOB004002MessageRequestW.DataAreaId = parametrorequest.DataAreaId;
                APICOB004002MessageRequestW.Enviroment = parametrorequest.Enviroment;
                APICOB004002MessageRequestW.SessionId = parametrorequest.SessionId;
                APICOB004002MessageRequestW.APTypeTransaction = parametrorequest.APTypeTransaction;
                APICOB004002MessageRequestW.DocumentLedgerList = new List<APDocumentLedgerRequest>();
                if (parametrorequest.DocumentLedgerList != null)

                    foreach (var elem in parametrorequest.DocumentLedgerList) 
                    {
                        APDocumentLedgerRequestList = new APDocumentLedgerRequest();
                        APDocumentLedgerRequestList.APIdentificationList = elem.APIdentificationList;
                        APDocumentLedgerRequestList.CustAccount = CodigoCliente.CrecosAdynamics(elem.CustAccount,longAllenar, "APICOB004002");
                        APDocumentLedgerRequestList.TransDate = elem.TransDate;
                        APDocumentLedgerRequestList.AmountDebit = elem.AmountDebit;
                        APDocumentLedgerRequestList.AmountCredit = elem.AmountCredit;
                        APDocumentLedgerRequestList.PostingProfile = elem.PostingProfile;
                        APDocumentLedgerRequestList.DocumentNegoc = elem.DocumentNegoc;
                        APICOB004002MessageRequestW.DocumentLedgerList.Add(APDocumentLedgerRequestList);
                    }
                respuestaLog = JsonConvert.SerializeObject(APICOB004002MessageRequestW);
                Logger.FileLogger("APICOB004002", "CONTROLADOR: Request Recibido Convertido : " + respuestaLog);


                await manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolarequest, APICOB004002MessageRequestW);

                respuesta = await manejadorReponseQueue.RecibeMensajeSesion(sbConenctionStringReceptar, nombrecolaresponse, sesionid, vl_Time, vl_Intentos, "APICOB004002", vl_TimeOutResp);
                if (respuesta != null) 
                {
                    APICOB004002MessageResponseW.SessionId = respuesta.SessionId;
                    APICOB004002MessageResponseW.StatusId = respuesta.StatusId;
                    APICOB004002MessageResponseW.ErrorList = respuesta.ErrorList;
                    if (APICOB004002MessageResponseW.ErrorList != null)
                    {
                        foreach (var elem in respuesta.ErrorList)
                        {
                            APICOB004002MessageResponseW.ErrorList.Add(elem);
                        }
                    }
                    APICOB004002MessageResponseW.APTypeTransaction = respuesta.APTypeTransaction;
                    APICOB004002MessageResponseW.DocumentList = new List<APDocumentLedgerResponse>();
                    if (respuesta.DocumentList != null)
                        foreach(var elem1 in respuesta.DocumentList)
                        {
                            APDocumentLedgerResponseList = new APDocumentLedgerResponse();
                            APDocumentLedgerResponseList.APIdentificationList = elem1.APIdentificationList;
                            APDocumentLedgerResponseList.Voucher = elem1.Voucher;
                            APDocumentLedgerResponseList.CustAccount = CodigoCliente.DynamicAcrecos(elem1.CustAccount, "APICOB004002");
                            APDocumentLedgerResponseList.TransDate = elem1.TransDate;
                            APDocumentLedgerResponseList.Amount = elem1.Amount;
                            APICOB004002MessageResponseW.DocumentList.Add(APDocumentLedgerResponseList);
                        }
                }
                

                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                    Logger.FileLogger("APICOB004002", "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger("APICOB004002", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICOB004002MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICOB004:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);  //StatusCode(StatusCodes.Status204NoContent, "No se retorno resultado de dynamics");
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                    Logger.FileLogger("APICOB004002", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(APICOB004002MessageResponseW);
                    Logger.FileLogger("APICOB004002", "CONTROLADOR: Response desde Dynamics: " + respuestaLog);

                    return Ok(APICOB004002MessageResponseW);
                }
            }
            catch (Exception ex)
            {

                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                respuesta = new APICOB004002MessageResponse();
                respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICOB004:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger("APICOB004002", "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger("APICOB004002", "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }
    }
}
