using ICRE006.api.Infraestructura.Servicios;
using ICRE006.api.Infraestructure.Configuration;
using ICRE006.api.Infraestructure.Services;
using ICRE006.api.Models._001.Request;
using ICRE006.api.Models._001.Response;
using ICRE006.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE006.api.Controllers
{
    
    [ApiController]
    public class ICRE006001Controller : ControllerBase
    {
        private IManejadorRequest<APICRE006001MessageRequest> QueueRequest;
        private IManejadorResponse<APICRE006001MessageResponse> QueueResponse;
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

        public ICRE006001Controller(IManejadorRequest<APICRE006001MessageRequest> _manejadorRequest
             , IManejadorResponse<APICRE006001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }


        [HttpPost]
        [Route("APICRE006001")]
        [ServiceFilter(typeof(ValidadorRequest))]
        public async Task<ActionResult<APICRE006001MessageResponse>> APICRE006001(APICRE006001MessageRequest parametrorequest)
        {
            APICRE006001MessageResponse respuesta = null;
            try
            {

                string nombre_metodo = "APICRE006001";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICRE006001", "CONTROLADOR: Request Recibido: " + jsonrequest);
               
                /***********************HOMOLOGACION****************************/
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId,vl_Time,vl_Attempts,nombre_metodo);
                if (ResultadoHomologa == null)
                {
                    respuesta = new APICRE006001MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICRE006:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");
                    Logger.FileLogger("APICRE006001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");
                    return Ok(respuesta);
                }
                else
                {
                    if (ResultadoHomologa.Response != null)
                    {
                        parametrorequest.DataAreaId = ResultadoHomologa.Response;
                    }

                }
                /***********************FIN DE HOMOLOGACION****************************/

                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                parametrorequest.Enviroment = vl_Environment;

                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICRE006001", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);


                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time1, vl_Attempts1,vl_Timeout, nombre_metodo);
                

                if (respuesta == null)
                {
                    
                    respuesta = new APICRE006001MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    
                    respuesta.ErrorList.Add("ICRE006:E001|NO SE RETORNO RESULTADOS DE DINAMYCS");
                    Logger.FileLogger("APICRE006001", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);
                 
                }
                else
                {
                    if (respuesta.APCustTable != null)
                    {
                        if (respuesta.APCustTable.AccountNum != null && respuesta.APCustTable.AccountNum != "")
                        {
                            respuesta.APCustTable.AccountNum = Int32.Parse(CodigoCliente.DynamicAcrecos(respuesta.APCustTable.AccountNum, "APICRE006001")).ToString();
                        }
                    }
                }
                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APICRE006001", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);

                return Ok(respuesta);

            }
            catch (Exception ex)
            {
                respuesta = new APICRE006001MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("ICRE006:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");
                Logger.FileLogger("APICRE006001", "CONTROLADOR: No se retorno resultado de Dynamics");
                return Ok(respuesta);
            }

        }
    }
}
