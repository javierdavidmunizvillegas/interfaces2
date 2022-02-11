using ICXP003.api.Infraestructura.Servicios;
using IVTA003.api.Infraestructure.Configuration;
using IVTA003.api.Infraestructure.Services;
using IVTA003.api.Models._002.Request;
using IVTA003.api.Models._002.Response;
using IVTA003.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVTA003.api.Controllers
{
    
    [ApiController]
    public class IVTA003002Controller : ControllerBase
    {
        private IManejadorRequest<APIVTA003002MessageRequest> QueueRequest;
        private IManejadorResponse<APIVTA003002MessageResponse> QueueResponse;
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest2").Value.ToString();
        private static string vl_QueueResponse = configuracion.GetSection("Data").GetSection("QueueResponse2").Value.ToString();
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep").Value);
        private static int vl_Attempts = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts").Value);
        private static int vl_Time2 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep2").Value);
        private static int vl_Attempts2 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts2").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);

        public IVTA003002Controller(IManejadorRequest<APIVTA003002MessageRequest> _manejadorRequest
            , IManejadorResponse<APIVTA003002MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }


        [HttpPost]
        [Route("APIVTA003002")]
        [ServiceFilter(typeof(ValidadorRequest002))]
        public async Task<ActionResult<APIVTA003002MessageResponse>> APIVTA003002(APIVTA003002MessageRequest parametrorequest)
        {
            APIVTA003002MessageResponse respuesta = null;
            try
            {

                string nombre_metodo = "APIVTA003002";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APIVTA003002", "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);

                if (ResultadoHomologa == null)
                {
                    respuesta = new APIVTA003002MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("IVTA003:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APIVTA003002", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

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
                Logger.FileLogger("APIVTA003002", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time2, vl_Attempts2, vl_Timeout, nombre_metodo);


                if (respuesta == null)
                {
                    respuesta = new APIVTA003002MessageResponse();
                    respuesta.ErrorList = new List<string>();

                    respuesta.ErrorList.Add("IVTA003:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    Logger.FileLogger("APIVTA003002", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);

                }

                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APIVTA003002", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new APIVTA003002MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("IVTA003:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");
                Logger.FileLogger("APIVTA003002", "CONTROLADOR: ERROR: " + ex.ToString());
                return Ok(respuesta);

            }

        }

    }
}
