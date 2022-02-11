using Interface.Api.Infraestructura.Configuracion;
using IVTA006.Infraestructure.Configuration;
using IVTA006.Infraestructure.Services;
using IVTA006.Models._001.Request;
using IVTA006.Models._001.Response;
using IVTA006.Models.Homologacion.ResponseHomologacion;
using IVTA006.Models.Response;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using static IVTA006.Services.ManejadorRequest;

namespace IVTA006.Function
{
    public static class IVTA006001
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
        private static int vl_Time1 = Convert.ToInt32(Environment.GetEnvironmentVariable("TimeSleep1"));
        private static int vl_Attempts1 = Convert.ToInt32(Environment.GetEnvironmentVariable("Attempts1"));
        private static string vl_Environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

        [FunctionName("APIVTA006001")]
        public static async Task Run([ServiceBusTrigger("%QueueRequest%", Connection = "ConectionStringRequest")] string myQueueItem, ILogger log)
        {//FUNCIONA 

            try
            {

                Logger.FileLogger("APIVTA006001", "Procesando Función");

                ResponseHomologacion respuesta = null;
                APIVTA006001MessageResponse objResponse = null;
                var APIVTA006001Request = JsonConvert.DeserializeObject<APIVTA006001MessageRequest>(myQueueItem);
                APIVTA006001Request.Enviroment = vl_Environment;

                string jsonData = JsonConvert.SerializeObject(APIVTA006001Request);
                Logger.FileLogger("APIVTA006001", "REQUEST RECIBIDO DE DYNAMICS: " + jsonData);

                ////////////////////HOMOLOGACION/////////////////////////////////////////////////////////////////////
                //string DataAreaId = APIVTA006001Request.DataAreaId;
               
                int cantidadInvoice = APIVTA006001Request.APInvoiceIVTA006001.Count;
                int i = 0;

                if (cantidadInvoice > 0)
                {
                    do
                    {
                        int cant = APIVTA006001Request.APInvoiceIVTA006001[i].APStoreId.Length;

                        if (cant > 0) {

                            IManejadorHomologacion<ResponseHomologacion> _manejadorHomologacion = new ManejadorHomologacion<ResponseHomologacion>();
                            respuesta = await _manejadorHomologacion.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriAx, APIVTA006001Request.APInvoiceIVTA006001[i].APStoreId, vl_Time, vl_Attempts);

                            if (respuesta == null)
                            {
                                objResponse = new APIVTA006001MessageResponse();
                                respuesta.ErrorList = new List<string>();
                                respuesta.ErrorList.Add("IVTA006:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                                Logger.FileLogger("APIVTA006001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");
                            }
                            else
                            {
                                if (respuesta.Response != null)
                                {
                                    APIVTA006001Request.APInvoiceIVTA006001[i].APStoreId = respuesta.Response;
                                }

                            }
                        }
                        i++;
                    } while (i < cantidadInvoice);
                }
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////

                jsonData = JsonConvert.SerializeObject(APIVTA006001Request);

                ConsumoWebService<APIVTA006001MessageResponse> cws = new ConsumoWebService<APIVTA006001MessageResponse>();
                objResponse = await cws.PostWebService(sbUriConsumoWebService, sbMetodoWsUriConsumowebService, jsonData, vl_Time1, vl_Attempts1);//concluido

                string consultado = JsonConvert.SerializeObject(objResponse);
               
                if (consultado == "null")
                {
                    Logger.FileLogger("APIVTA006001", "WS LEGADO: No se retorno resultado de WS");
                }
                else
                {
                    string jsonrequest = JsonConvert.SerializeObject(objResponse);

                    Logger.FileLogger("APIVTA006001", "RESULTADO RECIBIDO WS: " + jsonrequest);

                    IManejadorRequest<APIVTA006001MessageResponse> _manejadorRequestQueue = new ManejadorRequest<APIVTA006001MessageResponse>();

                    await _manejadorRequestQueue.EnviarMensajeAsync(objResponse.SessionId, sbConectionStringSend, sbQueueResponse, objResponse);
                    string mensaje = JsonConvert.SerializeObject(objResponse);
                    Logger.FileLogger("APIVTA006001", "REQUEST ENVIADO A DYNAMICS: " + mensaje);

                }

               
            }
            catch (Exception ex)
            {
                Logger.FileLogger("APIVTA006001", "ERROR: " + ex.ToString());
            }

        }
    }
}
