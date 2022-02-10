using ISAC017.api.Infraestructura.Servicios;
using ISAC017.api.Infraestructure.Configuration;
using ISAC017.api.Infraestructure.Services;
using ISAC017.api.Models._001.Request;
using ISAC017.api.Models._002.Request;
using ISAC017.api.Models._004.Request;
using ISAC017.api.Models._004.Response;
using ISAC017.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.api.Controllers
{
    [ApiController]
    public class ISAC017004Controller : ControllerBase
    {

        private IManejadorRequest<APISAC017004MessageRequest> QueueRequest;
        private IManejadorResponse<APISAC017004MessageResponse> QueueResponse;
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest4").Value.ToString();
        private static string vl_QueueResponse = configuracion.GetSection("Data").GetSection("QueueResponse4").Value.ToString();
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep4").Value);
        private static int vl_Attempts = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts4").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout4").Value);
        private static int vl_TimeWS = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleepWS").Value);
        private static int vl_AttemptsWS = Convert.ToInt32(configuracion.GetSection("Data").GetSection("AttemptsWS").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        private static int longAllenar = Convert.ToInt32(configuracion.GetSection("Data").GetSection("LongAllenar").Value);

        string respuestaLog;
        const int SegAMiliS = 1000;
        const int NanoAMiliS = 10000;

        public ISAC017004Controller(IManejadorRequest<APISAC017004MessageRequest> _manejadorRequest
            , IManejadorResponse<APISAC017004MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APISAC017004")]
        [ServiceFilter(typeof(ValidationFilter004Attribute))]
        public async Task<ActionResult<APISAC017004MessageResponse>> APISAC017004(APISAC017004MessageRequest parametrorequest)
        {
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APISAC017004MessageResponse respuesta = null;
            ResponseHomologacion ResuldatoHomologa = null;
            ConvierteCodigo Convertir = new ConvierteCodigo();
            string nombre_metodo = "APISAC017004";
            APISAC017004MessageRequest APISAC017004MessageRequestW = new APISAC017004MessageRequest();
            APReturnTableDisposition004 APReturnTableDispositionList = null;
            APSalesLineReturn004 APSalesLineReturnList = null;
            APCreditNoteLine APCreditNoteLineList = null;
            APISAC017004MessageResponse APISAC017004MessageResponseW = new APISAC017004MessageResponse();
            try
            {

                

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
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
                            respuesta = new APISAC017004MessageResponse();
                            respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("ISAC017:E000|EMPRESA NO EXISTE");
                            return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                        }
                        if (ResuldatoHomologa != null && "OK".Equals(ResuldatoHomologa.DescripcionId))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            parametrorequest.DataAreaId = responseHomologa;
                            Logger.FileLogger(nombre_metodo, $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APISAC017004", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APISAC017004MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ISAC017:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                }


                /////////////////////////////////////////////////////////////////////

                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                parametrorequest.Enviroment = vl_Environment;
                APISAC017004MessageRequestW.Enviroment = parametrorequest.Enviroment;
                APISAC017004MessageRequestW.DataAreaId = parametrorequest.DataAreaId;
                APISAC017004MessageRequestW.SessionId = parametrorequest.SessionId;
                APISAC017004MessageRequestW.NumberSequenceGroup = parametrorequest.NumberSequenceGroup;
                APISAC017004MessageRequestW.ReturnTable = new APSalesTableReturn004();
                if (APISAC017004MessageRequestW.ReturnTable != null)
                {
                    APISAC017004MessageRequestW.ReturnTable.CustAccount = Convertir.CrecosAdynamics(parametrorequest.ReturnTable.CustAccount, longAllenar, nombre_metodo);
                    APISAC017004MessageRequestW.ReturnTable.InvoiceId = parametrorequest.ReturnTable.InvoiceId;
                    APISAC017004MessageRequestW.ReturnTable.ReturnDeadline = parametrorequest.ReturnTable.ReturnDeadline;
                    APISAC017004MessageRequestW.ReturnTable.ReturnReasonCodeId = parametrorequest.ReturnTable.ReturnReasonCodeId;
                    APISAC017004MessageRequestW.ReturnTable.ReturnReasonComment = parametrorequest.ReturnTable.ReturnReasonComment;
                    APISAC017004MessageRequestW.ReturnTable.APSalesLineReturnList = new List<APSalesLineReturn004>();
                    if (APISAC017004MessageRequestW.ReturnTable.APSalesLineReturnList != null) 
                    {
                        foreach (var elem in parametrorequest.ReturnTable.APSalesLineReturnList)
                            {
                                APSalesLineReturnList = new APSalesLineReturn004();
                                APSalesLineReturnList.ItemId = elem.ItemId;
                                APSalesLineReturnList.Qty = elem.Qty;
                                APSalesLineReturnList.SerialId = elem.SerialId;
                                APSalesLineReturnList.Monto = elem.Monto;
                                APISAC017004MessageRequestW.ReturnTable.APSalesLineReturnList.Add(APSalesLineReturnList);
                            }
                    }
                }
                APISAC017004MessageRequestW.ReturnTableDispositionList = new List<APReturnTableDisposition004>();
                if (APISAC017004MessageRequestW.ReturnTableDispositionList != null)
                {
                    foreach (var elem in parametrorequest.ReturnTableDispositionList)
                    {
                        APReturnTableDispositionList = new APReturnTableDisposition004();
                        APReturnTableDispositionList.ReturnItemNum = elem.ReturnItemNum;
                        APReturnTableDispositionList.ItemId = elem.ItemId;
                        APReturnTableDispositionList.Qty = elem.Qty;
                        APReturnTableDispositionList.DispositionCodeId = elem.DispositionCodeId;
                        APReturnTableDispositionList.InventLocationId = elem.InventLocationId;
                        APReturnTableDispositionList.WMSLocationId = elem.WMSLocationId;
                        APISAC017004MessageRequestW.ReturnTableDispositionList.Add(APReturnTableDispositionList);
                    }
                }

                string jsonrequest2 = JsonConvert.SerializeObject(APISAC017004MessageRequestW);
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Cambiado Enviado a Dynamics: " + jsonrequest2);
                jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Origen antes de Enviar a Dynamics: " + jsonrequest2);
                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, APISAC017004MessageRequestW);
                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time, vl_Attempts, vl_Timeout, nombre_metodo);
                

                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(APISAC017004MessageRequestW);
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APISAC017004MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ISAc017:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);
                }
                else
                {
                    
                    APISAC017004MessageResponseW.SessionId = respuesta.SessionId;
                    APISAC017004MessageResponseW.StatusId = respuesta.StatusId;
                    APISAC017004MessageResponseW.ErrorList = respuesta.ErrorList;
                    APISAC017004MessageResponseW.CreditNote = new APCreditNoteHeader();

                    if (respuesta.CreditNote != null)
                    {
                        APISAC017004MessageResponseW.CreditNote.CustAccount = Convertir.DynamicAcrecos(respuesta.CreditNote.CustAccount, nombre_metodo);
                        APISAC017004MessageResponseW.CreditNote.VatNum = respuesta.CreditNote.VatNum;
                        APISAC017004MessageResponseW.CreditNote.CreditNoteNum = respuesta.CreditNote.CreditNoteNum;
                        APISAC017004MessageResponseW.CreditNote.CreditNoteDate = respuesta.CreditNote.CreditNoteDate;
                        APISAC017004MessageResponseW.CreditNote.ReturnNum = respuesta.CreditNote.ReturnNum;
                        APISAC017004MessageResponseW.CreditNote.InvoiceNum = respuesta.CreditNote.InvoiceNum;
                        APISAC017004MessageResponseW.CreditNote.ReasonRefund = respuesta.CreditNote.ReasonRefund;
                        APISAC017004MessageResponseW.CreditNote.Voucher = respuesta.CreditNote.Voucher;
                        APISAC017004MessageResponseW.CreditNote.CreditNoteAmount = respuesta.CreditNote.CreditNoteAmount;
                        APISAC017004MessageResponseW.CreditNote.ItemReturnList = new List<APCreditNoteLine>();

                        //}

                        if (respuesta.CreditNote.ItemReturnList != null)
                       // if (respuesta.CreditNote != null && respuesta.CreditNote.ItemReturnList != null)
                        {
                            foreach (var elem in respuesta.CreditNote.ItemReturnList)
                            {
                                APCreditNoteLineList = new APCreditNoteLine();
                                APCreditNoteLineList.ItemId = elem.ItemId;
                                APCreditNoteLineList.Qty = elem.Qty;
                                APCreditNoteLineList.DispositionCode = elem.DispositionCode;
                                APISAC017004MessageResponseW.CreditNote.ItemReturnList.Add(APCreditNoteLineList);
                            }
                        }
                    }
                       
                    respuestaLog = JsonConvert.SerializeObject(APISAC017004MessageRequestW);
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(APISAC017004MessageResponseW);
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: Response desde Dynamics: " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: Response desde Dynamics Origen: " + respuestaLog);

                    return Ok(APISAC017004MessageResponseW);
                }
            }
            catch (Exception ex)
            {
                respuestaLog = JsonConvert.SerializeObject(APISAC017004MessageRequestW);
                respuesta = new APISAC017004MessageResponse();
                respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ISAC017:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }
    }
}
