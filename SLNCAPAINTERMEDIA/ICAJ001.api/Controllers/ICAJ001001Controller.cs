using ICAJ001.api.Infraestructura.Servicios;
using ICAJ001.api.Infraestructure.Configuration;
using ICAJ001.api.Infraestructure.Services;
using ICAJ001.api.Models._001.Response;
using ICAJ001.api.Models.ResponseHomologacion;
using ICAJ001.Models._001.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ001.api.Controllers
{
    
    [ApiController]
    public class ICAJ001001Controller : ControllerBase
    {
        private IManejadorRequest<APICAJ001001MessageRequest> manejadorRequestQueue;
        private IManejadorResponse<APICAJ001001MessageResponse> manejadorReponseQueue;
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
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

        public ICAJ001001Controller(IManejadorRequest<APICAJ001001MessageRequest> _manejadorRequest
             , IManejadorResponse<APICAJ001001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            manejadorRequestQueue = _manejadorRequest;
            manejadorReponseQueue = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }


        [HttpPost]
        [Route("APICAJ001001")]
        [ServiceFilter(typeof(ValidadorRequest))]
        public async Task<ActionResult<APICAJ001001MessageResponse>> APICAJ001001(APICAJ001001MessageRequest parametrorequest)
        {
            string nombre_metodo = "APICAJ001001";

            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }




            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APICAJ001001MessageResponse respuesta = null;
            ResponseHomologacion ResuldatoHomologa = null;
            try
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Recibido : " + respuestaLog);
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
                            respuesta = new APICAJ001001MessageResponse();
                          //  respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("ICAJ001:E000|EMPRESA NO EXISTE");
                            return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
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
                Logger.FileLogger(nombre_metodo, $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICAJ001001MessageResponse();
                  //  respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ001:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                }

                // asignar campo ambiente
                parametrorequest.Enviroment = Entorno;

                // asigna session
                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;

                await manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolarequest, parametrorequest);

                respuesta = await manejadorReponseQueue.RecibeMensajeSesion(sbConenctionStringReceptar, nombrecolaresponse, sesionid, vl_Time, vl_Intentos, nombre_metodo, vl_TimeOutResp);

                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICAJ001001MessageResponse();
                    //respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ001:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);  //StatusCode(StatusCodes.Status204NoContent, "No se retorno resultado de dynamics");
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: Response desde Dynamics: " + respuestaLog);

                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {

                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                respuesta = new APICAJ001001MessageResponse();
                //respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICAJ001:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }
    }
}
