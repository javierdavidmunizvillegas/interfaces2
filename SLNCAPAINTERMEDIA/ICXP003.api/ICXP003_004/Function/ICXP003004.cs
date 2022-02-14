using System;
using System.Threading.Tasks;
using ICXP003_004.Infraestructura.Servicios;
using ICXP003_004.Models;
using ICXP003_004.Models.Request;
using Interface.Api.Infraestructura.Configuracion;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ICXP003_004
{
    public static class ICXP003004
    {
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService");
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static int vl_Attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        [FunctionName("APICXP003004")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest001%", Connection = "ConectionStringRequest001")] string myQueueItem, ILogger log)
        {

            try
            {

                Logger.FileLogger("APICXP003004", "Procesando Función");

                var APICXP003004Request = JsonConvert.DeserializeObject<APICXP003004MessageRequest>(myQueueItem);
                APICXP003004Request.Enviroment = vl_Environment;

                ////////////////////HOMOLOGACION/////////////////////////////////////////////////////////////////////
              
                int cantidadFechas = APICXP003004Request.apvendorjournalcontract.Count;
                int i = 0;

                if (cantidadFechas > 0)
                {
                    do
                    {
                        int cant = APICXP003004Request.apvendorjournalcontract[i].transdate.Length;

                        if (cant > 0)
                        {
                            DateTime fecha = DateTime.Parse(APICXP003004Request.apvendorjournalcontract[i].transdate);
                            APICXP003004Request.apvendorjournalcontract[i].transdate = fecha.ToString("dd/MM/yyyy");

                        }
                        i++;
                    } while (i < cantidadFechas);
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////

                string jsonData = JsonConvert.SerializeObject(APICXP003004Request);

                Logger.FileLogger("APICXP003004", "REQUEST RECIBIDO: " + jsonData);

                ConsumoWebService<ResponseWS> cws = new ConsumoWebService<ResponseWS>();
                ResponseWS objResponse = await cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData, vl_Time, vl_Attempts);

                if (objResponse == null)
                {

                    Logger.FileLogger("APICXP003004", "WS LEGADO: No se retorno resultado de WS");

                }
                else
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);

                    Logger.FileLogger("APICXP003004", "RESULTADO RECIBIDO WS: " + jsonrequest);

                }


            }
            catch (Exception ex)
            {
                Logger.FileLogger("APICXP003004", "ERROR: " + ex.ToString());
            }

        }
    }
}
