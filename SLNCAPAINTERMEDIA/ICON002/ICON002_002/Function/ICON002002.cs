using ICON002_002.Infraestructure.Configuration;
using ICON002_002.Infraestructure.Services;
using ICON002_002.Models._002;
using ICON002_002.Models._002.Response;
using Interface.Api.Infraestructura.Configuracion;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ICON002_002.Services.ManejadorRequest;

namespace ICON002.Function
{
    public static class ICON002002
    {
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService002");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService002");
        private static string sbUriHomologacionDynamic = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriSiac = Environment.GetEnvironmentVariable("MetodoWsUriSiac");
        private static string sbMetodoWsUriAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string sbQueueResponse = Environment.GetEnvironmentVariable("QueueResponse2");
        private static string sbConectionStringSend = Environment.GetEnvironmentVariable("ConectionStringResponse");
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static int vl_Attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));      
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        [FunctionName("APICON002002")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest2%", Connection = "ConectionStringRequest")] string myQueueItem, ILogger log)
        {//FUNCIONA 

            try
            {

                Logger.FileLogger("APICON002002", "Procesando Función");

                APICON002002MessageResponse objResponse = null;
                var APICON002002Request = JsonConvert.DeserializeObject<APICON002002MessageRequest>(myQueueItem);
                APICON002002Request.Enviroment = vl_Environment;

                string jsonData = JsonConvert.SerializeObject(APICON002002Request);
                Logger.FileLogger("APICON002002", "REQUEST RECIBIDO DE DYNAMICS: " + jsonData);


                jsonData = JsonConvert.SerializeObject(APICON002002Request);
                string FacturaId = APICON002002Request.FacturaId;

                ConsumoWebService<APICON002002MessageResponse> cws = new ConsumoWebService<APICON002002MessageResponse>();
                objResponse = await cws.GetWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, FacturaId, vl_Time, vl_Attempts);//concluido
                objResponse.SessionId = APICON002002Request.SessionId;

                string consultado = JsonConvert.SerializeObject(objResponse);

                if (consultado == "null")
                {

                    Logger.FileLogger("APICON002002", "WS LEGADO: No se retorno resultado de WS");

                }
                else
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);

                    Logger.FileLogger("APICON002002", "RESULTADO RECIBIDO WS: " + jsonrequest);

                    IManejadorRequest<APICON002002MessageResponse> _manejadorRequestQueue = new ManejadorRequest<APICON002002MessageResponse>();


                    await _manejadorRequestQueue.EnviarMensajeAsync(objResponse.SessionId, sbConectionStringSend, sbQueueResponse, objResponse);
                    string mensaje = JsonConvert.SerializeObject(objResponse);
                    Logger.FileLogger("APICON002002", "REQUEST ENVIADO A DYNAMICS: " + mensaje);

                }


            }
            catch (Exception ex)
            {
                Logger.FileLogger("APICON002002", "ERROR: " + ex.ToString());
            }

        }
    }
}
