using ICOB009.api.Infraestructura.Servicios;
using ICOB009.api.Infraestructure.Configuration;
using ICOB009.api.Infraestructure.Services;
using ICOB009.api.Models._001.Request;
using ICOB009.api.Models._001.Response;
using ICOB009.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ICOB009.api.Controllers
{
    
    [ApiController]
    public class ICOB009001Controller : ControllerBase
    {
        private IManejadorRequest<APICOB009001MessageRequest> QueueRequest;
        private IManejadorResponse<APICOB009001MessageResponse> QueueResponse;
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
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32(configuracion.GetSection("Data").GetSection("LongAllenar").Value);

        public ICOB009001Controller(IManejadorRequest<APICOB009001MessageRequest> _manejadorRequest
            , IManejadorResponse<APICOB009001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }



        [HttpPost]
        [Route("APICOB009001")]
        [ServiceFilter(typeof(ValidadorRequest))]
        public async Task<ActionResult<APICOB009001MessageResponse>> APICOB009001(APICOB009001MessageRequest parametrorequest)
        {
            APICOB009001MessageResponse respuesta = null;
            try
            {

                string nombre_metodo = "APICOB009001";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICOB009001", "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);

                if (ResultadoHomologa == null)
                {
                    respuesta = new APICOB009001MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICOB009:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APICOB009001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

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

                if (parametrorequest.APDationRequests != null) { 
                    if (parametrorequest.APDationRequests.CustAccount!=null && parametrorequest.APDationRequests.CustAccount != "") { 
                        parametrorequest.APDationRequests.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.APDationRequests.CustAccount, longAllenar, "APICOB009001");
                    }
                }
                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICOB009001", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time1, vl_Attempts1, vl_Timeout, nombre_metodo);


                if (respuesta == null)
                {
                    respuesta = new APICOB009001MessageResponse();
                    respuesta.ErrorList = new List<string>();

                    respuesta.ErrorList.Add("ICOB009:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    Logger.FileLogger("APICOB009001", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);

                }
                else
                {
                    if (respuesta.APDationResponse != null)
                    {
                        int cantidad = respuesta.APDationResponse.Count;
                        int i = 0;

                        if (cantidad > 0)
                        {
                            if (respuesta.APDationResponse[i].CustAccount!="" && respuesta.APDationResponse[i].CustAccount !=null)
                            do
                            {                               
                                respuesta.APDationResponse[i].CustAccount = CodigoCliente.DynamicAcrecos(respuesta.APDationResponse[i].CustAccount, "APICOB009001");
                                i++;
                            } while (i < cantidad);
                        }
                    }
                }


                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APICOB009001", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new APICOB009001MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("ICOB009:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");

                Logger.FileLogger("APICOB009001", "CONTROLADOR: ERROR: " + ex.ToString());
                return Ok(respuesta);

            }

        }

    }
}