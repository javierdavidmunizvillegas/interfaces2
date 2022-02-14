using ICRE002.api.Infraestructura.Servicios;
using ICRE002.api.Infraestructure.Configuration;
using ICRE002.api.Infraestructure.Services;
using ICRE002.api.Models._003.Request;
using ICRE002.api.Models._003.Response;
using ICRE002.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE002.api.Controllers
{
   //VALE
    [ApiController]
    public class ICRE002003Controller : ControllerBase
    {

        private IManejadorRequest<APICRE002003MessageRequest> QueueRequest;
        private IManejadorResponse<APICRE002003MessageResponse> QueueResponse;
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest3").Value.ToString();
        private static string vl_QueueResponse = configuracion.GetSection("Data").GetSection("QueueResponse3").Value.ToString();
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep").Value);
        private static int vl_Attempts = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts").Value);
        private static int vl_Time3 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep3").Value);
        private static int vl_Attempts3 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts3").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32(configuracion.GetSection("Data").GetSection("LongAllenar").Value);

        public ICRE002003Controller(IManejadorRequest<APICRE002003MessageRequest> _manejadorRequest, IManejadorResponse<APICRE002003MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }


        [HttpPost]
        [Route("APICRE002003")]
        [ServiceFilter(typeof(ValidadorRequest003))]
        public async Task<ActionResult<APICRE002003MessageResponse>> APICRE002003(APICRE002003MessageRequest parametrorequest)
        {
            APICRE002003MessageResponse respuesta = null;
            try
            {

                string nombreMetodo = "APICRE002003";
                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICRE002003", "CONTROLADOR: Request Recibido: " + jsonrequest);

                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResuldatoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts,nombreMetodo);



                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICRE002003MessageResponse();

                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICRE002:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APICRE002003", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

                    return Ok(respuesta);
                }
                else
                {
                    if (ResuldatoHomologa.Response != null)
                    {
                        parametrorequest.DataAreaId = ResuldatoHomologa.Response;
                    }

                }

                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                parametrorequest.Enviroment = vl_Environment;

                if (parametrorequest.APIndependetEntrep != null) {
              
                    int cantidadAPIndependetEntrep = parametrorequest.APIndependetEntrep.Count;
                    int i = 0;
              
                    if (cantidadAPIndependetEntrep > 0) {
                        do
                        {
                            int cantidadAcount = parametrorequest.APIndependetEntrep[i].AccountNum.Length;
                            if (cantidadAcount > 0)
                            {                          
                               parametrorequest.APIndependetEntrep[i].AccountNum = CodigoCliente.CrecosAdynamics(parametrorequest.APIndependetEntrep[i].AccountNum, longAllenar, "APICRE002003");
                            }
                            i++;
                        } while (i < cantidadAPIndependetEntrep);
                    }
                }

                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICRE002003", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time3, vl_Attempts3,vl_Timeout, nombreMetodo);

                if (respuesta == null)
                {
                    respuesta = new APICRE002003MessageResponse();
                    respuesta.ErrorList = new List<string>();

                    respuesta.ErrorList.Add("ICRE002:E001|NO SE RETORNO RESULTADOS DE DINAMYCS");
                    Logger.FileLogger("APICRE002003", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);

                }
                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APICRE002003", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);
                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta = new APICRE002003MessageResponse();
                respuesta.ErrorList = new List<string>();
                Logger.FileLogger("APICRE002003", ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);

            }

        }//FIN DEL METODO
    }
}
