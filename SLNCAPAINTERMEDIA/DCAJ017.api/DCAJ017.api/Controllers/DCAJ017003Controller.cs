using DCAJ017.api.Infraestructura.Servicios;
using DCAJ017.api.Infraestructure.Configuration;
using DCAJ017.api.Infraestructure.Services;
using DCAJ017.api.Models._001.Response;
using DCAJ017.api.Models._003.Request;
using DCAJ017.api.Models._003.Response;
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
    public class DCAJ017003Controller : ControllerBase
    {
        private IManejadorRequest<APDCAJ017003MessageRequest> QueueRequest;
        private IManejadorResponse<APDCAJ017003MessageResponse> QueueResponse;
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest3").Value.ToString();
        private static string vl_QueueResponse = configuracion.GetSection("Data").GetSection("QueueResponse3").Value.ToString();
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep").Value);
        private static int vl_Attempts = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts").Value);
        private static int vl_Time3 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep3").Value);
        private static int vl_Attempts3 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts3").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout").Value);
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32(configuracion.GetSection("Data").GetSection("LongAllenar").Value);
        public DCAJ017003Controller(IManejadorRequest<APDCAJ017003MessageRequest> _manejadorRequest
             , IManejadorResponse<APDCAJ017003MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }


        [HttpPost]
        [Route("APDCAJ017003")]
        [ServiceFilter(typeof(ValidadorRequest003))]
        public async Task<ActionResult<APDCAJ017003MessageResponse>> APDCAJ017003(APDCAJ017003MessageRequest parametrorequest)
        {
            APDCAJ017003MessageResponse respuesta = null;
            try
            {

                string nombre_metodo = "APDCAJ017003";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APDCAJ017003", "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);

                if (ResultadoHomologa == null)
                {
                    respuesta = new APDCAJ017003MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("DCAJ017:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APDCAJ017003", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

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

                if (parametrorequest.CustAccount != null && parametrorequest.CustAccount != "")
                {
                    parametrorequest.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.CustAccount, longAllenar, "APDCAJ017003");

                }

                if (parametrorequest.APAditionalInformationContract.CustAccount != null && parametrorequest.APAditionalInformationContract.CustAccount != "")
                {
                    parametrorequest.APAditionalInformationContract.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.APAditionalInformationContract.CustAccount, longAllenar, "APDCAJ017003");

                }

                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APDCAJ017003", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time3, vl_Attempts3, vl_Timeout, nombre_metodo);


                if (respuesta == null)
                {
                    respuesta = new APDCAJ017003MessageResponse();
                    respuesta.ErrorList = new List<string>();

                    respuesta.ErrorList.Add("DCAJ017:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");
                    Logger.FileLogger("APDCAJ017003", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);
                }
                else
                {
                    if (respuesta.CustAccount != null && respuesta.CustAccount != "")
                    {
                        respuesta.CustAccount = CodigoCliente.DynamicAcrecos(respuesta.CustAccount, "APDCAJ017002");
                    }
                }
                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APDCAJ017003", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);
                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new APDCAJ017003MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("DCAJ017:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");
                Logger.FileLogger("APDCAJ017003", "CONTROLADOR: ERROR: " + ex.ToString());
                return Ok(respuesta);

            }

        }

    }
}
