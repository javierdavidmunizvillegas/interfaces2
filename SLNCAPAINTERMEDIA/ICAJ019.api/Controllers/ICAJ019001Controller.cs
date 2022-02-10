using ICAJ019.api.Infraestructura.Configuracion;
using ICAJ019.api.Infraestructura.Services;
using ICAJ019.api.Models._001.Request;
using ICAJ019.api.Models._001.Response;
using ICAJ019.api.Models.Homologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static ICAJ019.api.Validator;

namespace ICAJ019.api.Controllers
{
    [ApiController]
    public class ICAJ019001Controller : Controller
    {
        private IManejadorRequestQueue<APICAJ019001MessageRequest> manejadorRequestQueue;
        private IManejadorResponseQueue2<APICAJ019001MessageResponse> manejadorReponseQueue;
        private IHomologacionService<ResponseHomologa> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string Entorno = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        private static string sbConenctionStringEnvio = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest001").Value);
        private static string sbConenctionStringReceptar = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse001").Value);
        private static string nombrecolarequest = configuracion.GetSection("Data").GetSection("QueueRequest001").Value.ToString();
        private static string nombrecolaresponse = configuracion.GetSection("Data").GetSection("QueueResponse001").Value.ToString();
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep").Value);
        private static int vl_Intentos = Convert.ToInt32(configuracion.GetSection("Data").GetSection("IntentosREsponse").Value);
        private static int vl_TimeWS = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleepWS").Value);
        private static int vl_IntentosWS = Convert.ToInt32(configuracion.GetSection("Data").GetSection("IntentosWS").Value);
        private static int vl_TimeOutResp = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeOutResponse").Value);
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32((configuracion.GetSection("Data").GetSection("LongAllenar").Value));
        string respuestaLog;
        const int SegAMiliS = 1000;
        const int NanoAMiliS = 10000;
        public ICAJ019001Controller(IManejadorRequestQueue<APICAJ019001MessageRequest> _manejadorRequestQueue
        , IManejadorResponseQueue2<APICAJ019001MessageResponse> _manejadorReponseQueue, IHomologacionService<ResponseHomologa> _homologacionRequest)
    {
        manejadorRequestQueue = _manejadorRequestQueue;
        manejadorReponseQueue = _manejadorReponseQueue;
        homologacionRequest = _homologacionRequest;
    }
    [HttpPost]
    [Route("APICAJ019001")]
        [ServiceFilter(typeof(ValidationFilter001Attribute))]
        public async Task<ActionResult<APICAJ019001MessageResponse>> APICAJ019001(APICAJ019001MessageRequest parametrorequest)
        {


            if (parametrorequest == null)
            {
                // Log
                Logger.FileLogger("APICAJ019001", "CONTROLADOR: El parámetro request es nulo.");
                return BadRequest();
            }

            //medir tiempo transcurrido en ws
            long start = 0, end = 0;
            double diff = 0;
            string responseHomologa = string.Empty;
            APICAJ019001MessageResponse respuesta = null;
            ResponseHomologa ResuldatoHomologa = null;
            APICAJ019001MessageRequest APICAJ019001MessageRequestW = new APICAJ019001MessageRequest();
            APEntityCreditNoteLine APEntityCreditNoteLineList = null;
            APEntityCreditNoteTable APEntityCreditNoteTableList = null;
            APEntityInvoiceReceivableLine APEntityInvoiceReceivableLineList = null;
            APEntityInvoiceServiceLine APEntityInvoiceServiceLineList = null;
            APEntityInvoiceServiceTable APEntityInvoiceServiceTableList = null;
            APEntityOrderDevolutionLine APEntityOrderDevolutionLineList = null;
            APEntityOrderDevolutionTable APEntityOrderDevolutionTableList = null;
            APEntityReceivableDevolutionLine APEntityReceivableDevolutionLineList = null;
            APEntityReceivableLine APEntityReceivableLineList = null;
            APEntityReceivableTable APEntityReceivableTableList = null;
            APEntitySalesOrderInvoiceLine APEntitySalesOrderInvoiceLineList = null;
            APEntitySalesOrderInvoiceTable APEntitySalesOrderInvoiceTableList = null;
            APICAJ019001MessageResponse APICAJ019001MessageResponseW = new APICAJ019001MessageResponse();
            List<APEntityCreditNoteTable> APEntityCreditNoteTableListCod = null;
            List<APEntityInvoiceServiceTable> APEntityInvoiceServiceTableCod = null;
            List<APEntityOrderDevolutionTable> APEntityOrderDevolutionTableCod = null;
            List<APEntitySalesOrderInvoiceTable> APEntitySalesOrderInvoiceTableCod = null;
            List<APEntityReceivableTable> APEntityReceivableTableCod = null;

            try
            {
                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ019001", "CONTROLADOR: Request Recibido : " + respuestaLog);
                string DataAreaId = parametrorequest.DataAreaId;


                if (string.IsNullOrEmpty(DataAreaId) || string.IsNullOrWhiteSpace(DataAreaId))
                    throw new Exception("CONTROLADOR WS HOMOLOGACION: El código de empresa no debe ser nulo.");

                start = Stopwatch.GetTimestamp();
                int cont = 0;
                for (int i = 1; i < vl_IntentosWS; i++)
                {
                    try
                    {

                        ResuldatoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId);

                        // Validacion el objeto recibido
                        // Condicion para salir
                        if (ResuldatoHomologa != null && (string.IsNullOrEmpty(ResuldatoHomologa.Response) || string.IsNullOrWhiteSpace(ResuldatoHomologa.Response)))
                        {
                            respuesta = new APICAJ019001MessageResponse();
                            respuesta.SessionId = string.Empty;
                            respuesta.StatusId = false;
                            respuesta.ErrorList = new List<string>();
                            respuesta.ErrorList.Add("ICAJ019:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                            return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                        }
                        if (ResuldatoHomologa != null && "OK".Equals(ResuldatoHomologa.DescripcionId))
                        {
                            responseHomologa = ResuldatoHomologa.Response ?? string.Empty; //parametrorequest.DataAreaId;
                            parametrorequest.DataAreaId = responseHomologa;
                            Logger.FileLogger("APICAJ019001", $"CONTROLADOR WS HOMOLOGACION: Código de empresa a enviarse a Dynamics es : {responseHomologa}");
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
                Logger.FileLogger("APICAJ019001", $"CONTROLADOR WS HOMOLOGACION: Número de intentos realizados : {cont} En Ws Homologación,Tiempo Transcurrido : {diff} ms.");

                if (ResuldatoHomologa == null)
                {
                    respuesta = new APICAJ019001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ019:E000|SERVICIO DE HOMOLOGACION NO DISPONIBLE");
                    return Ok(respuesta); // StatusCode(StatusCodes.Status204NoContent, respuesta);
                }

                // asignar campo ambiente
                parametrorequest.Enviroment = Entorno;

                // asigna session
                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                // convertir codigo cliente de crecos a dynamics
                APICAJ019001MessageRequestW.DataAreaId = parametrorequest.DataAreaId;
                APICAJ019001MessageRequestW.Enviroment = parametrorequest.Enviroment;
                APICAJ019001MessageRequestW.SessionId = parametrorequest.SessionId;
                APICAJ019001MessageRequestW.EntidadList = parametrorequest.EntidadList;
                APICAJ019001MessageRequestW.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.CustAccount, longAllenar,"APICAJ019001");
                APICAJ019001MessageRequestW.DateStart = parametrorequest.DateStart;
                APICAJ019001MessageRequestW.DateEnd = parametrorequest.DateEnd;
                APICAJ019001MessageRequestW.InvoiceId = parametrorequest.InvoiceId;
                APICAJ019001MessageRequestW.Voucher = parametrorequest.Voucher;
                APICAJ019001MessageRequestW.APRecibeSIAC = parametrorequest.APRecibeSIAC;
                APICAJ019001MessageRequestW.SalesId = parametrorequest.SalesId;
                APICAJ019001MessageRequestW.ReturnItemNum = parametrorequest.ReturnItemNum;
                APICAJ019001MessageRequestW.APTransactionType = parametrorequest.APTransactionType;
                APICAJ019001MessageRequestW.EinvoiceReasonRefund = parametrorequest.EinvoiceReasonRefund;
                APICAJ019001MessageRequestW.PaymMode = parametrorequest.PaymMode;

                //
                respuestaLog = JsonConvert.SerializeObject(APICAJ019001MessageRequestW);
                Logger.FileLogger("APICAJ019001", "CONTROLADOR: Request despues de conversion, desde Dynamics: " + respuestaLog);

                await manejadorRequestQueue.EnviarMensajeAsync(sesionid, sbConenctionStringEnvio, nombrecolarequest, APICAJ019001MessageRequestW);

                respuesta = await manejadorReponseQueue.RecibeMensajeSesion(sbConenctionStringReceptar, nombrecolaresponse, sesionid, vl_Time, vl_Intentos, "APICAJ019001", vl_TimeOutResp);

                if (respuesta == null)
                {
                    respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                    Logger.FileLogger("APICAJ019001", "CONTROLADOR: No se retorno resultado de Dynamics. ");
                    Logger.FileLogger("APICAJ019001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuesta = new APICAJ019001MessageResponse();
                    respuesta.SessionId = string.Empty;
                    respuesta.StatusId = false;
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ019:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    return Ok(respuesta);  //StatusCode(StatusCodes.Status204NoContent, "No se retorno resultado de dynamics");
                }
                else
                {
                    respuestaLog = JsonConvert.SerializeObject(APICAJ019001MessageRequestW);
                    Logger.FileLogger("APICAJ019001", "CONTROLADOR: Request Enviado a Dynamics : " + respuestaLog);
                    respuestaLog = JsonConvert.SerializeObject(respuesta);
                    Logger.FileLogger("APICAJ019001", "CONTROLADOR: Response antes de conversion desde Dynamics: " + respuestaLog);

                    //convertir codigo cliente de dynamics a crecos
                    APICAJ019001MessageResponseW.SessionId = respuesta.SessionId;
                    APICAJ019001MessageResponseW.StatusId = respuesta.StatusId;
                    if (respuesta.ErrorList != null)
                    {
                        APICAJ019001MessageResponseW.ErrorList = new List<string>();
                        foreach (var ele in respuesta.ErrorList) 
                        {
                            APICAJ019001MessageResponseW.ErrorList.Add(ele);
                        }
                    }
                    if (respuesta.APEntityCreditNoteTableList != null)
                    {
                        APICAJ019001MessageResponseW.APEntityCreditNoteTableList = new List<APEntityCreditNoteTable>();
                        foreach (var elem in respuesta.APEntityCreditNoteTableList)
                        {
                            APEntityCreditNoteTableList = new APEntityCreditNoteTable();
                            APEntityCreditNoteTableList.CustAccount = elem.CustAccount;
                            APEntityCreditNoteTableList.CreditNote = elem.CreditNote;
                            APEntityCreditNoteTableList.Amount = elem.Amount;
                            APEntityCreditNoteTableList.Date = elem.Date;
                            APEntityCreditNoteTableList.DocumentOrigin = elem.DocumentOrigin;
                            APEntityCreditNoteTableList.VoucherCreditNote = elem.VoucherCreditNote;
                            APEntityCreditNoteTableList.MotiveCreditNote = elem.MotiveCreditNote;
                            if (elem.TypeOfDocument != null)
                                APEntityCreditNoteTableList.TypeOfDocument = elem.TypeOfDocument;
                            APEntityCreditNoteTableList.APEntityCreditNoteLineList = new List<APEntityCreditNoteLine>();
                            if (elem.APEntityCreditNoteLineList != null)
                            foreach (var elem1 in elem.APEntityCreditNoteLineList)
                            {
                                APEntityCreditNoteLineList = new APEntityCreditNoteLine();
                                APEntityCreditNoteLineList.CreditNote = elem1.CreditNote;
                                APEntityCreditNoteLineList.DateLiquidate = elem1.DateLiquidate;
                                APEntityCreditNoteLineList.Amount = elem1.Amount;
                                APEntityCreditNoteLineList.Document = elem1.Document;
                                APEntityCreditNoteLineList.VoucherLiquidate = elem1.VoucherLiquidate;
                                APEntityCreditNoteLineList.LastSettleVoucher = elem1.LastSettleVoucher;
                                APEntityCreditNoteLineList.RecibeSIAC = elem1.RecibeSIAC;
                                APEntityCreditNoteLineList.AmountInvoice = elem1.AmountInvoice;
                                APEntityCreditNoteLineList.AmountSettle = elem1.AmountSettle;
                              //  APEntityCreditNoteLineList.TypeOfDocument = elem1.TypeOfDocument;
                              //  APEntityCreditNoteLineList.DocumeRecibeSIACnt = elem1.DocumeRecibeSIACnt;
                                APEntityCreditNoteTableList.APEntityCreditNoteLineList.Add(APEntityCreditNoteLineList);
                            }
                            APICAJ019001MessageResponseW.APEntityCreditNoteTableList.Add(APEntityCreditNoteTableList);
                        }
                    
                        APEntityCreditNoteTableListCod = APICAJ019001MessageResponseW.APEntityCreditNoteTableList;
                        foreach (var ListElem in APEntityCreditNoteTableListCod)
                        {  if (ListElem.CustAccount != null)
                            ListElem.CustAccount = CodigoCliente.DynamicAcrecos(ListElem.CustAccount,"APICAJ019001");
                        }
                    }
                    if (respuesta.APEntityInvoiceServiceTableList != null)
                    {
                        APICAJ019001MessageResponseW.APEntityInvoiceServiceTableList = new List<APEntityInvoiceServiceTable>();
                        if (respuesta.APEntityInvoiceServiceTableList != null)
                        {
                            foreach (var elem in respuesta.APEntityInvoiceServiceTableList)
                            {
                                APEntityInvoiceServiceTableList = new APEntityInvoiceServiceTable();
                                APEntityInvoiceServiceTableList.CustAccount = elem.CustAccount;
                                APEntityInvoiceServiceTableList.Invoice = elem.Invoice;
                                APEntityInvoiceServiceTableList.Amount = elem.Amount;
                                APEntityInvoiceServiceTableList.InvoiceDate = elem.InvoiceDate;
                                APEntityInvoiceServiceTableList.DocumentRelation = elem.DocumentRelation;
                                APEntityInvoiceServiceTableList.InvoiceVoucher = elem.InvoiceVoucher;
                                if (elem.TypeOfDocument != null )
                                APEntityInvoiceServiceTableList.TypeOfDocument = elem.TypeOfDocument;
                                APEntityInvoiceServiceTableList.APEntityInvoiceServiceLineList = new List<APEntityInvoiceServiceLine>();
                                if (elem.APEntityInvoiceServiceLineList != null)
                                {
                                    foreach (var elem1 in elem.APEntityInvoiceServiceLineList)
                                    {
                                        APEntityInvoiceServiceLineList = new APEntityInvoiceServiceLine();
                                        APEntityInvoiceServiceLineList.Invoice = elem1.Invoice;
                                        APEntityInvoiceServiceLineList.DateLiquidate = elem1.DateLiquidate;
                                        APEntityInvoiceServiceLineList.Amount = elem1.Amount;
                                        APEntityInvoiceServiceLineList.VoucherLiquidate = elem1.VoucherLiquidate;
                                        APEntityInvoiceServiceLineList.RecibeSIAC = elem1.RecibeSIAC;
                                        APEntityInvoiceServiceLineList.MotiveCreditNote = elem1.MotiveCreditNote;
                                        APEntityInvoiceServiceLineList.LastSettleVoucher = elem1.LastSettleVoucher;
                                        APEntityInvoiceServiceLineList.TransactionType = elem1.TransactionType;
                                        APEntityInvoiceServiceLineList.PaymMode = elem1.PaymMode;
                                        APEntityInvoiceServiceLineList.CreditNote = elem1.CreditNote;
                                        APEntityInvoiceServiceLineList.AmountSettle = elem1.AmountSettle;
                                  //      APEntityInvoiceServiceLineList.TypeOfDocument = elem1.TypeOfDocument;
                                        APEntityInvoiceServiceTableList.APEntityInvoiceServiceLineList.Add(APEntityInvoiceServiceLineList);
                                    }
                                }
                                APICAJ019001MessageResponseW.APEntityInvoiceServiceTableList.Add(APEntityInvoiceServiceTableList);
                            }
                        }
                        APEntityInvoiceServiceTableCod = APICAJ019001MessageResponseW.APEntityInvoiceServiceTableList;
                        foreach (var ListElem in APEntityInvoiceServiceTableCod)
                        {
                            ListElem.CustAccount = CodigoCliente.DynamicAcrecos(ListElem.CustAccount, "APICAJ019001");
                        }
                    }
                    if (respuesta.APEntityOrderDevolutionTableList != null)
                    {
                        APICAJ019001MessageResponseW.APEntityOrderDevolutionTableList = new List<APEntityOrderDevolutionTable>();
                       
                            foreach (var elem in respuesta.APEntityOrderDevolutionTableList)
                            {
                                APEntityOrderDevolutionTableList = new APEntityOrderDevolutionTable();
                                APEntityOrderDevolutionTableList.CustAccount = elem.CustAccount;
                                APEntityOrderDevolutionTableList.ADM = elem.ADM;
                                APEntityOrderDevolutionTableList.ReturnReasonCode = elem.ReturnReasonCode;
                                APEntityOrderDevolutionTableList.SalesIdOrigin = elem.SalesIdOrigin;
                                APEntityOrderDevolutionTableList.InvoiceIdOrigin = elem.InvoiceIdOrigin;
                                APEntityOrderDevolutionTableList.SalesId = elem.SalesId;
                                APEntityOrderDevolutionTableList.Amount = elem.Amount;
                                APEntityOrderDevolutionTableList.Status = elem.Status;
                                
                                if (elem.APEntityOrderDevolutionLineList != null)
                                {
                                    APEntityOrderDevolutionTableList.APEntityOrderDevolutionLineList = new List<APEntityOrderDevolutionLine>();
                                    foreach (var elem1 in elem.APEntityOrderDevolutionLineList)
                                    {
                                        APEntityOrderDevolutionLineList = new APEntityOrderDevolutionLine();
                                        APEntityOrderDevolutionLineList.SalesId = elem1.SalesId;
                                        APEntityOrderDevolutionLineList.CreditNote = elem1.CreditNote;
                                        APEntityOrderDevolutionLineList.VoucherCreditNote = elem1.VoucherCreditNote;
                                        APEntityOrderDevolutionLineList.DateCreditNote = elem1.DateCreditNote;
                                        APEntityOrderDevolutionLineList.ReasonCreditNote = elem1.ReasonCreditNote;
                                        APEntityOrderDevolutionLineList.AmountCreditNote = elem1.AmountCreditNote;
                                        if (elem1.TypeOfDocument != null)
                                        APEntityOrderDevolutionLineList.TypeOfDocument = elem1.TypeOfDocument;
                                       
                                        if (elem1.APEntityReceivableDevolutionLineList != null)
                                        {
                                            APEntityOrderDevolutionLineList.APEntityReceivableDevolutionLineList = new List<APEntityReceivableDevolutionLine>();
                                            foreach (var elem2 in elem1.APEntityReceivableDevolutionLineList)
                                            {
                                                APEntityReceivableDevolutionLineList = new APEntityReceivableDevolutionLine();
                                                APEntityReceivableDevolutionLineList.DateLiquidate = elem2.DateLiquidate;
                                                APEntityReceivableDevolutionLineList.Amount = elem2.Amount;
                                                APEntityReceivableDevolutionLineList.VoucherLiquidate = elem2.VoucherLiquidate;
                                                APEntityReceivableDevolutionLineList.RecibeSIAC = elem2.RecibeSIAC;
                                                APEntityReceivableDevolutionLineList.Invoice = elem2.Invoice;
                                                APEntityReceivableDevolutionLineList.LastSettleVoucher = elem2.LastSettleVoucher;
                                                APEntityReceivableDevolutionLineList.AmountSettle = elem2.AmountSettle;
                                            if (elem2.TypeOfDocument != null)
                                                APEntityReceivableDevolutionLineList.TypeOfDocument = elem2.TypeOfDocument;
                                                APEntityOrderDevolutionLineList.APEntityReceivableDevolutionLineList.Add(APEntityReceivableDevolutionLineList);
                                            }
                                        }
                                        APEntityOrderDevolutionTableList.APEntityOrderDevolutionLineList.Add(APEntityOrderDevolutionLineList);
                                    }

                                 APICAJ019001MessageResponseW.APEntityOrderDevolutionTableList.Add(APEntityOrderDevolutionTableList);
                            }
                        }
                            APEntityOrderDevolutionTableCod = APICAJ019001MessageResponseW.APEntityOrderDevolutionTableList;
                            foreach (var ListElem in APEntityOrderDevolutionTableCod)
                            {
                                ListElem.CustAccount = CodigoCliente.DynamicAcrecos(ListElem.CustAccount, "APICAJ019001");
                            }
                       
                    }
                    if (respuesta.APEntityReceivableTableList != null)
                    {
                        APICAJ019001MessageResponseW.APEntityReceivableTableList = new List<APEntityReceivableTable>();
                        foreach (var elem in respuesta.APEntityReceivableTableList)
                        {
                            APEntityReceivableTableList = new APEntityReceivableTable();
                            APEntityReceivableTableList.TransType = elem.TransType;
                            APEntityReceivableTableList.TransactionType = elem.TransactionType;
                            APEntityReceivableTableList.CustAccount = elem.CustAccount;
                            APEntityReceivableTableList.SalesId = elem.SalesId;
                            APEntityReceivableTableList.VoucherReceivable = elem.VoucherReceivable;
                            APEntityReceivableTableList.DocumentSIAC = elem.DocumentSIAC;
                            APEntityReceivableTableList.PaymMode = elem.PaymMode;
                            APEntityReceivableTableList.AmountReceivable = elem.AmountReceivable;
                            APEntityReceivableTableList.Date = elem.Date;
                            if (elem.APEntityReceivableLineList != null)
                            {
                                APEntityReceivableTableList.APEntityReceivableLineList = new List<APEntityReceivableLine>();
                                foreach (var elem1 in elem.APEntityReceivableLineList)
                                {
                                    APEntityReceivableLineList = new APEntityReceivableLine();
                                    APEntityReceivableLineList.Invoice = elem1.Invoice;
                                    APEntityReceivableLineList.VoucherLiquidate = elem1.VoucherLiquidate;
                                    APEntityReceivableLineList.LastSettleVoucher = elem1.LastSettleVoucher;
                                    APEntityReceivableLineList.VoucherReverse = elem1.VoucherReverse;
                                    APEntityReceivableLineList.DateLiquidate = elem1.DateLiquidate;
                                    APEntityReceivableLineList.AmountLiquidate = elem1.AmountLiquidate;
                                    APEntityReceivableLineList.MotiveReverse = elem1.MotiveReverse;
                                    APEntityReceivableLineList.AmountSettle = elem1.AmountSettle;
                                    APEntityReceivableTableList.APEntityReceivableLineList.Add(APEntityReceivableLineList);
                                }
                            }
                            APICAJ019001MessageResponseW.APEntityReceivableTableList.Add(APEntityReceivableTableList);
                        }
                        APEntityReceivableTableCod = APICAJ019001MessageResponseW.APEntityReceivableTableList;
                        foreach (var ListElem in APEntityReceivableTableCod)
                        {
                            ListElem.CustAccount = CodigoCliente.DynamicAcrecos(ListElem.CustAccount, "APICAJ019001");
                        }
                    }
                    if (respuesta.APEntitySalesOrderInvoiceTableList != null)
                    {
                        APICAJ019001MessageResponseW.APEntitySalesOrderInvoiceTableList = new List<APEntitySalesOrderInvoiceTable>();
                        foreach (var elem in respuesta.APEntitySalesOrderInvoiceTableList)
                        {
                            APEntitySalesOrderInvoiceTableList = new APEntitySalesOrderInvoiceTable();
                            APEntitySalesOrderInvoiceTableList.CustAccount = elem.CustAccount;
                            APEntitySalesOrderInvoiceTableList.SalesId = elem.SalesId;
                            APEntitySalesOrderInvoiceTableList.OrderClient = elem.OrderClient;
                            APEntitySalesOrderInvoiceTableList.SalesStatus = elem.SalesStatus;
                            APEntitySalesOrderInvoiceTableList.AmountOrder = elem.AmountOrder;
                            
                            if (elem.APEntitySalesOrderInvoiceLineList != null)
                            {
                                APEntitySalesOrderInvoiceTableList.APEntitySalesOrderInvoiceLineList = new List<APEntitySalesOrderInvoiceLine>();
                                foreach (var elem1 in elem.APEntitySalesOrderInvoiceLineList)
                                {
                                    APEntitySalesOrderInvoiceLineList = new APEntitySalesOrderInvoiceLine();
                                    APEntitySalesOrderInvoiceLineList.SalesId = elem1.SalesId;
                                    APEntitySalesOrderInvoiceLineList.Invoice = elem1.Invoice;
                                    APEntitySalesOrderInvoiceLineList.InvoiceVoucher = elem1.InvoiceVoucher;
                                    APEntitySalesOrderInvoiceLineList.InvoiceDate = elem1.InvoiceDate;
                                    APEntitySalesOrderInvoiceLineList.InvoiceAmount = elem1.InvoiceAmount;
                                    if (elem1.APEntityInvoiceReceivableLineList != null)
                                    {
                                        APEntitySalesOrderInvoiceLineList.APEntityInvoiceReceivableLineList = new List<APEntityInvoiceReceivableLine>();
                                        foreach (var elem2 in elem1.APEntityInvoiceReceivableLineList)
                                        {
                                            APEntityInvoiceReceivableLineList = new APEntityInvoiceReceivableLine();
                                            APEntityInvoiceReceivableLineList.DateLiquidate = elem2.DateLiquidate;
                                            APEntityInvoiceReceivableLineList.Amount = elem2.Amount;
                                            APEntityInvoiceReceivableLineList.VoucherLiquidate = elem2.VoucherLiquidate;
                                            APEntityInvoiceReceivableLineList.RecibeSIAC = elem2.RecibeSIAC;
                                            APEntityInvoiceReceivableLineList.MotiveCreditNote = elem2.MotiveCreditNote;
                                            APEntityInvoiceReceivableLineList.CodeMotiveDevolution = elem2.CodeMotiveDevolution;
                                            APEntityInvoiceReceivableLineList.TransactionType = elem2.TransactionType;
                                            APEntityInvoiceReceivableLineList.PaymMode = elem2.PaymMode;
                                            APEntityInvoiceReceivableLineList.LastSettleVoucher = elem2.LastSettleVoucher;
                                            APEntityInvoiceReceivableLineList.CreditNote = elem2.CreditNote;
                                            APEntityInvoiceReceivableLineList.AmountSettle = elem2.AmountSettle;
                                        //    APEntityInvoiceReceivableLineList.TypeOfDocument = elem2.TypeOfDocument;
                                            APEntitySalesOrderInvoiceLineList.APEntityInvoiceReceivableLineList.Add(APEntityInvoiceReceivableLineList);

                                        }
                                    }
                                    APEntitySalesOrderInvoiceTableList.APEntitySalesOrderInvoiceLineList.Add(APEntitySalesOrderInvoiceLineList);
                                }
                            }
                            APICAJ019001MessageResponseW.APEntitySalesOrderInvoiceTableList.Add(APEntitySalesOrderInvoiceTableList);
                       
                            APEntitySalesOrderInvoiceTableCod = APICAJ019001MessageResponseW.APEntitySalesOrderInvoiceTableList;
                            foreach (var ListElem in APEntitySalesOrderInvoiceTableCod)
                            {
                                ListElem.CustAccount = CodigoCliente.DynamicAcrecos(ListElem.CustAccount, "APICAJ019001");
                            }
                        }
                    }
                    respuestaLog = JsonConvert.SerializeObject(APICAJ019001MessageResponseW);
                    Logger.FileLogger("APICAJ019001", "CONTROLADOR: Response despues de conversion, desde Dynamics: " + respuestaLog);
                    return Ok(APICAJ019001MessageResponseW);
                }
            }
            catch (Exception ex)
            {

                respuestaLog = JsonConvert.SerializeObject(parametrorequest);
                respuesta = new APICAJ019001MessageResponse();
                respuesta.SessionId = string.Empty;
                respuesta.StatusId = false;
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ICAJ019:E000|NO SE RETORNO RESULTADO DE DYNAMICS"); //ex.Message
                Logger.FileLogger("APICAJ019001", "CONTROLADOR: Request Enviado a Dynamics: " + respuestaLog);
                Logger.FileLogger("APICAJ019001", "CONTROLADOR: Error por Exception: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, respuesta);

            }

        }

    }



}

