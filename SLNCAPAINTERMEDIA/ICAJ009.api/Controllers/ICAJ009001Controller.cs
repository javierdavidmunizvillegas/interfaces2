using ICAJ009.api.Infraestructura.Configuracion;
using ICAJ009.api.Infraestructura.Servicios;
using ICAJ009.api.Models._001.Request;
using ICAJ009.api.Models._001.Response;
using ICAJ009.api.Models.Homologacion;
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

namespace ICAJ009.api.Controllers
{
    [ApiController]
    public class ICAJ009001Controller : ControllerBase
    {

        private IManejadorRequestQueue<APICAJ009001MessageRequest> manejadorRequestQueue;
        private IManejadorResponseQueue2<APICAJ009001MessageResponse> manejadorReponseQueue;
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
        public ICAJ009001Controller(IManejadorRequestQueue<APICAJ009001MessageRequest> _manejadorRequestQueue
            , IManejadorResponseQueue2<APICAJ009001MessageResponse> _manejadorReponseQueue, IHomologacionService<ResponseHomologa> _homologacionRequest)
        {
            manejadorRequestQueue = _manejadorRequestQueue;
            manejadorReponseQueue = _manejadorReponseQueue;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APICAJ009001")]
        [ServiceFilter(typeof(ValidationFilter001Attribute))]
        public async Task<ActionResult<APICAJ009001MessageResponse>> APICAJ009001(APICAJ009001MessageRequest parametrorequest)
        {
            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger("APICAJ009001", "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }


            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APICAJ009001MessageResponse respuesta = null;
            ResponseHomologa ResuldatoHomologa = null;
            APICAJ009001MessageRequest APICAJ009001RequestW = null;
            APCustInvoiceServicesContract tmp = null;
            APCustInvoiceServicesLineContract tmp2 = null;
            try
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ009001", "CONTROLADOR: Request Recibido : " + respuestaLog);
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
                            respuesta = new APICAJ009001MessageResponse();
                          //  respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("ICAJ009:E000|EMPRESA NO EXISTE");
                            return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                        }
                        if (ResuldatoHomologa != null && "OK".Equals(ResuldatoHomologa.DescripcionId))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            parametrorequest.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICAJ009001", $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APICAJ009001", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICAJ009001MessageResponse();
                   // respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ009:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                }

                // asignar campo ambiente
                parametrorequest.Enviroment = Entorno;

                // asigna session
                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;

                // actualiza campocodigo de cliente//
                APICAJ009001RequestW = new APICAJ009001MessageRequest();
                APICAJ009001RequestW.DataAreaId = parametrorequest.DataAreaId;
                APICAJ009001RequestW.Enviroment = parametrorequest.Enviroment;
                APICAJ009001RequestW.SessionId = parametrorequest.SessionId;
                if (parametrorequest.APCustInvoiceServicesContractList != null)
                {
                    APICAJ009001RequestW.APCustInvoiceServicesContractList = new List<APCustInvoiceServicesContract>();
                    foreach (var det in parametrorequest.APCustInvoiceServicesContractList)
                    {
                        tmp = new APCustInvoiceServicesContract();
                        tmp.Date = det.Date;
                        tmp.NumericalSequence = det.NumericalSequence;
                        tmp.DocumentNumber = det.DocumentNumber;
                        //tmp.ReasonSIAC = det.ReasonSIAC;
                        tmp.ReasonNC = det.ReasonNC;
                        tmp.DocumentDate = det.DocumentDate;
                        tmp.CustAccount = det.CustAccount;
                        tmp.SourceReceipt = det.SourceReceipt;
                        tmp.ListIdentifier = det.ListIdentifier;
                        tmp.TypeOfDocument = det.TypeOfDocument;
                        tmp.OutsideSytem = det.OutsideSytem;
                        tmp.APCustInvoiceServicesLineContractList = new List<APCustInvoiceServicesLineContract>();
                        foreach (var det2 in det.APCustInvoiceServicesLineContractList)
                        {
                            tmp2 = new APCustInvoiceServicesLineContract();
                            tmp2.NumLine = det2.NumLine;
                            tmp2.Qty = det2.Qty;
                            tmp2.Amount = det2.Amount;
                            tmp2.ReasonSIAC = det2.ReasonSIAC;
                            tmp.APCustInvoiceServicesLineContractList.Add(tmp2);
                        }
                        APICAJ009001RequestW.APCustInvoiceServicesContractList.Add(tmp);
                    }
                    List<APCustInvoiceServicesContract> APCustInvoiceServicesContractListCod = null;
                    APCustInvoiceServicesContractListCod = APICAJ009001RequestW.APCustInvoiceServicesContractList;
                    foreach (APCustInvoiceServicesContract ListElem in APCustInvoiceServicesContractListCod)
                    {
                        ListElem.CustAccount = CodigoCliente.CrecosAdynamics(ListElem.CustAccount, longAllenar, "APICAJ009001"); //para mapear si es necesario
                    }
                }
                string responseLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ009001", "Request con código cliente No convertido: " + responseLog);
                responseLog = JsonConvert.SerializeObject(APICAJ009001RequestW);
                Logger.FileLogger("APICAJ009001", "Request con código cliente convertido: " + responseLog);

                //await manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolarequest, parametrorequest);
                await manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolarequest, APICAJ009001RequestW);

                respuesta = await manejadorReponseQueue.RecibeMensajeSesion(sbConenctionStringReceptar, nombrecolaresponse, sesionid, vl_Time, vl_Intentos, "APICAJ009001", vl_TimeOutResp);

                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(APICAJ009001RequestW);
                    Logger.FileLogger("APICAJ009001", "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger("APICAJ009001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICAJ009001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ009:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);  
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(APICAJ009001RequestW);
                    Logger.FileLogger("APICAJ009001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger("APICAJ009001", "CONTROLADOR: Response desde Dynamics: " + respuestaLog);

                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {

                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                respuesta = new APICAJ009001MessageResponse();
                respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICAJ009:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger("APICAJ009001", "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger("APICAJ009001", "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }



    }
}
