using System;
using System.Threading.Tasks;
using Interface.Api.Infraestructura.Configuracion;
using IVTA018_02.Infraestructure.Configuration;
using IVTA018_02.Infraestructure.Services;
using IVTA018_02.Models._002.Response;
using IVTA018_02.Models.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IVTA018_02
{
    public static class IVTA018002F
    {
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService");
        private static string sbUriHomologacionDynamic = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriSiac = Environment.GetEnvironmentVariable("MetodoWsUriSiac");
        private static string sbMetodoWsUriAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");  
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static int vl_Attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


        [FunctionName("APIVTA018002F")]
        public static async Task Run([ServiceBusTrigger("%QueueResponse%", Connection = "ConectionStringResponse")] string myQueueItem, ILogger log)
        {

            try
            {

                Logger.FileLogger("APIVTA018002F", "Procesando Función");

                ResponseWS objResponse = null;
                var APIVTA018002Request = JsonConvert.DeserializeObject<APIVTA018002MessageResponse>(myQueueItem);
                //APIVTA018002Request.Enviroment = vl_Environment;

                string jsonData = JsonConvert.SerializeObject(APIVTA018002Request);
                Logger.FileLogger("APIVTA018002F", "REQUEST RECIBIDO DE DYNAMICS: " + jsonData);

               
                jsonData = JsonConvert.SerializeObject(APIVTA018002Request);

                ConsumoWebService<ResponseWS> cws = new ConsumoWebService<ResponseWS>();
                objResponse = await cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData, vl_Time, vl_Attempts);

                if (objResponse == null)
                {
                    Logger.FileLogger("APIVTA018002F", "WS PRECIOS: No se insertó el registro de precios");
                }
                else
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);

                    Logger.FileLogger("APIVTA018002F", "RESULTADO RECIBIDO WS: " + jsonrequest);

                }

            }
            catch (Exception ex)
            {
                Logger.FileLogger("APIVTA018002F", "ERROR: " + ex.ToString());
            }

        }
    }

}
