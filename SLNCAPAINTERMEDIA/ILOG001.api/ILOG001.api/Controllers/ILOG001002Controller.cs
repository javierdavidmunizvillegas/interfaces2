using ILOG001.api.Infraestructura.Servicios;
using ILOG001.api.Infraestructure.Configuration;
using ILOG001.api.Infraestructure.Services;
using ILOG001.api.Models._002.Request;
using ILOG001.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ILOG001.api.Controllers
{
    [ApiController]
    public class ILOG001002Controller : ControllerBase
    {
        private IManejadorRequest<APILOG001002MessageRequest> QueueRequest;
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest2").Value.ToString();
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep").Value);
        private static int vl_Attempts = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout").Value);
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string vl_DataAreaid = Convert.ToString(configuracion.GetSection("Data").GetSection("DATA_AREA_ID").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);

        public ILOG001002Controller(IManejadorRequest<APILOG001002MessageRequest> _manejadorRequest,
             IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            homologacionRequest = _homologacionRequest;
        }


        [HttpPost]
        [Route("APILOG001002")]
        public async Task APILOG001002(WeHooksList2D[] parametrorequest)
        {
            try
            {
                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APILOG001002", "CONTROLADOR: Request Recibido: " + jsonrequest);

                APILOG001002MessageRequest request = new APILOG001002MessageRequest();
                List<WeHooksList2> lista = new List<WeHooksList2>();

                WeHooksList2 WH = new WeHooksList2();

                foreach (var p in parametrorequest)
                {
                    WH.eventCreatedAt = Convert.ToDateTime(p.eventCreatedAt);
                    WH.eventType = p.eventType;
                    WH.id = p.id;
                    WH.notes = p.notes;
                    WH.routeId = p.routeId;
                    WH.shipperId = p.shipperId.ToString();

                }

                lista.Add(WH);


                request.weHooks = lista;

                request.DataAreaId = vl_DataAreaid;
                string sesionid = Guid.NewGuid().ToString();
                request.SessionId = sesionid;
                request.Enviroment = vl_Environment;

                string jsonrequest2 = JsonConvert.SerializeObject(request);
                Logger.FileLogger("APILOG001002", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, request);

            }
            catch (Exception ex)
            {

                Logger.FileLogger("APILOG001002", "CONTROLADOR: ERROR: " + ex.ToString());

            }

        }
    }
}