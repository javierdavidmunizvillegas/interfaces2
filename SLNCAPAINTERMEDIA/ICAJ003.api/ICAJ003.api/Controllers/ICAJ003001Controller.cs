using ICAJ003.api.Infraestructura.Servicios;
using ICAJ003.api.Infraestructure.Configuration;
using ICAJ003.api.Infraestructure.Services;
using ICAJ003.api.Models;
using ICAJ003.api.Models._001.Response;
using ICAJ003.api.Models.ResponseHomologacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ICAJ003.api.Controllers
{
    
    [ApiController]
    public class ICAJ003001Controller : ControllerBase
    {
        private IManejadorRequest<APICAJ003001MessageRequest> QueueRequest;
        private IManejadorResponse<APICAJ003001MessageResponse> QueueResponse;
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static RegistroLog Logger = new RegistroLog();
        private static readonly Configuracion conf = new Configuracion();
        static IConfigurationRoot configuracion = conf.GetConfiguration();
        private static string vl_ConnectionStringRequest = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringRequest").Value);
        private static string vl_ConnectionStringResponse = Convert.ToString(configuracion.GetSection("Data").GetSection("ConectionStringResponse").Value);
        private static string vl_QueueRequest = configuracion.GetSection("Data").GetSection("QueueRequest").Value.ToString();
        private static string vl_QueueResponse = configuracion.GetSection("Data").GetSection("QueueResponse").Value.ToString();
        private static int vl_Time = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep").Value);
        private static int vl_Attempts = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts").Value);
        private static int vl_Time1 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("TimeSleep1").Value);
        private static int vl_Attempts1 = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Attempts1").Value);
        private static int vl_Timeout = Convert.ToInt32(configuracion.GetSection("Data").GetSection("Timeout").Value);
        private static string vl_Environment = Convert.ToString(configuracion.GetSection("Data").GetSection("ASPNETCORE_ENVIRONMENT").Value);
        private static string sbUriHomologacionDynamic = Convert.ToString(configuracion.GetSection("Data").GetSection("UriHomologacionDynamicSiac").Value);
        private static string sbMetodoWsUriSiac = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriSiac").Value);
        private static string sbMetodoWsUriAx = Convert.ToString(configuracion.GetSection("Data").GetSection("MetodoWsUriAx").Value);
        private static ConvierteCodigo CodigoCliente = new ConvierteCodigo();
        private static int longAllenar = Convert.ToInt32(configuracion.GetSection("Data").GetSection("LongAllenar").Value);


        public ICAJ003001Controller(IManejadorRequest<APICAJ003001MessageRequest> _manejadorRequest
            , IManejadorResponse<APICAJ003001MessageResponse> _manejadorReponse, IManejadorHomologacion<ResponseHomologacion> _homologacionRequest)
        {
            QueueRequest = _manejadorRequest;
            QueueResponse = _manejadorReponse;
            homologacionRequest = _homologacionRequest;
        }


        [HttpPost]
        [Route("APICAJ003001")]
        [ServiceFilter(typeof(ValidadorRequest))]
        public async Task<ActionResult<APICAJ003001MessageResponse>> APICAJ003001(APICAJ003001MessageRequest parametrorequest)
        {
            APICAJ003001MessageResponse respuesta = null;
            try
            {
                string nombre_metodo = "APICAJ003001";


                string jsonrequest = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ003001", "CONTROLADOR: Request Recibido: " + jsonrequest);

                ///HOMOLOGACIÓN////////////////////////////////////////////////
                string DataAreaId = parametrorequest.DataAreaId;
                ResponseHomologacion ResultadoHomologa = await homologacionRequest.GetHomologacion(sbUriHomologacionDynamic, sbMetodoWsUriSiac, DataAreaId, vl_Time, vl_Attempts, nombre_metodo);

                if (ResultadoHomologa == null)
                {
                    respuesta = new APICAJ003001MessageResponse();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("ICAJ003:E000|SERVICIO DE HOMOLOGACIÓN NO DISPONIBLE");

                    Logger.FileLogger("APICAJ003001", "CONTROLADOR WS HOMOLOGACION: No se retorno resultado de Homologación");

                    return Ok(respuesta);
                }
                else
                {
                    if (ResultadoHomologa.Response != null)
                    {
                        parametrorequest.DataAreaId = ResultadoHomologa.Response;
                    }

                }
                /////////////////////////////////////////////////////////////////////


                string sesionid = Guid.NewGuid().ToString();
                parametrorequest.SessionId = sesionid;
                parametrorequest.Enviroment = vl_Environment;

                if (parametrorequest.APLedgerJournalTableCustPaym.APLedgerJournalLineList != null) {
                    int cantidadJournalLine = parametrorequest.APLedgerJournalTableCustPaym.APLedgerJournalLineList.Count;
                    int i = 0;

                    if (cantidadJournalLine > 0)
                    {
                        do
                        {
                            int cantidadAcount = parametrorequest.APLedgerJournalTableCustPaym.APLedgerJournalLineList[i].CustAccount.Length;

                            if (cantidadAcount > 0)
                            {
                                parametrorequest.APLedgerJournalTableCustPaym.APLedgerJournalLineList[i].CustAccount= CodigoCliente.CrecosAdynamics(parametrorequest.APLedgerJournalTableCustPaym.APLedgerJournalLineList[i].CustAccount, longAllenar, "APICAJ003001");
                            }

                            if (parametrorequest.APLedgerJournalTableCustPaym.APLedgerJournalLineList[i].APPaymModeCheck != null) { 

                                int cantidadAcount2 = parametrorequest.APLedgerJournalTableCustPaym.APLedgerJournalLineList[i].APPaymModeCheck.CustAccount.Length;

                                if (cantidadAcount2 > 0)
                                {
                                    parametrorequest.APLedgerJournalTableCustPaym.APLedgerJournalLineList[i].APPaymModeCheck.CustAccount = CodigoCliente.CrecosAdynamics(parametrorequest.APLedgerJournalTableCustPaym.APLedgerJournalLineList[i].APPaymModeCheck.CustAccount, longAllenar, "APICAJ003001");
                                }

                            }

                            if (parametrorequest.APLedgerJournalTableCustPaym.APLedgerJournalLineList[i].APPaymModeElectronic != null)
                            {
                                int cantidadAcount4 = parametrorequest.APLedgerJournalTableCustPaym.APLedgerJournalLineList[i].APPaymModeElectronic.AccountNum.Length;

                                if (cantidadAcount4 > 0)
                                {
                                    parametrorequest.APLedgerJournalTableCustPaym.APLedgerJournalLineList[i].APPaymModeElectronic.AccountNum = CodigoCliente.CrecosAdynamics(parametrorequest.APLedgerJournalTableCustPaym.APLedgerJournalLineList[i].APPaymModeElectronic.AccountNum, longAllenar, "APICAJ003001");
                                }
                            }

                            i++;
                        } while (i < cantidadJournalLine);
                    }

                }

                string jsonrequest2 = JsonConvert.SerializeObject(parametrorequest);
                Logger.FileLogger("APICAJ003001", "CONTROLADOR: Request Enviado a Dynamics: " + jsonrequest2);

                await QueueRequest.EnviarMensajeAsync(sesionid, vl_ConnectionStringRequest, vl_QueueRequest, parametrorequest);

                respuesta = await QueueResponse.RecibeMensajeSesion(vl_ConnectionStringResponse, vl_QueueResponse, sesionid, vl_Time1, vl_Attempts1, vl_Timeout, nombre_metodo);


                if (respuesta == null)
                {
                    respuesta = new APICAJ003001MessageResponse();
                    respuesta.ErrorList = new List<string>();

                    respuesta.ErrorList.Add("ICAJ003:E000|NO SE RETORNO RESULTADO DE DYNAMICS");
                    Logger.FileLogger("APICAJ003001", "CONTROLADOR: No se retorno resultado de Dynamics");
                    return Ok(respuesta);

                }
                else
                {

                    if (respuesta.APLedgerJournalTableCustPaym != null)
                    {
                        int cantidadJournalLine2 = respuesta.APLedgerJournalTableCustPaym.Count();
                        int j = 0;

                        if (cantidadJournalLine2 > 0)
                        {
                            do
                            {
                                if (respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList != null)
                                {
                                    int cantidadJournalLine3 = respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList.Count();
                                    int k = 0;

                                    if (cantidadJournalLine3 > 0)
                                    {
                                        do
                                        {
                                            int cantidadAcount = respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].CustAccount.Length;

                                            if (cantidadAcount > 0 && cantidadAcount < 11)
                                            {
                                                respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].CustAccount = Int32.Parse(CodigoCliente.DynamicAcrecos(respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].CustAccount, "APICAJ003001")).ToString();
                                            }
                                            if (respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APVendTransRegistrationFine != null)
                                            {
                                                if (respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APVendTransRegistrationFine.CustAccount != null && respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APVendTransRegistrationFine.CustAccount != "")
                                                {
                                                    respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APVendTransRegistrationFine.CustAccount = Int32.Parse(CodigoCliente.DynamicAcrecos(respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APVendTransRegistrationFine.CustAccount, "APICAJ003001")).ToString();
                                                }
                                            }
                                            if (respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeElectronic != null)
                                            {
                                                if (respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeElectronic.AccountNum != null && respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeElectronic.AccountNum != "")
                                                {
                                                    respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeElectronic.AccountNum = Int32.Parse(CodigoCliente.DynamicAcrecos(respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeElectronic.AccountNum, "APICAJ003001")).ToString();
                                                }   
                                            }
                                            if (respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeCheck != null)
                                            {
                                                if (respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeCheck.CustAccount != null && respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeCheck.CustAccount != "")
                                                {
                                                    respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeCheck.CustAccount = Int32.Parse(CodigoCliente.DynamicAcrecos(respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeCheck.CustAccount, "APICAJ003001")).ToString();
                                                }
                                                if (respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeCheck.AccountNum != null && respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeCheck.AccountNum != "")
                                                {
                                                    respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeCheck.AccountNum = Int32.Parse(CodigoCliente.DynamicAcrecos(respuesta.APLedgerJournalTableCustPaym[j].APLedgerJournalLineList[k].APPaymModeCheck.AccountNum, "APICAJ003001")).ToString();
                                                }

                                            }
                                            k++;
                                        } while (k < cantidadJournalLine3);

                                    }

                                }
                                j++;
                            } while (j < cantidadJournalLine2);


                        }
                    }
                }

                string jsonrequest3 = JsonConvert.SerializeObject(respuesta);
                Logger.FileLogger("APICAJ003001", "CONTROLADOR: Response desde Dynamics: " + jsonrequest3);

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new APICAJ003001MessageResponse();
                respuesta.ErrorList = new List<string>();

                respuesta.ErrorList.Add("ICAJ003:E000|NO SE RETORNO RESULTADOS DE DYNAMICS");
                Logger.FileLogger("APICAJ003001", "CONTROLADOR: ERROR: " + ex.ToString());
                return Ok(respuesta);

            }

        }

    }
}
