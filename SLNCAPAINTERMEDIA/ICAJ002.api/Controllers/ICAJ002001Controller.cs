using ICAJ002.api.Infraestructura.Configuracion;
using ICAJ002.api.Infraestructura.Servicios;
using ICAJ002.api.Models;
using ICAJ002.api.Models._001.Request;
using ICAJ002.api.Models._001.Response;
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

namespace ICAJ002.api.Controllers
{
    
    [ApiController]
    public class ICAJ002001Controller : ControllerBase
    {

    private IManejadorRequestQueue<APICAJ002001MessageRequest> manejadorRequestQueue;
    private IManejadorResponseQueue2<APICAJ002001MessageResponse> manejadorReponseQueue;
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


    string respuestaLog;
    const int SegAMiliS = 1000;
    const int NanoAMiliS = 10000;
    public ICAJ002001Controller(IManejadorRequestQueue<APICAJ002001MessageRequest> _manejadorRequestQueue
        , IManejadorResponseQueue2<APICAJ002001MessageResponse> _manejadorReponseQueue
        , IHomologacionService<ResponseHomologa> _homologacionRequest)
    {
        manejadorRequestQueue = _manejadorRequestQueue;
        manejadorReponseQueue = _manejadorReponseQueue;
        homologacionRequest = _homologacionRequest;
    }
    [HttpPost]
    [Route("APICAJ002001")]
        [ServiceFilter(typeof(ValidationFilter001Attribute))]
        public async Task<ActionResult<APICAJ002001MessageResponse>> APICAJ002001(APICAJ002001MessageRequest parametrorequest)
        {


            if (parametrorequest == null)
            {
            // Log
            Logger.FileLogger("APICAJ002001", "CONTROLADOR: El parámetro request es nulo.");
            return BadRequest();
            }

            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APICAJ002001MessageResponse respuesta = null;
            ResponseHomologa ResuldatoHomologa = null;
            try
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ002001", "CONTROLADOR: Request Recibido : " + respuestaLog);
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
                            respuesta = new APICAJ002001MessageResponse();
                            respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("ICAJ002:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                            return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                        }
                        if (ResuldatoHomologa != null && "OK".Equals(ResuldatoHomologa.DescripcionId))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            parametrorequest.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICAJ002001", $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APICAJ002001", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICAJ002001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ002:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                }

                // asignar campo ambiente
                parametrorequest.Enviroment = Entorno;

                // asigna session
                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;

                await manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolarequest, parametrorequest);

                respuesta = await manejadorReponseQueue.RecibeMensajeSesion(sbConenctionStringReceptar, nombrecolaresponse, sesionid, vl_Time, vl_Intentos, "APICAJ002001", vl_TimeOutResp);

                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                    Logger.FileLogger("APICAJ002001", "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger("APICAJ002001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICAJ002001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ002:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);  //StatusCode(StatusCodes.Status204NoContent, "No se retorno resultado de dynamics");
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                    Logger.FileLogger("APICAJ002001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger("APICAJ002001", "CONTROLADOR: Response desde Dynamics: " + respuestaLog);

                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {

                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                respuesta = new APICAJ002001MessageResponse();
                respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICAJ002:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger("APICAJ002001", "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger("APICAJ002001", "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }



    }
}
