/*
 Objetivo: Interfaz para Preparación de depósitos de cheques, confirmación de depósitos de cheques y anulaciones de cheques en la gestión de cheques.
 Archivo: DCAJ017001Controller.cs
 Versión: 1.0
 Creación: 07/03/2022
 Autor: Solange Moncada
*/

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

namespace DCAJ017.api.Controllers
{
   
    [ApiController]
    public class DCAJ017001Controller : ControllerBase
    {
        /*Variable para invocar al manejador del request*/
        private IManejadorRequest<APDCAJ017001MessageRequest> QueueRequest;
        /*Variable para invocar al manejador del response*/
        private IManejadorResponse<APDCAJ017001MessageResponse> QueueResponse;
        /*Variable para invocar al manejador de homologación*/
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        /*Objeto de la clase RegistroLog*/
        private static RegistroLog Logger = new RegistroLog();
        /*Objeto de la clase configuración que hace referencia al appsettings*/
        private static readonly Configuracion conf = new Configuracion();
        /*Variable de la clase configuración que hace referencia al appsettings*/
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        /*Variable bus request configurada en appsettings*/
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        /*Variable bus response configurada en appsettings*/
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        /*Variable cola request configurada en appsettings*/
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest").Value.ToString();
        /*Variable cola response configurada en appsettings*/
        private static string vl_QueueResponse = configuracion.GetSection("Data").GetSection("QueueResponse").Value.ToString();
        /*Variable para tiempo del manejador homologación configurada en appsettings*/
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep").Value);
        /*Variable para intentos del manejador homologación configurada en appsettings*/
        private static int vl_Attempts = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts").Value);
        /*Variable para tiempos del manejador response configurada en appsettings*/
        private static int vl_Time1 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep1").Value);
        /*Variable para intentos del manejador response configurada en appsettings*/
        private static int vl_Attempts1 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts1").Value);
        /*Variable para tiempo espera general del response configurada en appsettings*/
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout").Value);
        /*Variable para ambiente configurada en appsettings*/
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        /*Variable url de homologación general en appsettings*/
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        /*Variable ruta de método de homologación para SIAC a Dynamics en appsettings*/
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        /*Objeto de la clase ConvierteCodigo.*/
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        /*Variable para configurar la cantidad de dígitos que debe tener el campo Código de cliente*/
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
        /*Llamada al ValidadorRequest001(Valida campos obligatorios de las clases del Controlador 1 si los hubiera)*/
        [ServiceFilter(typeof(ValidadorRequest001))]
        public async Task<ActionResult<APDCAJ017001MessageResponse>> APDCAJ017001(APDCAJ017001MessageRequest parametrorequest)
        {
            /*Variable para guardar la respuesta de la cola respose*/
            APDCAJ017001MessageResponse respuesta = null;
            /*Variable para escribir el nombre del controlador en el log*/
            string nombre_metodo;
            /*Variable para codigo de empresa*/
            string DataAreaId;
            /*Variable para guardar la respuesta de homologación*/
            ResponseHomologacion resultadoHomologa;
            /*Variable para id de sesión*/
            string sesionid;
            /*Variable para guardar el json de request*/
            string jsonrequest2;
            /*Variable para guardar el json de response*/
            string jsonresponse;

            try
            {
                nombre_metodo = "APDCAJ017001";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APDCAJ017001", "CONTROLADOR: Request Recibido: " + jsonrequest);
                                
                DataAreaId = parametrorequest.DataAreaId;

                /*Llamada al método GetHomologacion del Manejador Homologación para hacer la conversión de Siac a Dynamics del código de empresa*/
                resultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);

                if (resultadoHomologa == null)
                {
                    respuesta = new APDCAJ017001MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("DCAJ017:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APDCAJ017001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

                    return Ok(respuesta);
                }
                else 
                {
                    if (resultadoHomologa.Response != null)
                    {
                        parametrorequest.DataAreaId = resultadoHomologa.Response;
                    }
                }                

                sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                /*Asigna el ambiente configurado en appsettings.json*/
                parametrorequest.Environment = vl_Environment;

                
                if (parametrorequest.CustAccount!=null && parametrorequest.CustAccount != "")
                {
                    /*Llama al médodo CrecosAdynamics de la clase ConvierteCódigo, donde le va a agregar la C al código del cliente*/
                    parametrorequest.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.CustAccount, longAllenar, "APDCAJ017001");

                }

                if (parametrorequest.APVendTransRegistrationFine != null)
                {
                    if (parametrorequest.APVendTransRegistrationFine.CustAccount != null && parametrorequest.APVendTransRegistrationFine.CustAccount != "")
                    {
                        /*Llama al médodo CrecosAdynamics de la calse ConvierteCódigo, donde le va a agregar la C al código del cliente*/
                        parametrorequest.APVendTransRegistrationFine.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.APVendTransRegistrationFine.CustAccount, longAllenar, "APDCAJ017001");
                    }
                }

                jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APDCAJ017001", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                /*Llama al médodo EnviarMensajeAsync del ManejadoRequest, donde va a enviar el mensaje a la cola de request (Mensaje a Dynamics)*/
                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                /*Llama al médodo RecibeMensajeSesion del ManejadoResponse, donde va a recibir la respuesta de la cola response (Respuesta de Dynamics a Crecos)*/
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
                        /*Llama al médodo DynamicAcrecos de la clase ConvierteCódigo, donde le va a quitar la C al código del cliente*/
                        respuesta.CustAccount = CodigoCliente.DynamicAcrecos(respuesta.CustAccount, "APDCAJ017001");
                    }
                }

                jsonresponse = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APDCAJ017001", "CONTROLADOR: Response desde Dynamics: " + jsonresponse);
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
