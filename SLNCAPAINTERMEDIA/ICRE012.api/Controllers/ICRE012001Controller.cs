
using ICRE012.api.Infraestructura.Servicios;
using ICRE012.api.Infraestructure.Configuration;
using ICRE012.api.Infraestructure.Services;
using ICRE012.api.Models._001.Request;
using ICRE012.api.Models._001.Response;
using ICRE012.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE012.api.Controllers
{
    
    [ApiController]
    public class ICRE012001Controller : ControllerBase
    {
        private IManejadorRequest<APICRE012001MessageRequest> QueueRequest;
        private IManejadorResponse<APICRE012001MessageResponse> QueueResponse;
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
        private static int vl_Time2 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep2").Value);
        private static int vl_Attempts2 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts2").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32((configuracion.GetSection("Data").GetSection("LongAllenar").Value));

        string respuestaLog;
        const int SegAMiliS = 1000;
        const int NanoAMiliS = 10000;
        public ICRE012001Controller(IManejadorRequest<APICRE012001MessageRequest> _manejadorRequest
            , IManejadorResponse<APICRE012001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APICRE012001")]
        [ServiceFilter(typeof(ValidadorRequest))]
        public async Task<ActionResult<APICRE012001MessageResponse>> APICRE012001(APICRE012001MessageRequest parametrorequest)
        {
            
            string nombre_metodo = "APICRE012001";
            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger("APICRE012001", "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APICRE012001MessageResponse respuesta = null;
            ResponseHomologacion ResuldatoHomologa = null;
            APICRE012001MessageRequest APICRE012001MessageRequestW = new APICRE012001MessageRequest();
            APICRE012001MessageResponse APICRE012001MessageResponseW = null;
            APInvoice APInvoiceW = null;
            APJournalTable APJournalTableW = null;
            APJournalTrans APJournalTransW = null;
            APSalesTable APSalesTableW = null;
            APSalesLine APSalesLineW = null;
            APCreditNoteServicesList APCreditNoteServicesListW = null;

            try
            {

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICRE012001", "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId);

                if (string.IsNullOrEmpty(DataAreaId) || string.IsNullOrWhiteSpace(DataAreaId))
                    throw new Exception("CONTROLADOR WS HOMOLOGACION: El código de empresa no debe ser nulo.");

                start = Stopwatch.GetTimestamp();
                int cont = 0;
                for (int i = 1; i < vl_Attempts2; i++)
                {
                    try
                    {

                        ResuldatoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId);

                        // Validacion el objeto recibido
                        // Condicion para salir
                        if (ResuldatoHomologa != null && (string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                        {
                            respuesta = new APICRE012001MessageResponse();
                            respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("ICRE012:E000|EMPRESA NO EXISTE");
                            return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                        }
                        if (ResuldatoHomologa != null && "OK".Equals(ResuldatoHomologa.DescripcionId))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            parametrorequest.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICRE012001", $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
                            cont = i;
                            break;
                        }
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    await Task.Delay(vl_Time2);
                } // fin for
                end = Stopwatch.GetTimestamp();
                diff = start > 0 ? (end - start) / NanoAMiliS : 0;
                Logger.FileLogger("APICRE012001", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICRE012001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICRE012:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                }
                /////////////////////////////////////////////////////////////////////

                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                parametrorequest.Enviroment = vl_Environment;
                // convertir crecos a dynamic
                APICRE012001MessageRequestW.DataAreaId = parametrorequest.DataAreaId;
                APICRE012001MessageRequestW.Enviroment = parametrorequest.Enviroment;
                APICRE012001MessageRequestW.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.CustAccount ,longAllenar, "APICRE012001");
                APICRE012001MessageRequestW.DocumenType = parametrorequest.DocumenType;
                APICRE012001MessageRequestW.OrdenTrabajo = parametrorequest.OrdenTrabajo;
                APICRE012001MessageRequestW.InvoiceIdNC = parametrorequest.InvoiceIdNC;
                APICRE012001MessageRequestW.Vourcher = parametrorequest.Vourcher;
                APICRE012001MessageRequestW.APStoreId = parametrorequest.APStoreId;
                APICRE012001MessageRequestW.BusinnesUnit = parametrorequest.BusinnesUnit;
                APICRE012001MessageRequestW.APIdentificationList = parametrorequest.APIdentificationList;
                APICRE012001MessageRequestW.TransactionType = parametrorequest.TransactionType;
                APICRE012001MessageRequestW.PaymMode = parametrorequest.PaymMode;
                APICRE012001MessageRequestW.CajaCode = parametrorequest.CajaCode;
                APICRE012001MessageRequestW.DateStart = parametrorequest.DateStart;
                APICRE012001MessageRequestW.DateEnd = parametrorequest.DateEnd;
                APICRE012001MessageRequestW.ItemId = parametrorequest.ItemId;
                APICRE012001MessageRequestW.DocumentNum = parametrorequest.DocumentNum;
                APICRE012001MessageRequestW.SessionId = parametrorequest.SessionId;
                              

                //
                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICRE012001", "CONTROLADOR: Request Enviado a Dynamics, antes convertir: " + jsonrequest2);
                 jsonrequest2 = JsonConvert.SerializeObject(APICRE012001MessageRequestW);
                Logger.FileLogger("APICRE012001", "CONTROLADOR: Request Enviado a Dynamics, despues de convertir: " + jsonrequest2);
                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, APICRE012001MessageRequestW);
                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time, vl_Attempts, vl_Timeout, nombre_metodo);
                //convertir dynamic a crecos
                APICRE012001MessageResponseW = new APICRE012001MessageResponse();
                APICRE012001MessageResponseW.SessionId = respuesta.SessionId;
                
                if (respuesta.APInvoice != null) //

                {
                    APICRE012001MessageResponseW.APInvoice = new List<APInvoice>();
                    foreach(var elem in respuesta.APInvoice)
                    {
                        APInvoiceW =  new APInvoice();
                       
                        APInvoiceW.VatNum = elem.VatNum;
                        APInvoiceW.Name = elem.Name;
                        APInvoiceW.Email = elem.Email;
                        APInvoiceW.Vourcher = elem.Vourcher;
                        APInvoiceW.phoneNumber = elem.phoneNumber;
                        APInvoiceW.dataAreaId = elem.dataAreaId;
                        APInvoiceW.InvoiceDate = elem.InvoiceDate;
                        APInvoiceW.InvoiceId = elem.InvoiceId;
                        APInvoiceW.Establishment = elem.Establishment;
                        APInvoiceW.EmissionPoint = elem.EmissionPoint;
                        APInvoiceW.DateAuthSRI = elem.DateAuthSRI;
                        APInvoiceW.AuthorizationNumber = elem.AuthorizationNumber;
                        APInvoiceW.DocumentApplied = elem.DocumentApplied;
                        APInvoiceW.EstablishmentApplied = elem.EstablishmentApplied;
                        APInvoiceW.EmissionPointApplied = elem.EmissionPointApplied;
                        APInvoiceW.APIdentificationList = elem.APIdentificationList;
                        APInvoiceW.DocumentAmountBalance = elem.DocumentAmountBalance;
                      
                        if (elem.APSalesTable != null)
                        {
                            APInvoiceW.APSalesTable = new APSalesTable();
                    
                            if (elem.APSalesTable.CustAccount != null)
                                APInvoiceW.APSalesTable.CustAccount = CodigoCliente.DynamicAcrecos(elem.APSalesTable.CustAccount, "APICRE012001");
                            APInvoiceW.APSalesTable.SalesId = elem.APSalesTable.SalesId;
                            APInvoiceW.APSalesTable.NumberOrdenRef = elem.APSalesTable.NumberOrdenRef;
                            APInvoiceW.APSalesTable.Subtotal = elem.APSalesTable.Subtotal;
                            APInvoiceW.APSalesTable.TotalIVA = elem.APSalesTable.TotalIVA;
                            APInvoiceW.APSalesTable.CustGroup = elem.APSalesTable.CustGroup;
                            APInvoiceW.APSalesTable.AmountInvoice = elem.APSalesTable.AmountInvoice;
                            APInvoiceW.APSalesTable.APStoreId = elem.APSalesTable.APStoreId;
                            APInvoiceW.APSalesTable.APStoreName = elem.APSalesTable.APStoreName;
                            APInvoiceW.APSalesTable.IndependentEntrepreneurId = elem.APSalesTable.IndependentEntrepreneurId;
                            APInvoiceW.APSalesTable.ChannelDimFinOrigin = elem.APSalesTable.ChannelDimFinOrigin;
                            APInvoiceW.APSalesTable.MotiveReturnDescription = elem.APSalesTable.MotiveReturnDescription;
                            APInvoiceW.APSalesTable.MotiveReturnId = elem.APSalesTable.MotiveReturnId;
                            APInvoiceW.APSalesTable.SalesReturn = elem.APSalesTable.SalesReturn;
                            APInvoiceW.APSalesTable.BusinessUnitDimFinOrigin = elem.APSalesTable.BusinessUnitDimFinOrigin;
                            APInvoiceW.APSalesTable.ReasonIdNC = elem.APSalesTable.ReasonIdNC;
                            APInvoiceW.APSalesTable.ReasonDescriptionNC = elem.APSalesTable.ReasonDescriptionNC;
                            APInvoiceW.APSalesTable.TotalDiscount = elem.APSalesTable.TotalDiscount;
                            APInvoiceW.APSalesTable.WorkSalesResponsible = elem.APSalesTable.WorkSalesResponsible;
                            APInvoiceW.APSalesTable.WorkSalesResponsibleName = elem.APSalesTable.WorkSalesResponsibleName;


                            if (elem.APSalesTable != null && elem.APSalesTable.APSalesLineList != null)
                            {
                                APInvoiceW.APSalesTable.APSalesLineList = new List<APSalesLine>();
                                foreach (var elem1 in elem.APSalesTable.APSalesLineList)
                                {
                                    APSalesLineW = new APSalesLine();
                                    APSalesLineW.ItemId = elem1.ItemId;
                                    APSalesLineW.APLineId = elem1.APLineId;
                                    APSalesLineW.APGroupId = elem1.APGroupId;
                                    APSalesLineW.APSubGroupId = elem1.APSubGroupId;
                                    APSalesLineW.APCategoryId = elem1.APCategoryId;
                                    APSalesLineW.ItemName = elem1.ItemName;
                                    APSalesLineW.ItemType = elem1.ItemType;
                                    APSalesLineW.InventLocationId = elem1.InventLocationId;
                                    APSalesLineW.Qty = elem1.Qty;
                                    APSalesLineW.SalesPrice = elem1.SalesPrice;
                                    APSalesLineW.LineAmount = elem1.LineAmount;
                                    APSalesLineW.Tax = elem1.Tax;
                                    APSalesLineW.SubTotalLinea = elem1.SubTotalLinea;
                                    APSalesLineW.LineDisc = elem1.LineDisc;
                                    APSalesLineW.Serie = elem1.Serie;
                                    APSalesLineW.Marca = elem1.Marca;
                                    APSalesLineW.Color = elem1.Color;
                                    APSalesLineW.WMSLocationId = elem1.WMSLocationId;

                                    APInvoiceW.APSalesTable.APSalesLineList.Add(APSalesLineW);

                                }
                            }
                        } // APSalesTable

                        
                        if (elem.APCreditNoteServices != null)
                        {
                            APInvoiceW.APCreditNoteServices = new List<APCreditNoteServicesList>();
                            foreach (var elem6 in elem.APCreditNoteServices) 
                            {
                                APCreditNoteServicesListW = new APCreditNoteServicesList();
                                APCreditNoteServicesListW.Description = elem6.Description;
                                APCreditNoteServicesListW.Qty = elem6.Qty;
                                APCreditNoteServicesListW.Subtotal = elem6.Subtotal;
                                APCreditNoteServicesListW.Tax = elem6.Tax;
                                APCreditNoteServicesListW.Total = elem6.Total;
                                APCreditNoteServicesListW.UnitPriece = elem6.UnitPriece;
                                APInvoiceW.APCreditNoteServices.Add(APCreditNoteServicesListW);
                            }
                        

                        }
                        APICRE012001MessageResponseW.APInvoice.Add(APInvoiceW);
                    }

                } //invoice

                if (respuesta.APJournalTable != null)
                {
                    
                    APICRE012001MessageResponseW.APJournalTable = new List<APJournalTable>();
                    foreach (var elem3 in respuesta.APJournalTable)
                    {
                        APJournalTableW = new APJournalTable();
                        APJournalTableW.JournalNum = elem3.JournalNum;
                        APJournalTableW.Name = elem3.Name;
                        if (elem3.APJournalTransList != null)
                        {
                            APJournalTableW.APJournalTransList = new List<APJournalTrans>();
                            foreach (var elem1 in elem3.APJournalTransList)
                            {
                                APJournalTransW = new APJournalTrans();
                                APJournalTransW.TransDate = elem1.TransDate;
                                APJournalTransW.Voucher = elem1.Voucher;
                              //  APJournalTransW.dataAreaId = elem1.dataAreaId;
                                APJournalTransW.CustAccount = CodigoCliente.DynamicAcrecos(elem1.CustAccount, "APICRE012001");
                                APJournalTransW.CustName = elem1.CustName;
                                APJournalTransW.CustGroup = elem1.CustGroup;
                                APJournalTransW.Txt = elem1.Txt;
                                APJournalTransW.InvoiceIdList = new List<string>();
                                if (elem1.InvoiceIdList != null)
                                {
                                    foreach (var elem2 in elem1.InvoiceIdList)
                                    {
                                        APJournalTransW.InvoiceIdList.Add(elem2);
                                    }
                                }

                                APJournalTransW.AmountDebit = elem1.AmountDebit;
                                APJournalTransW.AmountCredit = elem1.AmountCredit;
                                APJournalTransW.PaymMode = elem1.PaymMode;
                                APJournalTransW.PostingProfile = elem1.PostingProfile;
                                APJournalTransW.APStoreId = elem1.APStoreId;
                                APJournalTransW.APStoreName = elem1.APStoreName;
                                APJournalTransW.UserCreation = elem1.UserCreation;
                                APJournalTransW.TransactionType = elem1.TransactionType;
                                APJournalTransW.SalesId = elem1.SalesId;
                                APJournalTransW.NumberOT = elem1.NumberOT;
                                APJournalTransW.CajaCode = elem1.CajaCode;
                                APJournalTransW.APIdentificationList = elem1.APIdentificationList;
                                APJournalTableW.APJournalTransList.Add(APJournalTransW);
                            }
                        }
                        APICRE012001MessageResponseW.APJournalTable.Add(APJournalTableW);
                    }
                }
                
                APICRE012001MessageResponseW.StatusId = respuesta.StatusId;
                APICRE012001MessageResponseW.ErrorList = new List<string>();
                if (respuesta.ErrorList != null) 
                {
                    foreach (var elem4 in respuesta.ErrorList)
                    {
                        APICRE012001MessageResponseW.ErrorList.Add(elem4);
                    }
                }
                
                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                    Logger.FileLogger("APICRE012001", "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger("APICRE012001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICRE012001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICRE012:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);  //StatusCode(StatusCodes.Status204NoContent, "No se retorno resultado de dynamics");
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                    Logger.FileLogger("APICRE012001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger("APICRE012001", "CONTROLADOR: Response desde Dynamics: " + respuestaLog);

                    return Ok(APICRE012001MessageResponseW);
                }
            }
            catch (Exception ex)
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                respuesta = new APICRE012001MessageResponse();
                respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICRE012:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger("APICRE012001", "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger("APICRE012001", "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);


            }

        }
    }
}
