using DCAJ017.api.Infraestructura.Servicios;
using DCAJ017.api.Infraestructure.Configuration;
using DCAJ017.api.Infraestructure.Services;
using DCAJ017.api.Models._001.Request;
using DCAJ017.api.Models._001.Response;
using DCAJ017.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DCAJ017.api.Controllers
{
   
    [ApiController]
    public class DCAJ017001Controller : ControllerBase
    {
        private IManejadorRequest<APDCAJ017001MessageRequest> QueueRequest;
        private IManejadorResponse<APDCAJ017001MessageResponse> QueueResponse;
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
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32(configuracion.GetSection("Data").GetSection("LongAllenar").Value);

        public DCAJ017001Controller(IManejadorRequest<APDCAJ017001MessageRequest> _manejadorRequest
             , IManejadorResponse<APDCAJ017001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }


        [HttpPost]
        [Route("APDCAJ017001")]
        [ServiceFilter(typeof(ValidadorRequest001))]
        public async Task<ActionResult<APDCAJ017001MessageResponse>> APDCAJ017001(APDCAJ017001MessageRequest parametrorequest)
        {
            APDCAJ017001MessageResponse respuesta = null;
            try
            {

                string nombre_metodo = "APDCAJ017001";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APDCAJ017001", "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);

                if (ResultadoHomologa == null)
                {
                    respuesta = new APDCAJ017001MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("DCAJ017:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APDCAJ017001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

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
                parametrorequest.Environment = vl_Environment;

                if (parametrorequest.CustAccount!=null && parametrorequest.CustAccount != "")
                {
                    parametrorequest.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.CustAccount, longAllenar, "APDCAJ017001");

                }
                if (parametrorequest.APVendTransRegistrationFine != null)
                {
                    if (parametrorequest.APVendTransRegistrationFine.CustAccount != null && parametrorequest.APVendTransRegistrationFine.CustAccount != "")
                    {
                        parametrorequest.APVendTransRegistrationFine.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.APVendTransRegistrationFine.CustAccount, longAllenar, "APDCAJ017001");
                    }
                }

                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APDCAJ017001", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time1, vl_Attempts1, vl_Timeout, nombre_metodo);


                if (respuesta == null)
                {
                    respuesta = new APDCAJ017001MessageResponse();
                    respuesta.ErrorList = new List<string>();

                    respuesta.ErrorList.Add("DCAJ017:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");
                    Logger.FileLogger("APDCAJ017001", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);
                }
                else
                {
                    if (respuesta.CustAccount != null && respuesta.CustAccount != "")
                    {
                        respuesta.CustAccount = CodigoCliente.DynamicAcrecos(respuesta.CustAccount, "APDCAJ017001");
                    }
                }

                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APDCAJ017001", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new APDCAJ017001MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("DCAJ017:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");
                Logger.FileLogger("APDCAJ017001", "CONTROLADOR: ERROR: " + ex.ToString());
                return Ok(respuesta);

            }

        }

    }
}
