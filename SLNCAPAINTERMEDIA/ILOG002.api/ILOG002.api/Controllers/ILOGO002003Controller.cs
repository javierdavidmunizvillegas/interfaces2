using ILOG002.api.Infraestructura.Servicios;
using ILOG002.api.Infraestructure.Configuration;
using ILOG002.api.Infraestructure.Services;
using ILOG002.api.Models;
using ILOG002.api.Models._001.Request;
using ILOG002.api.Models._001.Response;
using ILOG002.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ILOG002.api.Controllers
{
    
    [ApiController]
    public class ILOGO002003Controller : ControllerBase
    {
        private IManejadorRequest<APILOG002003MessageRequest> QueueRequest;
        private IManejadorResponse<APILOG002003MessageResponse> QueueResponse;
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest").Value.ToString();
        private static string vl_QueueResponse = configuracion.GetSection("Data").GetSection("QueueResponse").Value.ToString();
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep").Value);
        private static int vl_Attempts = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts").Value);
        private static int vl_Time1 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep1").Value);
        private static int vl_Attempts1 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts1").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout").Value);
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string vl_DataAreaid = Convert.ToString(configuracion.GetSection("Data").GetSection("DATA_AREA_ID").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);

        public ILOGO002003Controller(IManejadorRequest<APILOG002003MessageRequest> _manejadorRequest
            , IManejadorResponse<APILOG002003MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }

        [HttpPost]
        [Route("APILOG002003")]
       
        public async Task<ActionResult<APILOG002003MessageResponse>> APILOG002003(APCurrierShippifyList[] parametrorequest)
        {
            string nombre_metodo = "APILOG002003";
            APILOG002003MessageResponse respuesta = null;
            APILOG002003MessageRequest request = new APILOG002003MessageRequest();
                       
            try
            {
                //if (parametrorequest.Count() > 0)
                //{
                //    int cant = parametrorequest.Count();
                //    int i = 0;

                //    if (cant > 0)
                //    {
                //        do
                //        {
                //            if (parametrorequest[i].steps != null)
                //            {

                                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                                Logger.FileLogger("APILOG002003", "CONTROLADOR: Request Recibido Shippify: " + jsonrequest);

                                request.DataAreaId = vl_DataAreaid;
                                string sesionid = Guid.NewGuid().ToString();
                                request.SessionId = sesionid;
                                request.Enviroment = vl_Environment;

                                request.CurrierShippifyList = parametrorequest;


                                string jsonrequest2 = JsonConvert.SerializeObject(request);
                                Logger.FileLogger("APILOG002003", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, request);

                                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time1, vl_Attempts1, vl_Timeout, nombre_metodo);


                                if (respuesta == null)
                                {
                                    respuesta = new APILOG002003MessageResponse();
                                    respuesta.ErrorList = new List<string>();

                                    respuesta.ErrorList.Add("APILOG002:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                                    Logger.FileLogger("APILOG002003", "CONTROLADOR: No se retorno resultado de Dynamics");
                                    return Ok(respuesta);

                                }

                                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                                Logger.FileLogger("APILOG002003", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);
                                                               
                //            }


                //            i++;
                //        } while (i < cant);
                //    }

                //}

                return Ok(respuesta);


            }
            catch (Exception ex)
            {
                respuesta = new APILOG002003MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("APILOG002:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");
              
                Logger.FileLogger("APILOG002003", "CONTROLADOR: ERROR: " + ex.ToString());
                return Ok(respuesta);

            }

        }
    }
}
