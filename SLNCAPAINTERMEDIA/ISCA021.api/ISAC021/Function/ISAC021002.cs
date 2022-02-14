using Interface.Api.Infraestructura.Configuracion;
using ISAC021.Infraestructure.Configuration;
using ISAC021.Infraestructure.Services;
using ISAC021.Models;
using ISAC021.Models._002.Request;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace ISAC021.Function
{
    class ISAC021002
    {
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService");
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static int vl_Attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        [FunctionName("APISAC021002")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")] string myQueueItem, ILogger log)
        {

            try
            {

                Logger.FileLogger("APISAC021002", "Procesando Función");

                var APISAC021001Request = JsonConvert.DeserializeObject<APISAC021002MessageResponse>(myQueueItem);
                APISAC021001Request.Enviroment = vl_Environment;

                string jsonData = JsonConvert.SerializeObject(APISAC021001Request);

                Logger.FileLogger("APISAC021002", "REQUEST RECIBIDO: " + jsonData);

                ConsumoWebService<ResponseWS> cws = new ConsumoWebService<ResponseWS>();
                ResponseWS objResponse = await cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData, vl_Time, vl_Attempts);

                if (objResponse == null)
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);
                    Logger.FileLogger("APISAC021002", "WS LEGADO: No se retorno resultado de WS: " + jsonrequest);
                }
                else
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);

                    Logger.FileLogger("APISAC021002", "RESULTADO RECIBIDO WS: " + jsonrequest);

                }


            }
            catch (Exception ex)
            {
                Logger.FileLogger("APISAC021002", "ERROR: " + ex.ToString());
            }

        }
    }
}
