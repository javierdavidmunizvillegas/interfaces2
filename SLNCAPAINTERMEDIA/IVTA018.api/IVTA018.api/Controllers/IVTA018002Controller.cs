using IVTA018.api.Infraestructura.Servicios;
using IVTA018.api.Infraestructure.Configuration;
using IVTA018.api.Infraestructure.Services;
using IVTA018.api.Models._002.Request;
using IVTA018.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace IVTA018.api.Controllers
{
    [ApiController]
    public class IVTA018002Controller : ControllerBase
    {
        private IManejadorRequest<APIVTA018002MessageRequest> QueueRequest;       
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest2").Value.ToString();       
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep").Value);
        private static int vl_Attempts = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts").Value);
        private static int vl_Time2 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep2").Value);
        private static int vl_Attempts2 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts2").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout").Value);
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);

        public IVTA018002Controller(IManejadorRequest<APIVTA018002MessageRequest> _manejadorRequest,
             IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            homologacionRequest = _homologacionRequest;
        }
       


        [HttpPost]
        [Route("APIVTA018002")]
        [ServiceFilter(typeof(ValidadorRequest))]
        public async Task APIVTA018002(APIVTA018002MessageRequest parametrorequest)
        {           
            try
            {

                string nombre_metodo = "APIVTA018002";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APIVTA018002", "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);

                if (ResultadoHomologa == null)
                {                   

                    Logger.FileLogger("APIVTA018002", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");
                                      
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

                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APIVTA018002", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);
              
            }
            catch (Exception ex)
            {             
            
                Logger.FileLogger("APIVTA018002", "CONTROLADOR: ERROR: " + ex.ToString());             

            }

        }

    }
}