using ICOB003.api.Infraestructura.Configuracion;
using ICOB003.api.Infraestructura.Servicios;
using ICOB003.api.Models._001.Request;
using ICOB003.api.Models._001.Response;
using ICOB003.api.Models.Homologa;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB003.api.Controllers
{
    [ApiController]
    public class ICOB003001Controller : Controller
    {

        private IManejadorRequestQueue<APICOB003001MessageRequest> manejadorRequestQueue;
        private IManejadorResponseQueue2<APICOB003001MessageResponse> manejadorReponseQueue;
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
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32((configuracion.GetSection("Data").GetSection("LongAllenar").Value));



        string respuestaLog;
        const int SegAMiliS = 1000;
        const int NanoAMiliS = 10000;
        public ICOB003001Controller(IManejadorRequestQueue<APICOB003001MessageRequest> _manejadorRequestQueue
            , IManejadorResponseQueue2<APICOB003001MessageResponse> _manejadorReponseQueue
            , IHomologacionService<ResponseHomologa> _homologacionRequest)
        {
            manejadorRequestQueue = _manejadorRequestQueue;
            manejadorReponseQueue = _manejadorReponseQueue;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APICOB003001")]
        [ServiceFilter(typeof(ValidationFilter001Attribute))]
        public async Task<ActionResult<APICOB003001MessageResponse>> APICOB003001(APICOB003001MessageRequest parametrorequest)
        {


            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger("APICOB003001", "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }

            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APICOB003001MessageResponse respuesta = null;
            ResponseHomologa ResuldatoHomologa = null;
            APICOB003001MessageRequest APICOB003001MessageRequestW = new APICOB003001MessageRequest();
            APLedgerJournalReverse APLedgerJournalReverseW = null;
            APICOB003001MessageResponse APICOB003001MessageResponseW = new APICOB003001MessageResponse();

            try
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICOB003001", "CONTROLADOR: Request Recibido : " + respuestaLog);
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
                            respuesta = new APICOB003001MessageResponse();
                            //respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("ICOB003:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                            return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                        }
                        if (ResuldatoHomologa != null && "OK".Equals(ResuldatoHomologa.DescripcionId))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            parametrorequest.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICOB003001", $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APICOB003001", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICOB003001MessageResponse();
                    // respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICOB003:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                }

                // asignar campo ambiente
                parametrorequest.Enviroment = Entorno;
                // asigna session
                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                //convertir
                APICOB003001MessageRequestW.DataAreaId = parametrorequest.DataAreaId;
                APICOB003001MessageRequestW.Enviroment = parametrorequest.Enviroment;
                APICOB003001MessageRequestW.SessionId = parametrorequest.SessionId;
                APICOB003001MessageRequestW.APLedgerJournalRevese = new APLedgerJournalReverse();
                if (parametrorequest.APLedgerJournalRevese != null)
                {
                   // foreach (var elem in parametrorequest.APLedgerJournalRevese) 
                   // {
                        APLedgerJournalReverseW = new APLedgerJournalReverse();
                        APLedgerJournalReverseW.VoucherSettlement = parametrorequest.APLedgerJournalRevese.VoucherSettlement;
                        APLedgerJournalReverseW.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.APLedgerJournalRevese.CustAccount, longAllenar, "APICOB003001");
                        APLedgerJournalReverseW.DateTrans = parametrorequest.APLedgerJournalRevese.DateTrans;
                        APLedgerJournalReverseW.ReasonCode = parametrorequest.APLedgerJournalRevese.ReasonCode;
                        APLedgerJournalReverseW.Amount = parametrorequest.APLedgerJournalRevese.Amount;
                   
                    APICOB003001MessageRequestW.APLedgerJournalRevese=APLedgerJournalReverseW;

                   // }
                }
                string responseLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICOB003001", "Request con código cliente No convertido: " + responseLog);
                responseLog = JsonConvert.SerializeObject(APICOB003001MessageRequestW);
                Logger.FileLogger("APICOB003001", "Request con código cliente convertido: " + responseLog);

                //

                await manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolarequest, APICOB003001MessageRequestW);

                respuesta = await manejadorReponseQueue.RecibeMensajeSesion(sbConenctionStringReceptar, nombrecolaresponse, sesionid, vl_Time, vl_Intentos, "APICOB003001", vl_TimeOutResp);
                
                

                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(APICOB003001MessageRequestW);
                    Logger.FileLogger("APICOB003001", "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger("APICOB003001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICOB003001MessageResponse();
                   // respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICOB003:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);  //StatusCode(StatusCodes.Status204NoContent, "No se retorno resultado de dynamics");
                }
                else
                {
                    APICOB003001MessageResponseW.APIdentificationList = respuesta.APIdentificationList;
                    APICOB003001MessageResponseW.VoucherReverse = respuesta.VoucherReverse;
                    if (respuesta.CustAccount != null)
                    APICOB003001MessageResponseW.CustAccount = CodigoCliente.DynamicAcrecos(respuesta.CustAccount, "APICOB003001");
                    APICOB003001MessageResponseW.Amount = respuesta.Amount;
                    APICOB003001MessageResponseW.VoucherOriginal = respuesta.VoucherOriginal;
                    APICOB003001MessageResponseW.StatusId = respuesta.StatusId;
                    APICOB003001MessageResponseW.ErrorList = new List<string>();
                    if (respuesta.ErrorList != null)
                    {
                        foreach (var elem1 in respuesta.ErrorList)
                        {
                            APICOB003001MessageResponseW.ErrorList.Add(elem1);
                        }
                    }
                    respuestaLog = JsonConvert.SerializeObject(APICOB003001MessageRequestW);
                    Logger.FileLogger("APICOB003001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger("APICOB003001", "CONTROLADOR: Response desde Dynamics: " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(APICOB003001MessageResponseW);
                    Logger.FileLogger("APICOB003001", "CONTROLADOR: Response desde Dynamics, Convertida : " + respuestaLog);


                    return Ok(APICOB003001MessageResponseW);
                }
            }
            catch (Exception ex)
            {

                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                respuesta = new APICOB003001MessageResponse();
               // respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICOB003:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger("APICOB003001", "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger("APICOB003001", "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }



    }
}
