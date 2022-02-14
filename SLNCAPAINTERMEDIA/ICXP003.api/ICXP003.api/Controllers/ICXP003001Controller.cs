using ICXP003.api.Infraestructura.Servicios;
using ICXP003.api.Infraestructure.Configuration;
using ICXP003.api.Infraestructure.Services;
using ICXP003.api.Models._001.Request;
using ICXP003.api.Models._001.Response;
using ICXP003.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ICXP003.api.Controllers
{    
    [ApiController]
    public class ICXP003001Controller : ControllerBase
    {
        private IManejadorRequest<APICXP003001MessageRequest> QueueRequest;
        private IManejadorResponse<APICXP003001MessageResponse> QueueResponse;
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
        private static string vl_Environment=Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32(configuracion.GetSection("Data").GetSection("LongAllenar").Value);

        public ICXP003001Controller(IManejadorRequest<APICXP003001MessageRequest> _manejadorRequest
            , IManejadorResponse<APICXP003001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {           
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }


        [HttpPost] 
        [Route("APICXP003001")]
        [ServiceFilter(typeof(ValidadorRequest001))]
        public async Task<ActionResult<APICXP003001MessageResponse>> APICXP003001(APICXP003001MessageRequest parametrorequest)
        {           

            APICXP003001MessageResponse respuesta = null;
            try
            {
                string nombre_metodo = "APICXP003001";
                

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICXP003001", "CONTROLADOR: Request Recibido: " + jsonrequest);

               
                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);

                if (ResultadoHomologa == null)
                {
                    respuesta = new APICXP003001MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICXP003:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APICXP003001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

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

                if (parametrorequest.APCreateVendorContract != null)
                {
                    int cantidad = parametrorequest.APCreateVendorContract.Count;
                    int i = 0;

                    if (cantidad > 0)
                    {
                        do
                        {
                            if (parametrorequest.APCreateVendorContract[i] != null)
                            {
                                if (parametrorequest.APCreateVendorContract[i].AccountNum != null && parametrorequest.APCreateVendorContract[i].AccountNum != "")
                                {
                                    parametrorequest.APCreateVendorContract[i].AccountNum = CodigoCliente.CrecosAdynamics(parametrorequest.APCreateVendorContract[i].AccountNum, longAllenar, "APICXP003001");
                                }

                                if (parametrorequest.APCreateVendorContract[i].ContactInfoList != null)
                                {
                                    int cantidad2 = parametrorequest.APCreateVendorContract[i].ContactInfoList.Count;
                                    int j = 0;

                                    if (cantidad2 > 0)
                                    {
                                        do
                                        {
                                            if (parametrorequest.APCreateVendorContract[i].ContactInfoList[j].Type.Equals(1))
                                            {
                                                parametrorequest.APCreateVendorContract[i].ContactInfoList[j].FacturacionElectronica = false;
                                            }
                                            if (parametrorequest.APCreateVendorContract[i].ContactInfoList[j].Type.Equals(2))
                                            {
                                                parametrorequest.APCreateVendorContract[i].ContactInfoList[j].FacturacionElectronica = true;
                                            }


                                            j++;
                                        } while (j < cantidad2);
                                    }
                                }
                            }
                            
                            i++;
                        } while (i < cantidad);
                    }
                }
           

                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICXP003001", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time1, vl_Attempts1, vl_Timeout, nombre_metodo);
          
                if (respuesta == null)
                {
                    respuesta = new APICXP003001MessageResponse();
                    respuesta.ErrorList = new List<string>();

                    respuesta.ErrorList.Add("ICXP003:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    Logger.FileLogger("APICXP003001", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);
                }
                else
                {
                    if (respuesta.APVendorContractList != null)
                    {
                        int cantidad2 = respuesta.APVendorContractList.Count;
                        int j = 0;

                        if (cantidad2 > 0)
                        {
                            do
                            {
                                if (respuesta.APVendorContractList[j].APCreateVendorContract != null)
                                {
                                    if (respuesta.APVendorContractList[j].APCreateVendorContract.AccountNum != null)
                                    {
                                        respuesta.APVendorContractList[j].APCreateVendorContract.AccountNum = CodigoCliente.DynamicAcrecos(respuesta.APVendorContractList[j].APCreateVendorContract.AccountNum, "APICXP003001");
                                    }
                                }
                               
                                j++;
                            } while (j < cantidad2);
                        }
                    }
                }

                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APICXP003001", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new APICXP003001MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("ICXP003:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");
                Logger.FileLogger("APICXP003001", "CONTROLADOR: ERROR: " + ex.ToString());
                return Ok(respuesta);

            }

        }

    }
}
