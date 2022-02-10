using IVTA004.api.Infraestructura.Servicios;
using IVTA004.api.Infraestructure.Configuration;
using IVTA004.api.Infraestructure.Services;
using IVTA004.api.Models._001.Request;
using IVTA004.api.Models._001.Response;
using IVTA004.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA004.api.Controllers
{
    
    [ApiController]
    public class IVTA004001Controller : ControllerBase
    {
        private IManejadorRequest<APISAC020001MessageRequest> QueueRequest;
        private IManejadorResponse<APIVTA004001MessageResponse> QueueResponse;
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
        private static int longAllenar = Convert.ToInt32(configuracion.GetSection("Data").GetSection("LongAllenar").Value); //Convert.ToInt32(Environment.GetEnvironmentVariable("LongAllenar"));
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        string respuestaLog;
        const int SegAMiliS = 1000;
        const int NanoAMiliS = 10000;
        string nombre_metodo = "APIVTA004001";

        public IVTA004001Controller(IManejadorRequest<APISAC020001MessageRequest> _manejadorRequest
            , IManejadorResponse<APIVTA004001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }
        [HttpPost]
        [Route("APIVTA004001")]
        [ServiceFilter(typeof(ValidadorRequest))]
        public async Task<ActionResult<APIVTA004001MessageResponse>> APIVTA004001(APISAC020001MessageRequest parametrorequest)
        {
            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger("APICRE007001", "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }

            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            ResponseHomologacion ResuldatoHomologa = null;
            APIVTA004001MessageResponse respuesta = null;
            APISAC020001MessageRequest APISAC020001MessageRequestW = new APISAC020001MessageRequest();
            APSalesTableBillBuyerNC APSalesTableBillBuyerNCList = null;
            APBuyerNumberTickectList APBuyerNumberTickectListList = null;
            APSalesLineIVTA004001 APSalesLineIVTA004001List = null;
            APFinancialDimension APFinancialDimensionList = null;


            try
            {

                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APIVTA004001", "CONTROLADOR: Request Recibido: " + jsonrequest);

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
                            respuesta = new APIVTA004001MessageResponse();
                            respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("IVTA004:E000|EMPRESA NO EXISTE");
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
                Logger.FileLogger(nombre_metodo, $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APIVTA004001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("IVTA004:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                }
                /////////////////////////////////////////////////////////////////////

                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                parametrorequest.Enviroment = vl_Environment;
                APISAC020001MessageRequestW.DataAreaId = parametrorequest.DataAreaId;
                APISAC020001MessageRequestW.SessionId = parametrorequest.SessionId;
                APISAC020001MessageRequestW.Enviroment = parametrorequest.Enviroment;
                if (parametrorequest.APSalesTable !=null)
                {
                    APISAC020001MessageRequestW.APSalesTable = new APSalesTableIVTA004001();
                    APISAC020001MessageRequestW.APSalesTable.PurchOrderFormNum = parametrorequest.APSalesTable.PurchOrderFormNum;
                    APISAC020001MessageRequestW.APSalesTable.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.APSalesTable.CustAccount, longAllenar,nombre_metodo);
                    APISAC020001MessageRequestW.APSalesTable.SalesPoolId = parametrorequest.APSalesTable.SalesPoolId;
                    APISAC020001MessageRequestW.APSalesTable.SalesOriginId = parametrorequest.APSalesTable.SalesOriginId;
                    APISAC020001MessageRequestW.APSalesTable.DeliveryPostalAddress = parametrorequest.APSalesTable.DeliveryPostalAddress;
                    APISAC020001MessageRequestW.APSalesTable.InventLocationId = parametrorequest.APSalesTable.InventLocationId;
                    APISAC020001MessageRequestW.APSalesTable.WorkSalesResponsibleCode = parametrorequest.APSalesTable.WorkSalesResponsibleCode;
                    APISAC020001MessageRequestW.APSalesTable.APSalesIdProductAsistenciaFacilita = parametrorequest.APSalesTable.APSalesIdProductAsistenciaFacilita;
                    APISAC020001MessageRequestW.APSalesTable.APPaymModeGeneral = parametrorequest.APSalesTable.APPaymModeGeneral;
                    APISAC020001MessageRequestW.APSalesTable.APPromotionalCode = parametrorequest.APSalesTable.APPromotionalCode;
                    APISAC020001MessageRequestW.APSalesTable.APProductFinancialCECode = parametrorequest.APSalesTable.APProductFinancialCECode;
                    APISAC020001MessageRequestW.APSalesTable.APProductFinancialCEDescription = parametrorequest.APSalesTable.APProductFinancialCEDescription;
                    APISAC020001MessageRequestW.APSalesTable.APProductFinancialTCCode = parametrorequest.APSalesTable.APProductFinancialTCCode;
                    APISAC020001MessageRequestW.APSalesTable.APProductFinancialTCDescription = parametrorequest.APSalesTable.APProductFinancialTCDescription;
                    APISAC020001MessageRequestW.APSalesTable.APAmountPayModeCash = parametrorequest.APSalesTable.APAmountPayModeCash;
                    APISAC020001MessageRequestW.APSalesTable.APAmountPayModeCredit = parametrorequest.APSalesTable.APAmountPayModeCredit;
                    APISAC020001MessageRequestW.APSalesTable.APAmountPayModeElectronic = parametrorequest.APSalesTable.APAmountPayModeElectronic;
                    APISAC020001MessageRequestW.APSalesTable.APProductFinancialTCCode2 = parametrorequest.APSalesTable.APProductFinancialTCCode2;
                    APISAC020001MessageRequestW.APSalesTable.APProductFinancialTCDescription2 = parametrorequest.APSalesTable.APProductFinancialTCDescription2;
                    APISAC020001MessageRequestW.APSalesTable.IndependentEntrepreneurId = parametrorequest.APSalesTable.IndependentEntrepreneurId;
                    APISAC020001MessageRequestW.APSalesTable.APStoreId = parametrorequest.APSalesTable.APStoreId;
                    APISAC020001MessageRequestW.APSalesTable.PaymFirstShareDate = parametrorequest.APSalesTable.PaymFirstShareDate;
                    APISAC020001MessageRequestW.APSalesTable.PaymLastShareDate = parametrorequest.APSalesTable.PaymLastShareDate;
                    APISAC020001MessageRequestW.APSalesTable.ShareNumber = parametrorequest.APSalesTable.ShareNumber;
                    APISAC020001MessageRequestW.APSalesTable.GraceMonths = parametrorequest.APSalesTable.GraceMonths;
                
                    if (parametrorequest.APSalesTable.APBuyerNumberTickectList != null)
                    {
                        APISAC020001MessageRequestW.APSalesTable.APBuyerNumberTickectList = new List<APBuyerNumberTickectList>();
                        foreach (var elem1 in parametrorequest.APSalesTable.APBuyerNumberTickectList)
                        {
                            APBuyerNumberTickectListList = new APBuyerNumberTickectList();
                            APBuyerNumberTickectListList.APAmountGeneratedBuyerTicket = elem1.APAmountGeneratedBuyerTicket;
                            APBuyerNumberTickectListList.APBuyerNumberTickect = elem1.APBuyerNumberTickect;
                            APBuyerNumberTickectListList.APBuyerTicketCheck = elem1.APBuyerTicketCheck;
                            APBuyerNumberTickectListList.APCreditNoteBuyerTicketCheck = elem1.APCreditNoteBuyerTicketCheck;
                            APBuyerNumberTickectListList.APPostingProfile = elem1.APPostingProfile;
                            APISAC020001MessageRequestW.APSalesTable.APBuyerNumberTickectList.Add(APBuyerNumberTickectListList);
                        }
                    }

                    if (parametrorequest.APSalesTable.APSalesLineList != null)
                    {
                        APISAC020001MessageRequestW.APSalesTable.APSalesLineList = new List<APSalesLineIVTA004001>();
                        foreach (var elem in parametrorequest.APSalesTable.APSalesLineList)
                        {
                            APSalesLineIVTA004001List = new APSalesLineIVTA004001();
                            APSalesLineIVTA004001List.ItemId = elem.ItemId;
                            APSalesLineIVTA004001List.InventSerialId = elem.InventSerialId;
                            APSalesLineIVTA004001List.StyleId = elem.StyleId;
                            APSalesLineIVTA004001List.TAMFundID = elem.TAMFundID;
                            APSalesLineIVTA004001List.APComboPromotionId = elem.APComboPromotionId;
                            APSalesLineIVTA004001List.APContributionValue = elem.APContributionValue;
                            APSalesLineIVTA004001List.APUserCreateCombo = elem.APUserCreateCombo;
                            APSalesLineIVTA004001List.APPrimarySecondary = elem.APPrimarySecondary;
                            APSalesLineIVTA004001List.APComboPromotionQtyLimit = elem.APComboPromotionQtyLimit;
                            APSalesLineIVTA004001List.APComboPromotionStartDate = elem.APComboPromotionStartDate;
                            APSalesLineIVTA004001List.APComboPromotionEndDate = elem.APComboPromotionEndDate;
                            APSalesLineIVTA004001List.APPromotionPO = elem.APPromotionPO;
                            APSalesLineIVTA004001List.InventLocationIdDelivery = elem.InventLocationIdDelivery;
                            APSalesLineIVTA004001List.WMSLocationIdDelivery = elem.WMSLocationIdDelivery;
                            APSalesLineIVTA004001List.InventStatusID = elem.InventStatusID;
                            APSalesLineIVTA004001List.ReceiptDateRequested = elem.ReceiptDateRequested;
                            APSalesLineIVTA004001List.APHomeDelivery = elem.APHomeDelivery;
                            APSalesLineIVTA004001List.APInstallationDate = elem.APInstallationDate;
                            APSalesLineIVTA004001List.QTYQuantity = elem.QTYQuantity;
                            APSalesLineIVTA004001List.SalesPrice = elem.SalesPrice;
                            APSalesLineIVTA004001List.APSalesCost = elem.APSalesCost;
                            APSalesLineIVTA004001List.APSalesPriceOffert = elem.APSalesPriceOffert;
                            APSalesLineIVTA004001List.APPromotionDiscount = elem.APPromotionDiscount;
                            APSalesLineIVTA004001List.APComboDiscount = elem.APComboDiscount;
                            APSalesLineIVTA004001List.APPaymModeDiscount = elem.APPaymModeDiscount;
                            APSalesLineIVTA004001List.APInventLocationIdDiscount = elem.APInventLocationIdDiscount;
                            APSalesLineIVTA004001List.APTermDiscount = elem.APTermDiscount;
                            APSalesLineIVTA004001List.APInitialFeeDiscount = elem.APInitialFeeDiscount;
                            APSalesLineIVTA004001List.LineDisc = elem.LineDisc;
                            APSalesLineIVTA004001List.DataAreaIdStockOwn = elem.DataAreaIdStockOwn;
                            APSalesLineIVTA004001List.InventLocationIdStock = elem.InventLocationIdStock;
                            APSalesLineIVTA004001List.WMSLocationIdStock = elem.WMSLocationIdStock;
                            APSalesLineIVTA004001List.APSalesNumberAF = elem.APSalesNumberAF;
                            APSalesLineIVTA004001List.APItemIdAF = elem.APItemIdAF;
                            APSalesLineIVTA004001List.APDiscountMargin = elem.APDiscountMargin;
                            APSalesLineIVTA004001List.APDiscountMarginId = elem.APDiscountMarginId;
                            APSalesLineIVTA004001List.APECInvoiceDetail = elem.APECInvoiceDetail;
                            APSalesLineIVTA004001List.APNumLine = elem.APNumLine;
                            APSalesLineIVTA004001List.APCreatePurchaseOrder = elem.APCreatePurchaseOrder;
                            APSalesLineIVTA004001List.APInvoiceSequence= elem.APInvoiceSequence;
                            APSalesLineIVTA004001List.APInventLocationIdMoto = elem.APInventLocationIdMoto;


                            if (elem.APFinancialDimensionList != null)
                            {
                                APSalesLineIVTA004001List.APFinancialDimensionList = new List<APFinancialDimension>();
                                foreach (var elem1 in elem.APFinancialDimensionList) 
                                {
                                    APFinancialDimensionList = new APFinancialDimension();
                                    APFinancialDimensionList.Name = elem1.Name;
                                    APFinancialDimensionList.Valor = elem1.Valor;
                                    APSalesLineIVTA004001List.APFinancialDimensionList.Add(APFinancialDimensionList);
                                }
                            }
                        APISAC020001MessageRequestW.APSalesTable.APSalesLineList.Add(APSalesLineIVTA004001List);
                        
                        }
                    }
                }
                string jsonrequest2 = JsonConvert.SerializeObject(APISAC020001MessageRequestW);
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, APISAC020001MessageRequestW);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time, vl_Attempts, vl_Timeout, nombre_metodo);


                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(APISAC020001MessageRequestW);
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APIVTA004001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("I:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);  //StatusCode(StatusCodes.Status204NoContent, "No se retorno resultado de dynamics");
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(APISAC020001MessageRequestW);
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger(nombre_metodo, "CONTROLADOR: Response desde Dynamics: " + respuestaLog);

                    return Ok(respuesta);
                }
            }
            catch (Exception ex)
            {
                respuestaLog = JsonConvert.SerializeObject(APISAC020001MessageRequestW);
                respuesta = new APIVTA004001MessageResponse();
                respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("IVTA004:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger(nombre_metodo, "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }
    }
}
