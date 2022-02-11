using DCAJ003.Infraestructure.Configuration;
using DCAJ003.Infraestructure.Services;
using DCAJ003.Models._001.Request;
using DCAJ003.Models._001.Response;
using Interface.Api.Infraestructura.Configuracion;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static DCAJ003.Services.ManejadorRequest;

namespace DCAJ003.Function
{
    public static class DCAJ003001
    {
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService001");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService001");
        private static string sbUriHomologacionDynamic = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriSiac = Environment.GetEnvironmentVariable("MetodoWsUriSiac");
        private static string sbMetodoWsUriAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string sbQueueResponse = Environment.GetEnvironmentVariable("QueueResponse");
        private static string sbConectionStringSend = Environment.GetEnvironmentVariable("ConectionStringResponse");
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static int vl_Attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");



        [FunctionName("APDCAJ003001")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest%", Connection = "ConectionStringRequest")] string myQueueItem, ILogger log)
        {//FUNCIONA 

            try
            {

                Logger.FileLogger("APDCAJ003001", "Procesando Función");

                string nombre_metodo = "APDCAJ003001";

                APDCAJ003001MessageResponse objResponse = null;
                var APDCAJ003001Request = JsonConvert.DeserializeObject<APDCAJ003001MessageRequest>(myQueueItem);
                APDCAJ003001Request.Enviroment = vl_Environment;

                string jsonData = JsonConvert.SerializeObject(APDCAJ003001Request);
                Logger.FileLogger("APDCAJ003001", "REQUEST RECIBIDO DE DYNAMICS: " + jsonData);


                jsonData = JsonConvert.SerializeObject(APDCAJ003001Request);

                ConsumoWebService<APDCAJ003001MessageResponse> cws = new ConsumoWebService<APDCAJ003001MessageResponse>();
                objResponse = await cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData, vl_Time, vl_Attempts, nombre_metodo);//concluido
               
                string consultado = JsonConvert.SerializeObject(objResponse);

                if (consultado == "null")
                {
                    Logger.FileLogger("APDCAJ003001", "WS LEGADO: No se retorno resultado de WS");

                }
                else
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);

                    Logger.FileLogger("APDCAJ003001", "RESULTADO RECIBIDO WS: " + jsonrequest);

                    IManejadorRequest<APDCAJ003001MessageResponse> _manejadorRequestQueue = new ManejadorRequest<APDCAJ003001MessageResponse>();


                    await _manejadorRequestQueue.EnviarMensajeAsync(APDCAJ003001Request.SessionId, sbConectionStringSend, sbQueueResponse, objResponse);
                    objResponse.SessionId = APDCAJ003001Request.SessionId;
                    
                    string mensaje = JsonConvert.SerializeObject(objResponse);
                    Logger.FileLogger("APDCAJ003001", "RESPONSE ENVIADO A LEGADO: " + mensaje);

                }


            }
            catch (Exception ex)
            {
                Logger.FileLogger("APDCAJ003001", "ERROR: " + ex.ToString());
            }

        }
    }
}
