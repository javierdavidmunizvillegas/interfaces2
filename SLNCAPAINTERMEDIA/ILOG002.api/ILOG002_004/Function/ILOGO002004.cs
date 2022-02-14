using System;
using System.Threading.Tasks;
using ILOG002_001.Models._001.Request;
using ILOG002_004.Infraestructure.Configuration;
using ILOG002_004.Infraestructure.Services;
using ILOG002_004.Models._001.Response;
using ILOG002_004.Models._002.Request;
using ILOG002_004.Models._002.Response;
using ILOG002_004.Models.Homologacion.ResponseHomologacion;
using Interface.Api.Infraestructura.Configuracion;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ILOG002_004
{
    public static class ILOGO002004
    {
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string sbUriConsumoWebService = Environment.GetEnvironmentVariable("UriConsumoWebService001");
        private static string sbMetodoWsUriConsumowebService = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService001");
        private static string sbUriConsumoWebService2 = Environment.GetEnvironmentVariable("UriConsumoWebService002");
        private static string sbMetodoWsUriConsumowebService2 = Environment.GetEnvironmentVariable("MetodoWsUriConsumowebService002");
        private static string sbUriHomologacionDynamic = Environment.GetEnvironmentVariable("UriHomologacionDynamicSiac");
        private static string sbMetodoWsUriSiac = Environment.GetEnvironmentVariable("MetodoWsUriSiac");
        private static string sbMetodoWsUriAx = Environment.GetEnvironmentVariable("MetodoWsUriAx");
        private static string sbQueueResponse = Environment.GetEnvironmentVariable("QueueResponse004");
        private static string sbConectionStringSend = Environment.GetEnvironmentVariable("ConectionStringResponse");
        private static int vl_Time = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep"));
        private static int vl_Attempts = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts"));
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        private static string vl_varAutorizacion = Environment.GetEnvironmentVariable("ParAuthorization");
        private static string vl_valAutorizacion = Environment.GetEnvironmentVariable("ValAuthorization");
        private static string vl_varEmail = Environment.GetEnvironmentVariable("ParEmail");
        private static string vl_valPass = Environment.GetEnvironmentVariable("ParPassword");
        private static string vl_valToken = Environment.GetEnvironmentVariable("ParToken");

        [FunctionName("APILOG002004")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest004%", Connection = "ConectionStringRequest")] string myQueueItem, ILogger log)
        {//FUNCIONA 

            try
            {

                Logger.FileLogger("APILOG002004", "Procesando Función");

                CourierLoginResponse objResponse1 = null;
                ResponseHomologacion respuesta = null;
                string VarToken = "";

                ////////////////Login////////////////////////////////////////////////////////////////////////////
                CourierLoginShipify jsonObject = new CourierLoginShipify()
                {
                    email = vl_varEmail,
                    password = vl_valPass
                };
                               
                string jsonData1 = JsonConvert.SerializeObject(jsonObject);
                Logger.FileLogger("APILOG002004", "DATOS DE LOGIN ENVIADOS: " + jsonData1);

                jsonData1 = JsonConvert.SerializeObject(jsonObject);



                ConsumoWebService<CourierLoginResponse> cws1 = new ConsumoWebService<CourierLoginResponse>();
                objResponse1 = await cws1.PostWebService(sbUriConsumoWebService2, sbMetodoWsUriConsumowebService2, jsonData1, vl_Time, vl_Attempts, vl_varAutorizacion, vl_valAutorizacion);

                string consultado1 = JsonConvert.SerializeObject(objResponse1);

                if (consultado1 != "null")
                {
                    Logger.FileLogger("APILOG002004", "RESULTADO RECIBIDO WS LOGIN: " + jsonData1);
                    VarToken = objResponse1.data.token;
                }

                ///////////////////////////////////////////////////////////////////////////////////////////////////////



                var APILOG002004Request = JsonConvert.DeserializeObject<CanceledDeliveryShipify>(myQueueItem);
                APILOG002004Request.Enviroment = vl_Environment;

                string jsonData = JsonConvert.SerializeObject(APILOG002004Request);

                Logger.FileLogger("APILOG002004", "REQUEST RECIBIDO: " + jsonData);

                IManejadorHomologacion<ResponseHomologacion> _manejadorHomologacion = new ManejadorHomologacion<ResponseHomologacion>();
                respuesta = await _manejadorHomologacion.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriAx, APILOG002004Request.DataAreaId, vl_Time, vl_Attempts);

                jsonData = JsonConvert.SerializeObject(APILOG002004Request);

                Logger.FileLogger("APILOG002004", "REQUEST ENVIADO A WS: " + jsonData);

                ConsumoWebService<ShipifyResponse> cws = new ConsumoWebService<ShipifyResponse>();
                ShipifyResponse objResponse = await cws.PutWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData, vl_Time, vl_Attempts, VarToken, vl_varAutorizacion, vl_valAutorizacion, vl_valToken);

                if (objResponse == null)
                {

                    Logger.FileLogger("APILOG002004", "WS LEGADO: No se retorno resultado de WS");

                }
                else
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);

                    Logger.FileLogger("APILOG002004", "RESULTADO RECIBIDO WS: " + jsonrequest);

                }


            }
            catch (Exception ex)
            {
                Logger.FileLogger("APILOG002004", "ERROR: " + ex.ToString());
            }

        }
    }

}