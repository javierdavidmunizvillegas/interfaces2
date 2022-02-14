using DCAJ003.Models._002.Request;
using DCAJ003.Models._002.Response;
using DCAJ003_002.Infraestructure.Configuration;
using DCAJ003_002.Infraestructure.Services;
using Interface.Api.Infraestructura.Configuracion;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static DCAJ003_002.Services.ManejadorRequest;

namespace DCAJ003_002.Function
{
    public static class DCAJ003002
    {
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService002");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService002");
        private static string sbUriHomologacionDynamic = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriSiac = Environment.GetEnvironmentVariable("MetodoWsUriSiac");
        private static string sbMetodoWsUriAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string sbQueueResponse = Environment.GetEnvironmentVariable("QueueResponse002");
        private static string sbConectionStringSend = Environment.GetEnvironmentVariable("ConectionStringResponse");
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static int vl_Attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static int vl_Time2 = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep2"));
        private static int vl_Attempts2 = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts2"));
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        [FunctionName("APDCAJ003002")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest002%", Connection = "ConectionStringRequest")] string myQueueItem, ILogger log)
        {//FUNCIONA 

            try
            {

                Logger.FileLogger("APDCAJ003002", "Procesando Función");

                string nombre_metodo = "APDCAJ003002";

                APDCAJ003002MessageResponse objResponse = null;
                var APDCAJ003002Request = JsonConvert.DeserializeObject<APDCAL003002MessageRequest>(myQueueItem);
                APDCAJ003002Request.Enviroment = vl_Environment;

                string jsonData = JsonConvert.SerializeObject(APDCAJ003002Request);
                Logger.FileLogger("APDCAJ003002", "REQUEST RECIBIDO DE DYNAMICS: " + jsonData);


                jsonData = JsonConvert.SerializeObject(APDCAJ003002Request);

                ConsumoWebService<APDCAJ003002MessageResponse> cws = new ConsumoWebService<APDCAJ003002MessageResponse>();
                objResponse = await cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData, vl_Time, vl_Attempts, nombre_metodo);//concluido

                string consultado = JsonConvert.SerializeObject(objResponse);

                if (consultado == "null")
                {

                    Logger.FileLogger("APDCAJ003002", "WS LEGADO: No se retorno resultado de WS");

                }
                else
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);

                    Logger.FileLogger("APDCAJ003002", "RESULTADO RECIBIDO WS: " + jsonrequest);

                    IManejadorRequest<APDCAJ003002MessageResponse> _manejadorRequestQueue = new ManejadorRequest<APDCAJ003002MessageResponse>();


                    await _manejadorRequestQueue.EnviarMensajeAsync(APDCAJ003002Request.SessionId, sbConectionStringSend, sbQueueResponse, objResponse);
                    objResponse.SessionId = APDCAJ003002Request.SessionId;
                    string mensaje = JsonConvert.SerializeObject(objResponse);
                    
                    Logger.FileLogger("APDCAJ003002", "RESPONSE ENVIADO A LEGADO: " + mensaje);

                }


            }
            catch (Exception ex)
            {
                Logger.FileLogger("APDCAJ003002", "ERROR: " + ex.ToString());
            }

        }
    }
}
