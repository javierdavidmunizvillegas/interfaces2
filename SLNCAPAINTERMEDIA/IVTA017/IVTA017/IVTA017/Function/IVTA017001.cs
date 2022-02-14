using Interface.Api.Infraestructura.Configuracion;
using IVTA017.Infraestructure.Configuration;
using IVTA017.Infraestructure.Services;
using IVTA017.Models._001.Request;
using IVTA017.Models._002.Response;

using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace IVTA017.Function
{
    class IVTA017001
    {
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService");
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static int vl_Attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");


        [FunctionName("APIVTA017001")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")] string myQueueItem, ILogger log)
        {

            try
            {

                Logger.FileLogger("APIVTA017001", "Procesando Función");

                var APIVTA017001Request = JsonConvert.DeserializeObject<APIVTA017001MessageRequest>(myQueueItem);
                APIVTA017001Request.Enviroment = vl_Environment;

                string jsonData = JsonConvert.SerializeObject(APIVTA017001Request);

                Logger.FileLogger("APIVTA017001", "REQUEST RECIBIDO: " + jsonData);

                ConsumoWebService<ResponseWS> cws = new ConsumoWebService<ResponseWS>();
                ResponseWS objResponse = await cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData, vl_Time, vl_Attempts);

                if (objResponse == null)
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);
                    Logger.FileLogger("APIVTA017001", "WS LEGADO: No se retorno resultado de WS: " + jsonrequest);
                }
                else
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);

                    Logger.FileLogger("APIVTA017001", "RESULTADO RECIBIDO WS: " + jsonrequest);

                }


            }
            catch (Exception ex)
            {
                Logger.FileLogger("APIVTA017001", "ERROR: " + ex.ToString());
            }

        }
    }
}
