using ISAC015.api.Infraestructura.Servicios;
using ISAC015.api.Infraestructure.Configuration;
using ISAC015.api.Infraestructure.Services;
using ISAC015.api.Models.Request;
using ISAC015.api.Models.Response;
using ISAC015.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ISAC015.api.Controllers
{

    [ApiController]
    public class ISAC015001Controller : ControllerBase
    {
        private IManejadorRequest<APISAC015001MessageRequest> QueueRequest;
        private IManejadorResponse<APISAC015001MessageResponse> QueueResponse;
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

        public ISAC015001Controller(IManejadorRequest<APISAC015001MessageRequest> _manejadorRequest
             , IManejadorResponse<APISAC015001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }


        [HttpPost]
        [Route("APISAC015001")]
        [ServiceFilter(typeof(ValidadorRequest))]
        public async Task<ActionResult<APISAC015001MessageResponse>> APISAC015001(APISAC015001MessageRequest parametrorequest)
        {
            APISAC015001MessageResponse respuesta = null;
            try
            {

                string nombre_metodo = "APISAC015001";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APISAC015001", "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);

                if (ResultadoHomologa == null)
                {
                    respuesta = new APISAC015001MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ISAC015:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APISAC015001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

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
                if (parametrorequest.DeliveryProductQuery != null)
                {
                    if (parametrorequest.DeliveryProductQuery.CustAccount.Count() > 0)
                    {
                        parametrorequest.DeliveryProductQuery.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.DeliveryProductQuery.CustAccount, longAllenar, "APISAC015001");                    
                    }
                }

                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                parametrorequest.Enviroment = vl_Environment;

                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APISAC015001", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time1, vl_Attempts1, vl_Timeout, nombre_metodo);

                if (respuesta.DeliveryProduct != null)
                {       
                    int cantidadDeliveryProduct = respuesta.DeliveryProduct.Count;
                    int i = 0;

                    if (cantidadDeliveryProduct > 0)
                    {
                        do
                        {
                            int cantidadCustAccount = respuesta.DeliveryProduct[i].CustAccount.Length;

                            if (cantidadCustAccount > 0)
                            {
                                respuesta.DeliveryProduct[i].CustAccount = CodigoCliente.DynamicAcrecos(respuesta.DeliveryProduct[i].CustAccount, "APISAC015001");
                            }
                            i++;
                        } while (i < cantidadDeliveryProduct);
                    }

                }

                if (respuesta == null)
                {
                    respuesta = new APISAC015001MessageResponse();
                    respuesta.ErrorList = new List<string>();

                    respuesta.ErrorList.Add("ISAC015:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    Logger.FileLogger("APISAC015001", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);

                }

                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APISAC015001", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new APISAC015001MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("ISAC015:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");
                Logger.FileLogger("APISAC015001", "CONTROLADOR: ERROR: " + ex.ToString());
                return Ok(respuesta);

            }

        }

    }
}