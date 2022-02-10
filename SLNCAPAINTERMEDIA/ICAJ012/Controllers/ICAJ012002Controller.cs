using ICAJ012.api.Infraestructure.Configuration;
using ICAJ012.api.Infraestructure.Services;
using ICAJ012.api.Models._002.Request;
using ICAJ012.api.Models._002.Response;
using ICAJ012.api.Models.Homologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ012.api.Controllers
{
    
    [ApiController]
    public class ICAJ012002Controller : ControllerBase
    {
        private IManejadorRequest<APICAJ012002MessageRequest> QueueRequest;
        private IManejadorResponse<APICAJ012002MessageResponse> QueueResponse;
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest2").Value.ToString();
        private static string vl_QueueResponse = configuracion.GetSection("Data").GetSection("QueueResponse2").Value.ToString();
        private static int vl_Time2 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep2").Value);
        private static int vl_Attempts2 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts2").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout2").Value);
        private static int vl_TimeWS = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleepWS").Value);
        private static int vl_AttemptsWS = Convert.ToInt32(configuracion.GetSection("Data").GetSection("AttemptsWS").Value);

        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        string respuestaLog;
        const int SegAMiliS = 1000;
        const int NanoAMiliS = 10000;

        public ICAJ012002Controller(IManejadorRequest<APICAJ012002MessageRequest> _manejadorRequest
            , IManejadorResponse<APICAJ012002MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APICAJ012002")]
        [ServiceFilter(typeof(ValidadorRequest002))]
        public async Task<ActionResult<APICAJ012002MessageResponse>> APICAJ012002(APICAJ012002MessageRequest parametrorequest)
        {
            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
           
            ResponseHomologacion ResuldatoHomologa = null;
            APICAJ012002MessageResponse respuesta = null;
            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger("APICAJ012002", "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }
            try
            {

                string nombre_metodo = "APICAJ012002";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ012002", "CONTROLADOR: Request Recibido: " + jsonrequest);
                string DataAreaId = parametrorequest.DataAreaId;
                ///HOMOLOGACIÓN////////////////////////////////////////////////
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
                            respuesta = new APICAJ012002MessageResponse();
                            //  respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("ICAJ012:E000|EMPRESA NO EXISTE");
                            return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                        }
                        if (ResuldatoHomologa != null && "OK".Equals(ResuldatoHomologa.DescripcionId))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            parametrorequest.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICAJ012002", $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APICAJ012002", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICAJ012002MessageResponse();
                    // respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ012:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                }


                /////////////////////////////////////////////////////////////////////

                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                parametrorequest.Enviroment = vl_Environment;
                
                // convertir
                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ012002", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);
               
                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time2, vl_Attempts2, vl_Timeout, nombre_metodo);
                // convertir
               
                //


                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                    Logger.FileLogger("APICAJ012002", "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger("APICAJ012002", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICAJ012002MessageResponse();
                    //  respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ012:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                    Logger.FileLogger("APICAJ012002", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger("APICAJ012002", "CONTROLADOR: Response desde Dynamics: " + respuestaLog);

                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                respuesta = new APICAJ012002MessageResponse();
                //  respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICAJ012:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger("APICAJ012002", "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger("APICAJ012002", "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }
    }
}
