using ICRE008.api.Infraestructura.Servicios;
using ICRE008.api.Infraestructure.Configuration;
using ICRE008.api.Infraestructure.Services;
using ICRE008.api.Models._001.Request;
using ICRE008.api.Models._001.Response;
using ICRE008.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE008.api.Controllers
{
    
    [ApiController]
    public class ICRE009001Controller : ControllerBase
    {
        private IManejadorRequest<APICRE009001MessageRequest> QueueRequest;
        private IManejadorResponse<APICRE009001MessageResponse> QueueResponse;
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

        public ICRE009001Controller(IManejadorRequest<APICRE009001MessageRequest> _manejadorRequest
            , IManejadorResponse<APICRE009001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APICRE009001")]
        [ServiceFilter(typeof(ValidadorRequest))]
        public async Task<ActionResult<APICRE009001MessageResponse>> APICRE009001(APICRE009001MessageRequest parametrorequest)
        {
            APICRE009001MessageResponse respuesta = null;
            try
            {

                string nombre_metodo = "APICRE009001";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICRE009001", "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);

                if (ResultadoHomologa == null)
                {
                    respuesta = new APICRE009001MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICRE009:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APICRE009001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

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

                if (parametrorequest.CustAccount != null && parametrorequest.CustAccount != "") {
                    parametrorequest.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.CustAccount, longAllenar, "APICRE009001");
                }
              

                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICRE009001", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time1, vl_Attempts1, vl_Timeout, nombre_metodo);


                if (respuesta == null)
                {
                    respuesta = new APICRE009001MessageResponse();
                    respuesta.ErrorList = new List<string>();

                    respuesta.ErrorList.Add("ICRE009:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    Logger.FileLogger("APICRE009001", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);

                }
                else
                {
                    if (respuesta.apicrE009001InvoiceOrderList != null) { 
                        int cantidad = respuesta.apicrE009001InvoiceOrderList.Count;
                        int i = 0;

                        if (cantidad > 0)
                        {
                            do
                            {
                                if (respuesta.apicrE009001InvoiceOrderList[i].APInvoice.DataAreaId != null && respuesta.apicrE009001InvoiceOrderList[i].APInvoice.DataAreaId != "")
                                {
                                    int cant = respuesta.apicrE009001InvoiceOrderList[i].APInvoice.DataAreaId.Length;

                                    if (cant > 0)
                                    {
                                        //Homologación de empresa de Dynamics a Legado
                                        string DataAreaId2 = respuesta.apicrE009001InvoiceOrderList[i].APInvoice.DataAreaId;
                                        ResponseHomologacion ResultadoHomologa2 = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriAx, DataAreaId2, vl_Time, vl_Attempts, nombre_metodo);

                                        if (ResultadoHomologa2 == null)
                                        {
                                            respuesta = new APICRE009001MessageResponse();
                                            respuesta.ErrorList = new List<string>();
                                            respuesta.ErrorList.Add("ICRE009:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                                            Logger.FileLogger("APICRE009001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");
                                        }
                                        else
                                        {
                                            if (ResultadoHomologa2.Response != null)
                                            {
                                                respuesta.apicrE009001InvoiceOrderList[i].APInvoice.DataAreaId = ResultadoHomologa2.Response;
                                            }

                                        }
                                    }
                                }
                                //Homologación de cliente de Dynamics a legado

                                if (respuesta.apicrE009001InvoiceOrderList[i].APSalesTable.CustAccount != null) {
                                    int cant2 = respuesta.apicrE009001InvoiceOrderList[i].APSalesTable.CustAccount.Length;

                                    if (cant2 > 0)
                                    {
                                        respuesta.apicrE009001InvoiceOrderList[i].APSalesTable.CustAccount = Int32.Parse(CodigoCliente.DynamicAcrecos(respuesta.apicrE009001InvoiceOrderList[i].APSalesTable.CustAccount, "APICRE009001")).ToString();                               
                                    }
                                }
                                i++;
                            } while (i < cantidad);
                        }
                    }
                }

                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APICRE009001", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new APICRE009001MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("ICRE009:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");

                Logger.FileLogger("APICRE009001", "CONTROLADOR: ERROR: " + ex.ToString());
                return Ok(respuesta);

            }

        }
    }
}
