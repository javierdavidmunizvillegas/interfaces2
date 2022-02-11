using IPRO002.api.Infraestructura.Servicios;
using IPRO002.api.Infraestructure.Configuration;
using IPRO002.api.Infraestructure.Services;
using IPRO002.api.Models._001.Response;
using IPRO002.api.Models._004.Request;
using IPRO002.api.Models._004.Response;
using IPRO002.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IPRO002.api.Controllers
{
   
    [ApiController]
    public class IPRO002004Controller : ControllerBase
    {
        private IManejadorRequest<APIPRO002004MessageRequest> QueueRequest;
        private IManejadorResponse<APIPRO002004MessageResponse> QueueResponse;
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest4").Value.ToString();
        private static string vl_QueueResponse = configuracion.GetSection("Data").GetSection("QueueResponse4").Value.ToString();
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep").Value);
        private static int vl_Attempts = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts").Value);
        private static int vl_Time4 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep4").Value);
        private static int vl_Attempts4 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts4").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout").Value);
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        
        public IPRO002004Controller(IManejadorRequest<APIPRO002004MessageRequest> _manejadorRequest
            , IManejadorResponse<APIPRO002004MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }


        [HttpPost]
        [Route("APIPRO002004")]
        [ServiceFilter(typeof(ValidadorRequest004))]
        public async Task<ActionResult<APIPRO002004MessageResponse>> APIPRO002001(APIPRO002004MessageRequest parametrorequest)
        {
            APIPRO002004MessageResponse respuesta = null;
            try
            {
                string nombre_metodo = "APIPRO002004";


                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APIPRO002004", "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);

                if (ResultadoHomologa == null)
                {
                    respuesta = new APIPRO002004MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("IPRO002:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APIPRO002004", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

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
                Logger.FileLogger("APIPRO002004", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time4, vl_Attempts4, vl_Timeout, nombre_metodo);


                if (respuesta == null)
                {
                    respuesta = new APIPRO002004MessageResponse();
                    respuesta.ErrorList = new List<string>();

                    respuesta.ErrorList.Add("IPRO002:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    Logger.FileLogger("APIPRO002004", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);

                }

                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APIPRO002004", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new APIPRO002004MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("IPRO002:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");
                Logger.FileLogger("APIPRO002004", "CONTROLADOR: ERROR: " + ex.ToString());
                return Ok(respuesta);

            }

        }
    }
}
