using ICOB003.api.Infraestructura.Configuracion;
using ICOB003.api.Infraestructura.Servicios;
using ICOB003.api.Models._003.Request;
using ICOB003.api.Models._003.Response;
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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ICOB003.api.Controllers
{
    [ApiController]
    public class ICOB003003Controller : Controller
    {

        private IManejadorRequestQueue<APICOB003003MessageRequest> manejadorRequestQueue;
        private IManejadorResponseQueue2<APICOB003003MessageResponse> manejadorReponseQueue;
        private IHomologacionService<ResponseHomologa> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string Entorno = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        private static string sbConenctionStringEnvio = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest003").Value);
        private static string sbConenctionStringReceptar = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse003").Value);
        private static string nombrecolarequest = configuracion.GetSection("Data").GetSection("QueueRequest003").Value.ToString();
        private static string nombrecolaresponse = configuracion.GetSection("Data").GetSection("QueueResponse003").Value.ToString();
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
        public ICOB003003Controller(IManejadorRequestQueue<APICOB003003MessageRequest> _manejadorRequestQueue
            , IManejadorResponseQueue2<APICOB003003MessageResponse> _manejadorReponseQueue
            , IHomologacionService<ResponseHomologa> _homologacionRequest)
        {
            manejadorRequestQueue = _manejadorRequestQueue;
            manejadorReponseQueue = _manejadorReponseQueue;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APICOB003003")]
        [ServiceFilter(typeof(ValidationFilter003Attribute))]
        public async Task<ActionResult<APICOB003003MessageResponse>> APICOB003003(APICOB003003MessageRequest parametrorequest)
        {


            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger("APICOB003003", "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }

            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APICOB003003MessageResponse respuesta = null;
            ResponseHomologa ResuldatoHomologa = null;
            APICOB003003MessageRequest APICOB003003MessageRequestW = new APICOB003003MessageRequest();
            DocumentLedgerList DocumentLedgerListW = null;
            APICOB003003MessageResponse APICOB003003MessageResponseW = new APICOB003003MessageResponse();

            try
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICOB003003", "CONTROLADOR: Request Recibido : " + respuestaLog);
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
                            respuesta = new APICOB003003MessageResponse();
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
                            Logger.FileLogger("APICOB003003", $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APICOB003003", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICOB003003MessageResponse();
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
                APICOB003003MessageRequestW.DataAreaId = parametrorequest.DataAreaId;
                APICOB003003MessageRequestW.Enviroment = parametrorequest.Enviroment;
                APICOB003003MessageRequestW.SessionId = parametrorequest.SessionId;
                APICOB003003MessageRequestW.APTypeTransaction = parametrorequest.APTypeTransaction;
                APICOB003003MessageRequestW.DocumentLedgerList = new List<DocumentLedgerList>();
                if (parametrorequest.DocumentLedgerList != null)
                {
                    foreach (var elem in parametrorequest.DocumentLedgerList)
                    {
                        DocumentLedgerListW = new DocumentLedgerList();
                        DocumentLedgerListW.APIdentificationList = elem.APIdentificationList;
                        DocumentLedgerListW.CustAccount = CodigoCliente.CrecosAdynamics(elem.CustAccount, longAllenar, "APICOB003003");
                        DocumentLedgerListW.TransDate = elem.TransDate;
                        DocumentLedgerListW.AmountDebit = elem.AmountDebit;
                        DocumentLedgerListW.AmountCredit = elem.AmountCredit;
                        DocumentLedgerListW.PostingProfile = elem.PostingProfile;
                        DocumentLedgerListW.DocumentNegoc = elem.DocumentNegoc;
                        APICOB003003MessageRequestW.DocumentLedgerList.Add(DocumentLedgerListW);

                    }
                }
                
                string responseLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICOB003003", "Request con código cliente No convertido: " + responseLog);
                responseLog = JsonConvert.SerializeObject(APICOB003003MessageRequestW);
                Logger.FileLogger("APICOB003003", "Request con código cliente convertido: " + responseLog);

                //

                await manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolarequest, APICOB003003MessageRequestW);

                respuesta = await manejadorReponseQueue.RecibeMensajeSesion(sbConenctionStringReceptar, nombrecolaresponse, sesionid, vl_Time, vl_Intentos, "APICOB003003", vl_TimeOutResp);



                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(APICOB003003MessageRequestW);
                    Logger.FileLogger("APICOB003003", "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger("APICOB003003", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICOB003003MessageResponse();
                    // respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICOB003:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);  //StatusCode(StatusCodes.Status204NoContent, "No se retorno resultado de dynamics");
                }
                else
                {
                    
                    APICOB003003MessageResponseW.APIdentificationList = respuesta.APIdentificationList;
                    APICOB003003MessageResponseW.LedgerJournalTransId = respuesta.LedgerJournalTransId;
                    if (respuesta.CustAccount != null)
                        APICOB003003MessageResponseW.CustAccount = CodigoCliente.DynamicAcrecos(respuesta.CustAccount, "APICOB003003");
                    APICOB003003MessageResponseW.TransDate = respuesta.TransDate;
                    APICOB003003MessageResponseW.Amount = respuesta.Amount;
                    APICOB003003MessageResponseW.StatusId = respuesta.StatusId;
                    APICOB003003MessageResponseW.ErrorList = new List<string>();
                    if (respuesta.ErrorList != null)
                    {
                        foreach (var elem1 in respuesta.ErrorList)
                        {
                            APICOB003003MessageResponseW.ErrorList.Add(elem1);
                        }
                    }
                    respuestaLog = JsonConvert.SerializeObject(APICOB003003MessageRequestW);
                    Logger.FileLogger("APICOB003003", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger("APICOB003003", "CONTROLADOR: Response desde Dynamics: " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(APICOB003003MessageResponseW);
                    Logger.FileLogger("APICOB003003", "CONTROLADOR: Response desde Dynamics, Convertida : " + respuestaLog);


                    return Ok(APICOB003003MessageResponseW);
                }
            }
            catch (Exception ex)
            {

                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                respuesta = new APICOB003003MessageResponse();
                // respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICOB003:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger("APICOB003003", "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger("APICOB003003", "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }



    }
}
