using ICAJ012.api.Infraestructure.Configuration;
using ICAJ012.api.Infraestructure.Services;
using ICAJ012.api.Models._001.Request;
using ICAJ012.api.Models._001.Response;
using ICAJ012.api.Models.Homologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ012.api.Controllers
{
    
    [ApiController]
    public class ICAJ012001Controller : ControllerBase
    {
        private IManejadorRequest<APICAJ012001MessageRequest> QueueRequest;
        private IManejadorResponse<APICAJ012001MessageResponse> QueueResponse;
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest").Value.ToString();
        private static string vl_QueueResponse = configuracion.GetSection("Data").GetSection("QueueResponse").Value.ToString();
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep").Value);
        private static int vl_Attempts = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout").Value);
        private static int vl_TimeWS = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleepWS").Value);
        private static int vl_AttemptsWS = Convert.ToInt32(configuracion.GetSection("Data").GetSection("AttemptsWS").Value);

        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32((configuracion.GetSection("Data").GetSection("LongAllenar").Value));

        string respuestaLog;
        const int SegAMiliS = 1000;
        const int NanoAMiliS = 10000;

        public ICAJ012001Controller(IManejadorRequest<APICAJ012001MessageRequest> _manejadorRequest
            , IManejadorResponse<APICAJ012001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APICAJ012001")]
        [ServiceFilter(typeof(ValidadorRequest001))]
        public async Task<ActionResult<APICAJ012001MessageResponse>> APICAJ012001(APICAJ012001MessageRequest parametrorequest)
        {
            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger("APICAJ012001", "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }


            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APICAJ012001MessageResponse respuesta = null;
            ResponseHomologacion ResuldatoHomologa = null;
            APICAJ012001MessageRequest APICAJ012001RequestW = null;
            APLedgerJournalTransICAJ012 APLedgerJournalTransICAJ012List = null;
            APFinancialDimension APFinancialDimensionList = null;
            APICAJ012001MessageResponse APICAJ012001MessageResponseW = null;
            APLedgerJournalTransICAJ012_Response APLedgerJournalTransICAJ012_ResponseList = null;
            APFinancialDimension_Response APFinancialDimension_ResponseList = null;
            try
            {

                string nombre_metodo = "APICAJ012001";

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ012001", "CONTROLADOR: Request Recibido: " + jsonrequest);
                string DataAreaId = parametrorequest.DataAreaId;
                ///HOMOLOGACIÓN////////////////////////////////////////////////
                if (string.IsNullOrEmpty(DataAreaId) || string.IsNullOrWhiteSpace(DataAreaId))
                    throw new Exception("CONTROLADOR WS HOMOLOGACION: El código de empresa no debe ser nulo.");

                start = Stopwatch.GetTimestamp();
                int cont = 0;
                for (int i = 1; i < vl_AttemptsWS; i++)
                {
                    try
                    {

                        ResuldatoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId);

                        // Validacion el objeto recibido
                        // Condicion para salir
                        if (ResuldatoHomologa != null && (string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                        {
                            respuesta = new APICAJ012001MessageResponse();
                            //  respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("ICAJ012:E000|EMPRESA NO EXISTE");
                            return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                        }
                        if (ResuldatoHomologa != null && "OK".Equals(ResuldatoHomologa.DescripcionId))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            parametrorequest.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICAJ012001", $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
                            cont = i;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    await Task.Delay(vl_TimeWS);
                } // fin for
                end = Stopwatch.GetTimestamp();
                diff = start > 0 ? (end - start) / NanoAMiliS : 0;
                Logger.FileLogger("APICAJ012001", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICAJ012001MessageResponse();
                    // respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ012:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                }


                /////////////////////////////////////////////////////////////////////

                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                parametrorequest.Enviroment = vl_Environment;
                // convertir
                APICAJ012001RequestW = new APICAJ012001MessageRequest();
                APICAJ012001RequestW.DataAreaId = parametrorequest.DataAreaId;
                APICAJ012001RequestW.Enviroment = parametrorequest.Enviroment;
                APICAJ012001RequestW.SessionId = parametrorequest.SessionId;
                APICAJ012001RequestW.APReasonIngressEgressId = parametrorequest.APReasonIngressEgressId;
                APICAJ012001RequestW.APLedgerJournalTransList = new List<APLedgerJournalTransICAJ012>();
                if (parametrorequest.APLedgerJournalTransList != null)
                { 
                foreach (var elem in parametrorequest.APLedgerJournalTransList) 
                {
                    APLedgerJournalTransICAJ012List = new APLedgerJournalTransICAJ012();
                    APLedgerJournalTransICAJ012List.TransDate = elem.TransDate;
                    APLedgerJournalTransICAJ012List.Txt = elem.Txt;
                    APLedgerJournalTransICAJ012List.AccountType = elem.AccountType;
                        if (Convert.ToInt32(elem.AccountType) == 1) //|| Convert.ToInt32(elem.AccountType) == 6) // Pidio Diego en pruebas
                        {
                           // Logger.FileLogger("APICAJ012001", " verdad a elem.AccountType: " + elem.AccountType);
                            APLedgerJournalTransICAJ012List.Account = CodigoCliente.DynamicAcrecos(elem.Account, nombre_metodo);

                        }
                        else
                        {
                          //  Logger.FileLogger("APICAJ012001", " falso a elem.AccountType: " + elem.AccountType);
                            APLedgerJournalTransICAJ012List.Account = elem.Account;
                        }



                     //   APLedgerJournalTransICAJ012List.Account = CodigoCliente.CrecosAdynamics(elem.Account,longAllenar,nombre_metodo);
                    APLedgerJournalTransICAJ012List.OffsetAccountType = elem.OffsetAccountType;
                    APLedgerJournalTransICAJ012List.OffSetAccount = elem.OffSetAccount;
                    APLedgerJournalTransICAJ012List.Debit = elem.Debit;
                    APLedgerJournalTransICAJ012List.Credit = elem.Credit;
                    APLedgerJournalTransICAJ012List.DocumentNum = elem.DocumentNum;
                    APLedgerJournalTransICAJ012List.ApStoreId = elem.ApStoreId;
                    APLedgerJournalTransICAJ012List.APUserGeneMovi = elem.APUserGeneMovi;
                    APLedgerJournalTransICAJ012List.ApTransactionType = elem.ApTransactionType;
                    APLedgerJournalTransICAJ012List.ApBoxCode = elem.ApBoxCode;
                    APLedgerJournalTransICAJ012List.Voucher = elem.Voucher;
                    APLedgerJournalTransICAJ012List.APFinancialDimensionList = new List<APFinancialDimension>();
                        if (elem.APFinancialDimensionList != null)
                        {
                            foreach (var elem1 in elem.APFinancialDimensionList)
                            {
                                APFinancialDimensionList = new APFinancialDimension();
                                APFinancialDimensionList.Name = elem1.Name;
                                APFinancialDimensionList.Valor = elem1.Valor;
                                APLedgerJournalTransICAJ012List.APFinancialDimensionList.Add(APFinancialDimensionList);
                            }
                       
                    APICAJ012001RequestW.APLedgerJournalTransList.Add(APLedgerJournalTransICAJ012List);
                        }
                    }
                }

                // convertir
                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ012001", "CONTROLADOR: Request Enviado a Dynamics, antes conversion: " + jsonrequest2);
                jsonrequest2 = JsonConvert.SerializeObject(APICAJ012001RequestW);
                Logger.FileLogger("APICAJ012001", "CONTROLADOR: Request Enviado a Dynamics, despues de conversion: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, APICAJ012001RequestW);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time, vl_Attempts, vl_Timeout, nombre_metodo);
                // convertir
                Logger.FileLogger("APICAJ012001", "respuesta null");
                if (respuesta != null) 
                {
                        APICAJ012001MessageResponseW = new APICAJ012001MessageResponse();
                        APICAJ012001MessageResponseW.SessionId = respuesta.SessionId;
                        APICAJ012001MessageResponseW.StatusId = respuesta.StatusId;
                        APICAJ012001MessageResponseW.ErrorList = new List<string>();
                        if (respuesta.ErrorList != null)
                            if (respuesta.ErrorList.Count > 0) 
                            {
                                foreach (var elem in respuesta.ErrorList) 
                                {
                                    APICAJ012001MessageResponseW.ErrorList.Add(elem);
                                }
                    
                            }
                        APICAJ012001MessageResponseW.APReasonIngressEgressId = respuesta.APReasonIngressEgressId;
                        APICAJ012001MessageResponseW.APLedgerJournalTransList = new List<APLedgerJournalTransICAJ012_Response>();
                    Logger.FileLogger("APICAJ012001", "respuesta cero");
                    if (respuesta.APLedgerJournalTransList != null)
                    {
                        foreach (var elem in respuesta.APLedgerJournalTransList)
                        {
                            APLedgerJournalTransICAJ012_ResponseList = new APLedgerJournalTransICAJ012_Response();
                            APLedgerJournalTransICAJ012_ResponseList.TransDate = elem.TransDate;
                            APLedgerJournalTransICAJ012_ResponseList.Txt = elem.Txt;
                            APLedgerJournalTransICAJ012_ResponseList.AccountType = elem.AccountType;
                            if (Convert.ToInt32(elem.AccountType) == 1 ) //|| Convert.ToInt32(elem.AccountType) == 6) // Pidio Diego en pruebas
                            {
                               // Logger.FileLogger("APICAJ012001", " verdad elem.AccountType: " + elem.AccountType);
                                APLedgerJournalTransICAJ012_ResponseList.Account = CodigoCliente.DynamicAcrecos(elem.Account, nombre_metodo);

                            }
                            else
                            {
                               // Logger.FileLogger("APICAJ012001", " falso elem.AccountType: " + elem.AccountType);
                                APLedgerJournalTransICAJ012_ResponseList.Account = elem.Account;
                            }
                            
                            APLedgerJournalTransICAJ012_ResponseList.OffsetAccountType = elem.OffsetAccountType;
                            APLedgerJournalTransICAJ012_ResponseList.OffSetAccount = elem.OffSetAccount;
                            APLedgerJournalTransICAJ012_ResponseList.Debit = elem.Debit;
                            APLedgerJournalTransICAJ012_ResponseList.Credit = elem.Credit;
                            APLedgerJournalTransICAJ012_ResponseList.DocumentNum = elem.DocumentNum;
                            APLedgerJournalTransICAJ012_ResponseList.ApStoreId = elem.ApStoreId;
                            APLedgerJournalTransICAJ012_ResponseList.APUserGeneMovi = elem.APUserGeneMovi;
                            APLedgerJournalTransICAJ012_ResponseList.ApTransactionType = elem.ApTransactionType;
                            APLedgerJournalTransICAJ012_ResponseList.ApBoxCode = elem.ApBoxCode;
                            APLedgerJournalTransICAJ012_ResponseList.Voucher = elem.Voucher;
                            APLedgerJournalTransICAJ012_ResponseList.APFinancialDimensionList = new List<APFinancialDimension_Response>();
                            if (elem.APFinancialDimensionList != null)
                            {
                                foreach (var elem1 in elem.APFinancialDimensionList)
                                {
                                    APFinancialDimension_ResponseList = new APFinancialDimension_Response();
                                    APFinancialDimension_ResponseList.Name = elem1.Name;
                                    APFinancialDimension_ResponseList.Valor = elem1.Valor;
                                    APLedgerJournalTransICAJ012_ResponseList.APFinancialDimensionList.Add(APFinancialDimension_ResponseList);
                                }
                               // APICAJ012001MessageResponseW.APLedgerJournalTransList.Add(APLedgerJournalTransICAJ012_ResponseList);
                            }
                            APICAJ012001MessageResponseW.APLedgerJournalTransList.Add(APLedgerJournalTransICAJ012_ResponseList);
                        }
                        
                    }
                }
                //


                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(APICAJ012001MessageResponseW);
                    Logger.FileLogger("APICAJ012001", "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger("APICAJ012001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICAJ012001MessageResponse();
                    //  respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ012:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(APICAJ012001MessageResponseW);
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(APICAJ012001RequestW);
                    Logger.FileLogger("APICAJ012001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(APICAJ012001MessageResponseW);
                    Logger.FileLogger("APICAJ012001", "CONTROLADOR: Response desde Dynamics: " + respuestaLog);

                    return Ok(APICAJ012001MessageResponseW);
                }
            }
            catch (Exception ex)
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                respuesta = new APICAJ012001MessageResponse();
                //  respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICAJ012:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger("APICAJ012001", "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger("APICAJ012001", "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }
    }
}
