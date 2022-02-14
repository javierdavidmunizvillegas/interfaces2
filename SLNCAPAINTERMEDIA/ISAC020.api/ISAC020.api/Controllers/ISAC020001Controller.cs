using Azure.Messaging.ServiceBus;
using ISAC020.api.Infraestructura.Servicios;
using ISAC020.api.Infraestructure.Configuration;
using ISAC020.api.Infraestructure.Services;
using ISAC020.api.Models.ResponseHomologacion;
using ISAC020.Models;
using ISAC020.Models.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISAC020.api.Controllers
{
    
    [ApiController]
    public class ISAC020001Controller : ControllerBase
    {
        private IManejadorRequest<APISAC020001MessageRequest> QueueRequest;
        private IManejadorResponse<APISAC020001MessageResponse> QueueResponse;
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
  

        public ISAC020001Controller(IManejadorRequest<APISAC020001MessageRequest> _manejadorRequest
            , IManejadorResponse<APISAC020001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }
       


        [HttpPost]
        [Route("APISAC020001")]
        [ServiceFilter(typeof(ValidadorRequest))]
        public async Task<ActionResult<APISAC020001MessageResponse>> APISAC020001(APISAC020001MessageRequest parametrorequest)
        {
            APISAC020001MessageResponse respuesta = null;
            try
            {
                
                string nombre_metodo = "APISAC020001";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APISAC020001", "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);
               
                if (ResultadoHomologa == null)
                {
                    respuesta = new APISAC020001MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ISAC020:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APISAC020001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

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
                Logger.FileLogger("APISAC020001", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time1, vl_Attempts1, vl_Timeout, nombre_metodo);


                if (respuesta == null)
                {
                    respuesta = new APISAC020001MessageResponse();
                    respuesta.ErrorList = new List<string>();

                    respuesta.ErrorList.Add("ISAC020:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    Logger.FileLogger("APISAC020001", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);
                   
                }

                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APISAC020001", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new APISAC020001MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("ISAC020:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");
                //Logger.FileLogger("APISAC020001", "CONTROLADOR: No se retorno resultado de Dynamics");
                Logger.FileLogger("APISAC020001", "CONTROLADOR: ERROR: "+ex.ToString());              
                return Ok(respuesta);

            }

        }

    }
}
