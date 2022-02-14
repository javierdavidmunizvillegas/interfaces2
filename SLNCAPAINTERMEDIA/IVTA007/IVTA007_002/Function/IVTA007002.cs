using System;
using System.Threading.Tasks;
using Interface.Api.Infraestructura.Configuracion;
using IVTA007.Infraestructure.Configuration;
using IVTA007.Infraestructure.Services;
using IVTA007.Models;
using IVTA007.Models._002.Request;
using IVTA007.Models.Homologacion.ResponseHomologacion;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IVTA007
{
    public static class IVTA007002
    {
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService002");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService002");
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static int vl_Attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static string vl_parUsername = Environment.GetEnvironmentVariable("ParUsername");
        private static string vl_parPass = Environment.GetEnvironmentVariable("ParPassword");
        private static string vl_parToken = Environment.GetEnvironmentVariable("ParApptoken");
        private static string vl_valUsername = Environment.GetEnvironmentVariable("ValUsername");
        private static string vl_valPass = Environment.GetEnvironmentVariable("ValPassword");
        private static string vl_valToken = Environment.GetEnvironmentVariable("ValApptoken");

        [FunctionName("APIVTA007002")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest002%", Connection = "ConectionStringRequest")] string myQueueItem, ILogger log)
        {

            try
            {

                Logger.FileLogger("APIVTA007002", "Procesando Función");

                var APIVTA007002Request = JsonConvert.DeserializeObject<APIVTA007002MessageRequest>(myQueueItem);
                APIVTA007002Request.Enviroment = vl_Environment;

                string jsonData = JsonConvert.SerializeObject(APIVTA007002Request);

                Logger.FileLogger("APIVTA007002", "REQUEST RECIBIDO: " + jsonData);

                ConsumoWebService<ResponseWS> cws = new ConsumoWebService<ResponseWS>();
                ResponseWS objResponse = await cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData, vl_Time, vl_Attempts, vl_parUsername, vl_parPass, vl_parToken, vl_valUsername, vl_valPass, vl_valToken);

                if (objResponse == null)
                {

                    Logger.FileLogger("APIVTA007002", "WS LEGADO: No se retorno resultado de WS");

                }
                else
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);

                    Logger.FileLogger("APIVTA007002", "RESULTADO RECIBIDO WS: " + jsonrequest);

                }


            }
            catch (Exception ex)
            {
                Logger.FileLogger("APIVTA007002", "ERROR: " + ex.ToString());
            }

        }
    }
}
