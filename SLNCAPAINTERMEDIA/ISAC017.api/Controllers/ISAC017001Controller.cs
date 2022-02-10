using ISAC017.api.Infraestructura.Servicios;
using ISAC017.api.Infraestructure.Configuration;
using ISAC017.api.Infraestructure.Services;
using ISAC017.api.Models._001.Request;
using ISAC017.api.Models._001.Response;
using ISAC017.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.api.Controllers
{
    
    [ApiController]
    public class ISAC017001Controller : ControllerBase
    {
        private IManejadorRequest<APISAC017001MessageRequest> QueueRequest;
        private IManejadorResponse<APISAC017001MessageResponse> QueueResponse;
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest1").Value.ToString();
        private static string vl_QueueResponse = configuracion.GetSection("Data").GetSection("QueueResponse1").Value.ToString();
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep1").Value);
        private static int vl_Attempts = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts1").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout1").Value);
        private static int vl_TimeWS = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleepWS").Value);
        private static int vl_AttemptsWS = Convert.ToInt32(configuracion.GetSection("Data").GetSection("AttemptsWS").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        private static int longAllenar = Convert.ToInt32(configuracion.GetSection("Data").GetSection("LongAllenar").Value);
        string respuestaLog;
        const int SegAMiliS = 1000;
        const int NanoAMiliS = 10000;

        public ISAC017001Controller(IManejadorRequest<APISAC017001MessageRequest> _manejadorRequest
            , IManejadorResponse<APISAC017001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APISAC017001")]
        [ServiceFilter(typeof(ValidationFilter001Attribute))]
        public async Task<ActionResult<APISAC017001MessageResponse>> APISAC017001(APISAC017001MessageRequest parametrorequest)
        {
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APISAC017001MessageResponse respuesta = null;
            ResponseHomologacion ResuldatoHomologa = null;
            ConvierteCodigo Convertir = new ConvierteCodigo();
            string nombre_metodo = "APISAC017001";
            APISAC017001MessageRequest APISAC017001MessageRequestW = new APISAC017001MessageRequest();
            APSalesLineReturn APSalesLineReturnW = null;
            APSalesTableReturn APSalesTableReturnW = null;
            APSalesLineReturn APSalesLineReturnList = null;
            APSalesTableReturn APSalesTableReturnCod = null;


            try
            {

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                if (string.IsNullOrEmpty(DataAreaId) || string.IsNullOrWhiteSpace(DataAreaId))
                    throw new Exception("CONTROLADOR WS HOMOLOGACION: El código de empresa no debe ser nulo.");

                start = Stopwatch.GetTimestamp();
                int cont = 0;
                for (int i = 1; i < vl_AttemptsWS; i++)
                {
                    try
                    {

                        ResuldatoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId);

                        // Validacion el objeto recibido
                        // Condicion para salir
                        if (ResuldatoHomologa != null && (string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                        {
                            respuesta = new APISAC017001MessageResponse();
                            respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("ISAC017:E000|EMPRESA NO EXISTE");
                            return Ok(respuesta); 
                        }
                        if (ResuldatoHomologa != null && "OK".Equals(ResuldatoHomologa.DescripcionId))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            parametrorequest.DataAreaId = responseHomologa;
                            Logger.FileLogger(nombre_metodo, $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APISAC017001", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APISAC017001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ISAC017:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); 
                }


                /////////////////////////////////////////////////////////////////////

                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                parametrorequest.Enviroment = vl_Environment;
                APISAC017001MessageRequestW.DataAreaId = parametrorequest.DataAreaId;
                APISAC017001MessageRequestW.SessionId = parametrorequest.SessionId;
                APISAC017001MessageRequestW.Enviroment = parametrorequest.Enviroment;
                APISAC017001MessageRequestW.ReturnTable = new APSalesTableReturn();
                if (APISAC017001MessageRequestW.ReturnTable != null) 
                {
                    APISAC017001MessageRequestW.ReturnTable.CustAccount = parametrorequest.ReturnTable.CustAccount; //
                    APISAC017001MessageRequestW.ReturnTable.InvoiceId = parametrorequest.ReturnTable.InvoiceId;
                    APISAC017001MessageRequestW.ReturnTable.ReturnDeadline = parametrorequest.ReturnTable.ReturnDeadline;
                    APISAC017001MessageRequestW.ReturnTable.ReturnReasonCodeId = parametrorequest.ReturnTable.ReturnReasonCodeId;
                    APISAC017001MessageRequestW.ReturnTable.ReturnReasonComment = parametrorequest.ReturnTable.ReturnReasonComment;
                    APISAC017001MessageRequestW.ReturnTable.APSalesLineReturnList = new List<APSalesLineReturn>();
                    if (APISAC017001MessageRequestW.ReturnTable.APSalesLineReturnList != null)
                    {
                        foreach (var elem in parametrorequest.ReturnTable.APSalesLineReturnList)
                        {
                            APSalesLineReturnList = new APSalesLineReturn();
                            APSalesLineReturnList.ItemId = elem.ItemId;
                            APSalesLineReturnList.Qty = elem.Qty;
                            APSalesLineReturnList.SerialId = elem.SerialId;
                            APSalesLineReturnList.Monto = elem.Monto;
                            APISAC017001MessageRequestW.ReturnTable.APSalesLineReturnList.Add(APSalesLineReturnList);
                        }
                    }
                    APISAC017001MessageRequestW.ReturnTable.CustAccount = Convertir.CrecosAdynamics(parametrorequest.ReturnTable.CustAccount, longAllenar, nombre_metodo);
                }

                string jsonrequest2 = JsonConvert.SerializeObject(APISAC017001MessageRequestW);
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request con Codigo Convertido Enviado a Dynamics: " + jsonrequest2);
                jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request antes de ser Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, APISAC017001MessageRequestW);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time, vl_Attempts, vl_Timeout, nombre_metodo);


                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(APISAC017001MessageRequestW);
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APISAC017001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ISAc017:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta); 
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(APISAC017001MessageRequestW);
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: Response desde Dynamics: " + respuestaLog);

                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuestaLog = JsonConvert.SerializeObject(APISAC017001MessageRequestW);
                respuesta = new APISAC017001MessageResponse();
                respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ISAC017:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); 
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }
    }
}
