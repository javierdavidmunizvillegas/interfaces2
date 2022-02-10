using ICAJ020.api.Infraestructura.Configuracion;
using ICAJ020.api.Infraestructura.Servicios;
using ICAJ020.api.Models._002.Request;
using ICAJ020.api.Models._002.Response;
using ICAJ020.api.Models.Homologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ020.api.Controllers
{
    [ApiController]
    public class ICAJ020002Controller : Controller
    {

        private IManejadorRequestQueue<APICAJ020002MessageRequest> manejadorRequestQueue;
        private IManejadorResponseQueue2<APICAJ020002MessageResponse> manejadorReponseQueue;
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
        public ICAJ020002Controller(IManejadorRequestQueue<APICAJ020002MessageRequest> _manejadorRequestQueue
            , IManejadorResponseQueue2<APICAJ020002MessageResponse> _manejadorReponseQueue, IHomologacionService<ResponseHomologa> _homologacionRequest)
        {
            manejadorRequestQueue = _manejadorRequestQueue;
            manejadorReponseQueue = _manejadorReponseQueue;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APICAJ020002")]
        [ServiceFilter(typeof(ValidationFilter002Attribute))]
        public async Task<ActionResult<APICAJ020002MessageResponse>> APICAJ009001(APICAJ020002MessageRequest parametrorequest)
        {
            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger("APICAJ020002", "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }
            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APICAJ020002MessageResponse respuesta = null;
            ResponseHomologa ResuldatoHomologa = null;
            APICAJ020002MessageRequest APICAJ020002MessageRequestW = new APICAJ020002MessageRequest();
            APDocumentRetentionRequest APDocumentRetentionRequestList = null;
            APChequeRequest APChequeRequestList = null;
            APLicenseRequest APLicenseRequestList = null;
            APMediaElectronicRequest APMediaElectronicRequestList = null;


            try
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ020002", "CONTROLADOR: Request Recibido : " + respuestaLog);
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
                            respuesta = new APICAJ020002MessageResponse();
                            respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("ICAJ020:E000|EMPRESA NO EXISTE");
                            return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                        }
                        if (ResuldatoHomologa != null && "OK".Equals(ResuldatoHomologa.DescripcionId))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            parametrorequest.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICAJ020002", $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APICAJ020002", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICAJ020002MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ020:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                }

                // asignar campo ambiente
                parametrorequest.Enviroment = Entorno;

                // asigna session
                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;

                APICAJ020002MessageRequestW.DataAreaId = parametrorequest.DataAreaId;
                APICAJ020002MessageRequestW.Enviroment = parametrorequest.Enviroment;
                APICAJ020002MessageRequestW.SessionId = parametrorequest.SessionId;
                APICAJ020002MessageRequestW.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.CustAccount, longAllenar, "APICAJ020002"); 
                APICAJ020002MessageRequestW.Voucher = parametrorequest.Voucher;
                APICAJ020002MessageRequestW.PaymMode = parametrorequest.PaymMode;
                APICAJ020002MessageRequestW.Amount = parametrorequest.Amount;
                APICAJ020002MessageRequestW.ReasonReverse = parametrorequest.ReasonReverse;
                APICAJ020002MessageRequestW.DateReverse = parametrorequest.DateReverse;
                APICAJ020002MessageRequestW.DescriptionReverse = parametrorequest.DescriptionReverse;
/*
                if (parametrorequest.ChequeRequestList != null)
                {
                    APICAJ020002MessageRequestW.ChequeRequestList = new List<APChequeRequest>();

                    foreach (var elem in parametrorequest.ChequeRequestList)
                    {
                        APChequeRequestList = new APChequeRequest();
                        APChequeRequestList.BankId = elem.BankId;
                        APChequeRequestList.NumberAccountCheck = elem.NumberAccountCheck;
                        APChequeRequestList.NumberCheck = elem.NumberCheck;
                        APChequeRequestList.DueDateCheque = elem.DueDateCheque;
                        APChequeRequestList.AmountCheck = elem.AmountCheck;
                        APChequeRequestList.CustAccount = CodigoCliente.CrecosAdynamics(elem.CustAccount, longAllenar, "APICAJ020002");
                        APChequeRequestList.Description = elem.Description;
                        APChequeRequestList.PostingProfile = elem.PostingProfile;
                        APChequeRequestList.DateReverse = elem.DateReverse;
                        APChequeRequestList.CodeStore = elem.CodeStore;
                        APChequeRequestList.CodeCash = elem.CodeCash;
                        APChequeRequestList.User = elem.User;
                        APChequeRequestList.TypeTransaction = elem.TypeTransaction;
                        APICAJ020002MessageRequestW.ChequeRequestList.Add(APChequeRequestList);

                    }
                }
*/

                if (parametrorequest.MediaElectronicRequestList != null)
                {
                    APICAJ020002MessageRequestW.MediaElectronicRequestList = new List<APMediaElectronicRequest>();

                    foreach (var elem1 in parametrorequest.MediaElectronicRequestList)
                    {
                        APMediaElectronicRequestList = new APMediaElectronicRequest();
                        APMediaElectronicRequestList.StoreId = elem1.StoreId;
                        APMediaElectronicRequestList.NumberDocument = elem1.NumberDocument;
                        APMediaElectronicRequestList.CustAccount = CodigoCliente.CrecosAdynamics(elem1.CustAccount, longAllenar, "APICAJ020002");
                        APMediaElectronicRequestList.Processor = elem1.Processor;
                        APMediaElectronicRequestList.BankAdquiringId = elem1.BankAdquiringId;
                        APMediaElectronicRequestList.BatchNumber = elem1.BatchNumber;
                        APMediaElectronicRequestList.Reference = elem1.Reference;
                        APMediaElectronicRequestList.AuthorizationNumber = elem1.AuthorizationNumber;
                        APICAJ020002MessageRequestW.MediaElectronicRequestList.Add(APMediaElectronicRequestList);
                    }
                }
                /*
                if (parametrorequest.DocumentRetentionRequest != null)
                {
                    APICAJ020002MessageRequestW.DocumentRetentionRequest = new List<APDocumentRetentionRequest>();


                    foreach (var elem2 in parametrorequest.DocumentRetentionRequest)
                    {
                        APDocumentRetentionRequestList = new APDocumentRetentionRequest();
                        APDocumentRetentionRequestList.NumberRetention = elem2.NumberRetention;
                        APDocumentRetentionRequestList.TransDate = elem2.TransDate;
                        APDocumentRetentionRequestList.AuthorizationNumberRet = elem2.AuthorizationNumberRet;
                        APICAJ020002MessageRequestW.DocumentRetentionRequest.Add(APDocumentRetentionRequestList);

                    }
                }
                */
                if (parametrorequest.LicenseRequest != null)
                {
                    //APICAJ020002MessageRequestW.LicenseRequest = new List<APLicenseRequest>();
                    APICAJ020002MessageRequestW.LicenseRequest = new APLicenseRequest();

                    //foreach (var elem3 in parametrorequest.LicenseRequest)
                    //{
                        APLicenseRequestList = new APLicenseRequest();
                        APLicenseRequestList.InvoiceMoto = parametrorequest.LicenseRequest.InvoiceMoto;
                        APLicenseRequestList.Cpn = parametrorequest.LicenseRequest.Cpn;
                        APLicenseRequestList.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.LicenseRequest.CustAccount, longAllenar, "APICAJ020002");
                        APLicenseRequestList.Amount = parametrorequest.LicenseRequest.Amount;
                        APLicenseRequestList.Reason = parametrorequest.LicenseRequest.Reason;
                        APLicenseRequestList.BusinessUnit = parametrorequest.LicenseRequest.BusinessUnit;
                        APLicenseRequestList.Voucher = parametrorequest.LicenseRequest.Voucher;
                        APICAJ020002MessageRequestW.LicenseRequest = APLicenseRequestList;

                    //}
                }

                
                respuestaLog = JsonConvert.SerializeObject(APICAJ020002MessageRequestW);
                Logger.FileLogger("APICAJ020002", "CONTROLADOR: Request Enviado a Dynamics, Convertido : " + respuestaLog);

                await manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolarequest, APICAJ020002MessageRequestW);

                respuesta = await manejadorReponseQueue.RecibeMensajeSesion(sbConenctionStringReceptar, nombrecolaresponse, sesionid, vl_Time, vl_Intentos, "APICAJ020002", vl_TimeOutResp);

                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(APICAJ020002MessageRequestW);
                    Logger.FileLogger("APICAJ020002", "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger("APICAJ020002", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICAJ020002MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ020:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);  //StatusCode(StatusCodes.Status204NoContent, "No se retorno resultado de dynamics");
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(APICAJ020002MessageRequestW);
                    Logger.FileLogger("APICAJ020002", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger("APICAJ020002", "CONTROLADOR: Response desde Dynamics: " + respuestaLog);

                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {

                respuestaLog = JsonConvert.SerializeObject(APICAJ020002MessageRequestW);
                respuesta = new APICAJ020002MessageResponse();
                respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICAJ020:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger("APICAJ020002", "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger("APICAJ020002", "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }



    }
}
