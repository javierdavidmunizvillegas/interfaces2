using System;
using System.Threading.Tasks;
using Interface.Api.Infraestructura.Configuracion;
using IVTA007.Infraestructure.Configuration;
using IVTA007.Infraestructure.Services;
using IVTA007.Models;
using IVTA007.Models._001.Request;
using IVTA007.Models._001.Response;
using IVTA007.Models.Homologacion.ResponseHomologacion;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using static IVTA007.Services.ManejadorRequest;

namespace IVTA007
{
    public static class IVTA007001
    {
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService001");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService001");
        private static string sbQueueResponse = Environment.GetEnvironmentVariable("QueueResponse001");
        private static string sbConectionStringSend = Environment.GetEnvironmentVariable("ConectionStringResponse");
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static int vl_Attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static string vl_parUsername = Environment.GetEnvironmentVariable("ParUsername");
        private static string vl_parPass = Environment.GetEnvironmentVariable("ParPassword");
        private static string vl_parToken = Environment.GetEnvironmentVariable("ParApptoken");
        private static string vl_valUsername = Environment.GetEnvironmentVariable("ValUsername");
        private static string vl_valPass = Environment.GetEnvironmentVariable("ValPassword");
        private static string vl_valToken = Environment.GetEnvironmentVariable("ValApptoken");

        [FunctionName("APIVTA007001")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest")] string myQueueItem, ILogger log)
        {//FUNCIONA 

            try
            {
                Logger.FileLogger("APIVTA007001", "Procesando Función");

               
                APIVTA007001MessageResponse objResponse = null;
                var APIVTA007001Request = JsonConvert.DeserializeObject<APIVTA007001MessageRequest>(myQueueItem);
                APIVTA007001Request.Enviroment = vl_Environment;

                string jsonData = JsonConvert.SerializeObject(APIVTA007001Request);
                Logger.FileLogger("APIVTA007001", "REQUEST RECIBIDO DE DYNAMICS: " + jsonData);

               
                jsonData = JsonConvert.SerializeObject(APIVTA007001Request);

                ConsumoWebService<APIVTA007001MessageResponse> cws = new ConsumoWebService<APIVTA007001MessageResponse>();
                objResponse = await cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData, vl_Time, vl_Attempts, vl_parUsername, vl_parPass, vl_parToken, vl_valUsername, vl_valPass, vl_valToken);//concluido

                string consultado = JsonConvert.SerializeObject(objResponse);

                if (consultado == "null")
                {
                    Logger.FileLogger("APIVTA007001", "WS LEGADO: No se retorno resultado de WS");
                }
                else
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);

                    Logger.FileLogger("APIVTA007001", "RESULTADO RECIBIDO WS: " + jsonrequest);

                    IManejadorRequest<APIVTA007001MessageResponse> _manejadorRequestQueue = new ManejadorRequest<APIVTA007001MessageResponse>();

                    await _manejadorRequestQueue.EnviarMensajeAsync(APIVTA007001Request.SessionId, sbConectionStringSend, sbQueueResponse, objResponse);
                    string mensaje = JsonConvert.SerializeObject(objResponse);
                    Logger.FileLogger("APIVTA007001", "REQUEST ENVIADO A DYNAMICS: " + mensaje);

                }


            }
            catch (Exception ex)
            {
                Logger.FileLogger("APIVTA007001", "ERROR: " + ex.ToString());
            }

        }
    }
}