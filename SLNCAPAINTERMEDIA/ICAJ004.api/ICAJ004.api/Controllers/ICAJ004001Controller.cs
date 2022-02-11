using ICAJ004.api.Infraestructura.Servicios;
using ICAJ004.api.Infraestructure.Configuration;
using ICAJ004.api.Infraestructure.Services;
using ICAJ004.api.Models.Request;
using ICAJ004.api.Models.Response;
using ICAJ004.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ004.api.Controllers
{
    
    [ApiController]
    public class ICAJ004001Controller : ControllerBase
    {
        private IManejadorRequest<APICAJ004001MessageRequest> QueueRequest;
        private IManejadorResponse<APICAJ004001MessageResponse> QueueResponse;
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest").Value.ToString();
        private static string vl_QueueResponse = configuracion.GetSection("Data").GetSection("QueueResponse").Value.ToString();
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep").Value);
        private static int vl_Attempts = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts").Value);
        private static int vl_Time1 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep1").Value);
        private static int vl_Attempts1 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts1").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);


        public ICAJ004001Controller(IManejadorRequest<APICAJ004001MessageRequest> _manejadorRequest
            , IManejadorResponse<APICAJ004001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }



        [HttpPost]
        [Route("APICAJ004001")]
        [ServiceFilter(typeof(ValidadorRequest))]
        public async Task<ActionResult<APICAJ004001MessageResponse>> APICAJ004001(APICAJ004001MessageRequest parametrorequest)
        {
            APICAJ004001MessageResponse respuesta = null;
            try
            {

                string nombre_metodo = "APICAJ004001";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ004001", "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);

                if (ResultadoHomologa == null)
                {
                    respuesta = new APICAJ004001MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ004:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APICAJ004001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

                    return Ok(respuesta);
                }
                else
                {
                    if (ResultadoHomologa.Response != null)
                    {
                        parametrorequest.DataAreaId = ResultadoHomologa.Response;
                    }

                }
                /////////////////////////////////////////////////////////////////////

                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                parametrorequest.Enviroment = vl_Environment;

                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ004001", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time1, vl_Attempts1, vl_Timeout, nombre_metodo);


                if (respuesta == null)
                {
                    respuesta = new APICAJ004001MessageResponse();
                    respuesta.ErrorList = new List<string>();

                    respuesta.ErrorList.Add("ICAJ004:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    Logger.FileLogger("APICAJ004001", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);

                }

                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APICAJ004001", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new APICAJ004001MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("ICAJ004:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");
                Logger.FileLogger("APICAJ004001", "CONTROLADOR: No se retorno resultado de Dynamics");
                return Ok(respuesta);

            }

        }

    }
}
