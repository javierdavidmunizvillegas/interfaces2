using ICON002.Infraestructure.Configuration;
using ICON002.Infraestructure.Services;
using ICON002_001.Models._001.Request;
using ICON002_001.Models._001.Response;
using Interface.Api.Infraestructura.Configuracion;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static ICON002.Services.ManejadorRequest;

namespace ICON002_001.Function
{
    class ICON002001
    {
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService001");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService001");
        private static string sbUriHomologacionDynamic = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriSiac = Environment.GetEnvironmentVariable("MetodoWsUriSiac");
        private static string sbMetodoWsUriAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string sbQueueResponse = Environment.GetEnvironmentVariable("QueueResponse1");
        private static string sbConectionStringSend = Environment.GetEnvironmentVariable("ConectionStringResponse");
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static int vl_Attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static int vl_Time2 = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep2"));
        private static int vl_Attempts2 = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts2"));
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        [FunctionName("APICON002001")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest1%", Connection = "ConectionStringRequest")] string myQueueItem, ILogger log)
        {//FUNCIONA 

            try
            {

                Logger.FileLogger("APICON002001", "Procesando Función");

                APCON002001MessageResponse objResponse = null;
                var APICON002001Request = JsonConvert.DeserializeObject<APICON002001MessageRequest>(myQueueItem);
                APICON002001Request.Enviroment = vl_Environment;

                string jsonData = JsonConvert.SerializeObject(APICON002001Request);
                Logger.FileLogger("APICON002001", "REQUEST RECIBIDO DE DYNAMICS: " + jsonData);


                jsonData = JsonConvert.SerializeObject(APICON002001Request);

                ConsumoWebService<APCON002001MessageResponse> cws = new ConsumoWebService<APCON002001MessageResponse>();
                objResponse = await cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData, vl_Time, vl_Attempts);//concluido
                objResponse.SessionId = APICON002001Request.SessionId;
                    
                string consultado = JsonConvert.SerializeObject(objResponse);

                if (consultado == "null")
                {

                    Logger.FileLogger("APICON002001", "WS LEGADO: No se retorno resultado de WS");

                }
                else
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);

                    Logger.FileLogger("APICON002001", "RESULTADO RECIBIDO WS: " + jsonrequest);

                    IManejadorRequest<APCON002001MessageResponse> _manejadorRequestQueue = new ManejadorRequest<APCON002001MessageResponse>();


                    await _manejadorRequestQueue.EnviarMensajeAsync(APICON002001Request.SessionId, sbConectionStringSend, sbQueueResponse, objResponse);
                    string mensaje = JsonConvert.SerializeObject(objResponse);
                    Logger.FileLogger("APICON002001", "REQUEST ENVIADO A DYNAMICS: " + mensaje);

                }


            }
            catch (Exception ex)
            {
                Logger.FileLogger("APICON002001", "ERROR: " + ex.ToString());
            }

        }
    }
}
