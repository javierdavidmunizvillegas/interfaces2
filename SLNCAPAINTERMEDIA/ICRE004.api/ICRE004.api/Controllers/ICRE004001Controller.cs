using ICRE004.api.Infraestructura.Servicios;
using ICRE004.api.Infraestructure.Configuration;
using ICRE004.api.Infraestructure.Services;
using ICRE004.api.Models._001.Request;
using ICRE004.api.Models._001.Response;
using ICRE004.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ICRE004.api.Controllers
{
    
    [ApiController]
    public class ICRE004001Controller : ControllerBase
    {
        private IManejadorRequest<APICRE004001MessageRequest> QueueRequest;
        private IManejadorResponse<APICRE004001MessageResponse> QueueResponse;
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

        public ICRE004001Controller(IManejadorRequest<APICRE004001MessageRequest> _manejadorRequest
             , IManejadorResponse<APICRE004001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }

                
        [HttpPost]
        [Route("APICRE004001")]
        [ServiceFilter(typeof(ValidadorRequest))]
        public async Task<ActionResult<APICRE004001MessageResponse>> APICRE004001(APICRE004001MessageRequest parametrorequest)
        {
            APICRE004001MessageResponse respuesta = null;
            try
            {

                string nombre_metodo = "APICRE004001";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICRE004001", "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);

                if (ResultadoHomologa == null)
                {
                    respuesta = new APICRE004001MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICRE004:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APICRE004001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

                    return Ok(respuesta);
                }
                else
                {
                    if (ResultadoHomologa.Response != null)
                    {
                        parametrorequest.DataAreaId = ResultadoHomologa.Response;
                    }

                }
               
                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                parametrorequest.Enviroment = vl_Environment;
               
                int cantidad = parametrorequest.CustAccountList.Count;
                int i = 0;

                if (cantidad > 0)
                {
                    do
                    {
                        int cant = parametrorequest.CustAccountList[i].Length;

                        if (cant > 0)
                        {
                            parametrorequest.CustAccountList[i] = CodigoCliente.CrecosAdynamics(parametrorequest.CustAccountList[i], longAllenar, "APICRE004001");

                        }
                        i++;
                    } while (i < cantidad);
                }
               
                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICRE004001", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time1, vl_Attempts1, vl_Timeout, nombre_metodo);


                if (respuesta == null)
                {
                    respuesta = new APICRE004001MessageResponse();
                    respuesta.ErrorList = new List<string>();

                    respuesta.ErrorList.Add("ICRE004:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    Logger.FileLogger("APICRE004001", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);

                }
                else
                {
                    if (respuesta.APSalesOrderList != null)
                    {

                        int cantidad2 = respuesta.APSalesOrderList.Count;
                        int j = 0;

                        if (cantidad2 > 0)
                        {
                            do
                            {
                                int cant2 = respuesta.APSalesOrderList[j].APSalesOrderHeader.CustAccount.Length;

                                if (cant2 > 0)
                                {
                                    respuesta.APSalesOrderList[j].APSalesOrderHeader.CustAccount = Int32.Parse(CodigoCliente.DynamicAcrecos(respuesta.APSalesOrderList[j].APSalesOrderHeader.CustAccount, "APICRE004001")).ToString();
                                }

                                int cant3 = respuesta.APSalesOrderList[j].APSalesOrderHeader.DataAreaId.Length;

                                if (cant3 > 0)
                                {
                                    //Homologación de empresa de Dynamics a Legado
                                    string DataAreaId2 = respuesta.APSalesOrderList[j].APSalesOrderHeader.DataAreaId;
                                    ResponseHomologacion ResultadoHomologa2 = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriAx, DataAreaId2, vl_Time, vl_Attempts, nombre_metodo);

                                    if (ResultadoHomologa2 == null)
                                    {
                                        respuesta = new APICRE004001MessageResponse();
                                        respuesta.ErrorList = new List<string>();
                                        respuesta.ErrorList.Add("ICRE004:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                                        Logger.FileLogger("APICRE004001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");
                                    }
                                    else
                                    {
                                        if (ResultadoHomologa2.Response != null)
                                        {
                                            respuesta.APSalesOrderList[j].APSalesOrderHeader.DataAreaId = ResultadoHomologa2.Response;
                                        }

                                    }

                                }

                                j++;
                            } while (j < cantidad2);
                        }
                    }

                }

                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APICRE004001", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new APICRE004001MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("ICRE004:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");

                Logger.FileLogger("APICRE004001", "CONTROLADOR: ERROR: " + ex.ToString());
                return Ok(respuesta);

            }

        }

    }
}
