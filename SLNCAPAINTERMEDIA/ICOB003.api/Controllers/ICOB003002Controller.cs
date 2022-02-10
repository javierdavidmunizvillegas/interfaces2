using ICOB003.api.Infraestructura.Configuracion;
using ICOB003.api.Infraestructura.Servicios;
using ICOB003.api.Models._002.Request;
using ICOB003.api.Models._002.Response;
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
    public class ICOB003002Controller : Controller
    {
        private IManejadorRequestQueue<APICOB003002MessageRequest> manejadorRequestQueue;
        private IManejadorResponseQueue2<APICOB003002MessageResponse> manejadorReponseQueue;
        private IHomologacionService<ResponseHomologa> homologacionRequest;
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
        private static int longAllenar = Convert.ToInt32((configuracion.GetSection("Data").GetSection("LongAllenar").Value));



        string respuestaLog;
        const int SegAMiliS = 1000;
        const int NanoAMiliS = 10000;
        public ICOB003002Controller(IManejadorRequestQueue<APICOB003002MessageRequest> _manejadorRequestQueue
            , IManejadorResponseQueue2<APICOB003002MessageResponse> _manejadorReponseQueue
            , IHomologacionService<ResponseHomologa> _homologacionRequest)
        {
            manejadorRequestQueue = _manejadorRequestQueue;
            manejadorReponseQueue = _manejadorReponseQueue;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APICOB003002")]
        [ServiceFilter(typeof(ValidationFilter002Attribute))]
        public async Task<ActionResult<APICOB003002MessageResponse>> APICOB003002(APICOB003002MessageRequest parametrorequest)
        {


            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger("APICOB003002", "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }

            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APICOB003002MessageResponse respuesta = null;
            ResponseHomologa ResuldatoHomologa = null;
            APCustInvoiceTable APCustInvoiceTableW = null;
            APICOB003002MessageRequest APICOB003002MessageRequestW = new APICOB003002MessageRequest();
            try
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICOB003002", "CONTROLADOR: Request Recibido : " + respuestaLog);
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
                            respuesta = new APICOB003002MessageResponse();
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
                            Logger.FileLogger("APICOB003002", $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APICOB003002", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICOB003002MessageResponse();
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
                // convertir dynamisc a crecos
                APICOB003002MessageRequestW.DataAreaId = parametrorequest.DataAreaId;
                APICOB003002MessageRequestW.Enviroment = parametrorequest.Enviroment;
                APICOB003002MessageRequestW.SessionId = parametrorequest.SessionId;
                APICOB003002MessageRequestW.APCustInvoiceTableList = new List<APCustInvoiceTable>();
                if (parametrorequest.APCustInvoiceTableList != null)
                {
                    foreach (var elem in parametrorequest.APCustInvoiceTableList)
                    {
                        APCustInvoiceTableW = new APCustInvoiceTable();
                        APCustInvoiceTableW.RubroSIAC = elem.RubroSIAC;
                        APCustInvoiceTableW.APIdentificationList = elem.APIdentificationList;
                        APCustInvoiceTableW.CustAccount = CodigoCliente.CrecosAdynamics(elem.CustAccount,longAllenar, "APICOB003002");
                        APCustInvoiceTableW.DateTrans = elem.DateTrans;
                        APCustInvoiceTableW.NumberSequenceGroupId = elem.NumberSequenceGroupId;
                        APCustInvoiceTableW.Qty = elem.Qty;
                        APCustInvoiceTableW.PriceUnit = elem.PriceUnit;
                        APCustInvoiceTableW.Amount = elem.Amount;
                        APCustInvoiceTableW.MotiveId = elem.MotiveId;
                        APCustInvoiceTableW.DocumentOrig = elem.DocumentOrig;
                        APCustInvoiceTableW.OutsideSytem = elem.OutsideSytem;
                        APCustInvoiceTableW.TypeOfDocument = elem.TypeOfDocument;
                        APICOB003002MessageRequestW.APCustInvoiceTableList.Add(APCustInvoiceTableW);

                    }

                }

                //
                string responseLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICOB003002", "Request con código cliente No convertido: " + responseLog);
                responseLog = JsonConvert.SerializeObject(APICOB003002MessageRequestW);
                Logger.FileLogger("APICOB003002", "Request con código cliente convertido: " + responseLog);

                await manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolarequest, APICOB003002MessageRequestW);

                respuesta = await manejadorReponseQueue.RecibeMensajeSesion(sbConenctionStringReceptar, nombrecolaresponse, sesionid, vl_Time, vl_Intentos, "APICOB003002", vl_TimeOutResp);

                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(APICOB003002MessageRequestW);
                    Logger.FileLogger("APICOB003002", "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger("APICOB003002", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICOB003002MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICOB003:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);  //StatusCode(StatusCodes.Status204NoContent, "No se retorno resultado de dynamics");
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(APICOB003002MessageRequestW);
                    Logger.FileLogger("APICOB003002", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger("APICOB003002", "CONTROLADOR: Response desde Dynamics: " + respuestaLog);

                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {

                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                respuesta = new APICOB003002MessageResponse();
                respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICOB003:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger("APICOB003002", "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger("APICOB003002", "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }
    }
}
