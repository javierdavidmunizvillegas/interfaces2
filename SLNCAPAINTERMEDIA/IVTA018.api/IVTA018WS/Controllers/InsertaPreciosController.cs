using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Web.Http;
using System.Web.Http.Description;
using IVTA018WS.Infraestructure.Services;
using IVTA018WS.Models;
using IVTA018WS.Models.Homologacion;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace IVTA018WS.Controllers
{
    public class InsertaPreciosController : ApiController
    {
        private RestClient client;
        private Precios db = new Precios();
        string DataAreaIdHom = "";
        private IManejadorHomologacion<ResponseHomologacion> homologacionRequest;
        private static string sbUriHomologacionDynamic = ConfigurationManager.AppSettings["UriHomologacionDynamicSiac"];
        private static string sbMetodoWsUriSiac = ConfigurationManager.AppSettings["MetodoWsUriSiac"];
        private static string sbMetodoWsUriAx = ConfigurationManager.AppSettings["MetodoWsUriAx"];
        private static int vl_Time = Convert.ToInt32(ConfigurationManager.AppSettings["TimeSleep"]);
        private static int vl_Attempts = Convert.ToInt32(ConfigurationManager.AppSettings["Attempts"]);       
        private static string sbEILiberCreUn = ConfigurationManager.AppSettings["EILiberCreUn"];
        private static string sbEILiberDisUn = ConfigurationManager.AppSettings["EILiberDisUn"];
        private static string sbEILiberGarUn = ConfigurationManager.AppSettings["EILiberGarUn"];

        // POST: api/ConsultaPrecios
        [ResponseType(typeof(APIVTA018002MessageResponse))]
        public IHttpActionResult InsertaPrecio([FromBody] APIVTA018002MessageResponse _response)
        {

            List<FlintFoxPVPMax> listDetails = new List<FlintFoxPVPMax>();

            ResponseWS respuesta = null;
            string hostName = Dns.GetHostName();
            IPHostEntry ip = Dns.GetHostEntry(hostName);
            WindowsIdentity identidad = WindowsIdentity.GetCurrent();
            string IDdeUsuario = identidad.Name;
            string EstInv = "";
            string serie = "";
           
            ///HOMOLOGACIÓN////////////////////////////////////////////////
            string DataAreaId = _response.DataAreaId;


            DataAreaIdHom = retornarDataAreaId(DataAreaId);
            if (DataAreaIdHom != "")
            {
                DataAreaId = DataAreaIdHom;
            }
            //////////////////////////////////////////////////////////////

             //Parámetros attstring//////////////////////////////////////

             var cliente = (from a in db.VW_ContiGenPClasificador
                           where a.CDESCRIPCION == "CustAccount"
                           select a).FirstOrDefault();

            var ccliente = (from a in db.VW_ContiGenPClasificador
                            where a.CDESCRIPCION == "ClfClte"
                            select a).FirstOrDefault();

            var pplazo = (from a in db.VW_ContiGenPClasificador
                          where a.CDESCRIPCION == "Plazo"
                          select a).FirstOrDefault();

            var ccanal = (from a in db.VW_ContiGenPClasificador
                          where a.CDESCRIPCION == "Canal"
                          select a).FirstOrDefault();

            var cantnserie = (from a in db.FlintFoxProductWork
                          where a.RegisterID.ToString() == _response.Registrationid
                          select a).Count();
            if (cantnserie > 0)
            {
                var nserie = (from a in db.FlintFoxProductWork
                              where a.RegisterID.ToString() == _response.Registrationid
                              select a).FirstOrDefault();
                serie = nserie.inventSerialId;
            }

            

            if (_response.AttString != null)
            {
                if (_response.AttString.Contains(sbEILiberCreUn))
                {
                    EstInv = sbEILiberCreUn;
                }
                if (_response.AttString.Contains(sbEILiberDisUn))
                {
                    EstInv = sbEILiberDisUn;
                }
                if (_response.AttString.Contains(sbEILiberGarUn))
                {
                    EstInv = sbEILiberGarUn;
                }
            }

            ////////////////////////////////////////////////////////////

            try
            {
                if (_response.APPriceDetails != null)
                {
                    int cantidad = _response.APPriceDetails.Count();
                    int i = 0;
                                      
                    if (cantidad > 0)
                    {
                        var PrePVPMax = new FlintFoxPVPMax
                        {
                            IdProd = _response.Registrationid,
                            Empresa = DataAreaId,
                            Cliente = cliente.CDESCRIPCIONCORTA,
                            CodProducto = _response.ItemId,
                            NumSerie = cantnserie > 0 ? serie : "",
                            CalificacionCliente = ccliente.CDESCRIPCIONCORTA,
                            EstadoInventario = EstInv,
                            Plazo = pplazo.CDESCRIPCIONCORTA,
                            Canal = ccanal.CDESCRIPCIONCORTA,
                            AttString = _response.AttString,
                        };

                        db.FlintFoxPVPMax.Add(PrePVPMax);
                        //db.SaveChanges();
                        try
                        {
                            db.SaveChanges();
                        }
                        catch (DbEntityValidationException ex)
                        {
                            foreach (var entityValidationErrors in ex.EntityValidationErrors)
                            {
                                foreach (var validationError in entityValidationErrors.ValidationErrors)
                                {
                                    return Ok(" Error: " + validationError.ErrorMessage);
                                }
                            }
                        }


                        var query = (from a in db.FlintFoxPVPMax
                                     where a.IdProd == _response.Registrationid
                                     select a).FirstOrDefault();
                        do
                        {
                            if (_response.APPriceDetails[i].DescriptionError == "") { 
                                if (_response.APPriceDetails[i].DescriptionCod == 1)
                                {
                                    query.NumAcuerdoCosto = _response.APPriceDetails[i].TradeAgreement;
                                    query.CodDescripcionCosto = _response.APPriceDetails[i].DescriptionCod;
                                    query.DescripcionCosto = _response.APPriceDetails[i].Description;
                                    query.FechaInicioCosto = _response.APPriceDetails[i].StartDate;
                                    query.FechaFinCosto = _response.APPriceDetails[i].EndDate;
                                    query.ValorTotalCosto = _response.APPriceDetails[i].TotalValue;
                                    query.PorcentajeCosto = _response.APPriceDetails[i].Percent;
                                }

                                if (_response.APPriceDetails[i].DescriptionCod == 2)
                                {
                                    query.NumAcuerdoIndCom = _response.APPriceDetails[i].TradeAgreement;
                                    query.CodDescripcionIndAlc = _response.APPriceDetails[i].DescriptionCod;
                                    query.DescripcionIndCom = _response.APPriceDetails[i].Description;
                                    query.FechaInicioIndCom = _response.APPriceDetails[i].StartDate; 
                                    query.FechaFinIndCom = _response.APPriceDetails[i].EndDate;
                                    query.ValorTotalIndCom = _response.APPriceDetails[i].TotalValue;
                                    query.PorcentajeIndCom = _response.APPriceDetails[i].Percent;
                                }

                                if (_response.APPriceDetails[i].DescriptionCod == 3)
                                {
                                    query.NumAcuerdoRecAntPO = _response.APPriceDetails[i].TradeAgreement;
                                    query.CodDescripcionRecAntPO = _response.APPriceDetails[i].DescriptionCod;
                                    query.DescripcionRecAntPO = _response.APPriceDetails[i].Description;
                                    query.FechaInicioRecAntPO = _response.APPriceDetails[i].StartDate;
                                    query.FechaFinRecAntPO = _response.APPriceDetails[i].EndDate;
                                    query.ValorTotalRecAntPO = _response.APPriceDetails[i].TotalValue;
                                    query.PorcentajeRecAntPO = _response.APPriceDetails[i].Percent;
                                }

                                if (_response.APPriceDetails[i].DescriptionCod == 4)
                                {
                                    query.NumAcuerdoPO = _response.APPriceDetails[i].TradeAgreement;
                                    query.CodDescripcionPO = _response.APPriceDetails[i].DescriptionCod;
                                    query.DescripcionPO = _response.APPriceDetails[i].Description;
                                    query.FechaInicioPO = _response.APPriceDetails[i].StartDate;
                                    query.FechaFinPO = _response.APPriceDetails[i].EndDate;
                                    query.ValorTotalPO = _response.APPriceDetails[i].TotalValue;
                                    query.PorcentajePO = _response.APPriceDetails[i].Percent;
                                }

                                if (_response.APPriceDetails[i].DescriptionCod == 5)
                                {
                                    query.NumAcuerdoIndFinM = _response.APPriceDetails[i].TradeAgreement;
                                    query.CodDescripcionIndFinM = _response.APPriceDetails[i].DescriptionCod;
                                    query.DescripcionIndFinM = _response.APPriceDetails[i].Description;
                                    query.FechaInicioIndFinM = _response.APPriceDetails[i].StartDate;
                                    query.FechaFinIndFinM = _response.APPriceDetails[i].EndDate;
                                    query.ValorTotalIndFinM = _response.APPriceDetails[i].TotalValue;
                                    query.PorcentajeIndFinM = _response.APPriceDetails[i].Percent;
                                }

                                if (_response.APPriceDetails[i].DescriptionCod == 6)
                                {
                                    query.NumAcuerdoPVPAAlc = _response.APPriceDetails[i].TradeAgreement;
                                    query.CodDescripcionPVPAAlc = _response.APPriceDetails[i].DescriptionCod;
                                    query.DescripcionPVPAAlc = _response.APPriceDetails[i].Description;
                                    query.FechaInicioPVPAAlc = _response.APPriceDetails[i].StartDate;
                                    query.FechaFinPVPAAlc = _response.APPriceDetails[i].EndDate;
                                    query.ValorTotalPVPAAlc = _response.APPriceDetails[i].TotalValue;
                                    query.PorcentajePVPAAlc = _response.APPriceDetails[i].Percent;
                                }

                                if (_response.APPriceDetails[i].DescriptionCod == 7)
                                {
                                    query.NumAcuerdoIndAlc = _response.APPriceDetails[i].TradeAgreement;
                                    query.CodDescripcionIndAlc = _response.APPriceDetails[i].DescriptionCod;
                                    query.DescripcionIndAlc = _response.APPriceDetails[i].Description;
                                    query.FechaInicioIndAlc = _response.APPriceDetails[i].StartDate;
                                    query.FechaFinIndAlc = _response.APPriceDetails[i].EndDate;
                                    query.ValorTotalIndAlc = _response.APPriceDetails[i].TotalValue;
                                    query.PorcentajeIndAlc = _response.APPriceDetails[i].Percent;
                                }

                                if (_response.APPriceDetails[i].DescriptionCod == 8)
                                {
                                    query.NumAcuerdoPVPMax = _response.APPriceDetails[i].TradeAgreement;
                                    query.CodDescripcionPVPMax = _response.APPriceDetails[i].DescriptionCod;
                                    query.DescripcionPVPMax = _response.APPriceDetails[i].Description;
                                    query.FechaInicioPVPMax = _response.APPriceDetails[i].StartDate;
                                    query.FechaFinPVPMax = _response.APPriceDetails[i].EndDate;
                                    query.ValorTotalPVPMax = _response.APPriceDetails[i].TotalValue;
                                    query.PorcentajePVPMax = _response.APPriceDetails[i].Percent;
                                }

                                if (_response.APPriceDetails[i].DescriptionCod == 9)
                                {
                                    query.NumAcuerdoDescPP = _response.APPriceDetails[i].TradeAgreement;
                                    query.CodDescripcionDescPP = _response.APPriceDetails[i].DescriptionCod;
                                    query.DescripcionDescPPx = _response.APPriceDetails[i].Description;
                                    query.FechaInicioDescPP = _response.APPriceDetails[i].StartDate;
                                    query.FechaFinDescPP = _response.APPriceDetails[i].EndDate;
                                    query.ValorTotalDescPP = _response.APPriceDetails[i].TotalValue;
                                    query.PorcentajeDescPP = _response.APPriceDetails[i].Percent;
                                }

                                if (_response.APPriceDetails[i].DescriptionCod == 10)
                                {
                                    query.NumAcuerdoDesWeb = _response.APPriceDetails[i].TradeAgreement;
                                    query.CodDescripcionDesWeb = _response.APPriceDetails[i].DescriptionCod;
                                    query.DescripcionDesWeb = _response.APPriceDetails[i].Description;
                                    query.FechaInicioDesWeb = _response.APPriceDetails[i].StartDate;
                                    query.FechaFinDesWeb = _response.APPriceDetails[i].EndDate;
                                    query.ValorTotalDesWeb = _response.APPriceDetails[i].TotalValue;
                                    query.PorcentajeDesWeb = _response.APPriceDetails[i].Percent;
                                }

                                if (_response.APPriceDetails[i].DescriptionCod == 11)
                                {
                                    query.NumAcuerdoPVP = _response.APPriceDetails[i].TradeAgreement;
                                    query.CodDescripcionPVP = _response.APPriceDetails[i].DescriptionCod;
                                    query.DescripcionPVP = _response.APPriceDetails[i].Description;
                                    query.FechaInicioPVP = _response.APPriceDetails[i].StartDate;
                                    query.FechaFinPVP = _response.APPriceDetails[i].EndDate;
                                    query.ValorTotalPVP = _response.APPriceDetails[i].TotalValue;
                                    query.PorcentajePVP = _response.APPriceDetails[i].Percent;
                                }

                                if (_response.APPriceDetails[i].DescriptionCod == 12)
                                {
                                    query.NumAcuerdoCuotaIni = _response.APPriceDetails[i].TradeAgreement;
                                    query.CodDescripcionCuotaIni = _response.APPriceDetails[i].DescriptionCod;
                                    query.DescripcionCuotaIni = _response.APPriceDetails[i].Description;
                                    query.FechaInicioCuotaIni = _response.APPriceDetails[i].StartDate;
                                    query.FechaFinCuotaIni = _response.APPriceDetails[i].EndDate;
                                    query.ValorTotalCuotaIni = _response.APPriceDetails[i].TotalValue;
                                    query.PorcentajeCuotaIni = _response.APPriceDetails[i].Percent;
                                }
                            }
                            else
                            {
                                query.CodErrorCosto = "0";
                                query.DescripcionErrorCosto = _response.APPriceDetails[i].DescriptionError;
                            }
                            i++;
                        } while (i < cantidad);

                        query.FechaActualizacion = DateTime.Now;


                        foreach (IPAddress ipaddress in ip.AddressList)
                        {
                            query.IpActualizacion = ipaddress.ToString();

                        }
                                             
                        query.UsuarioActualizacion = IDdeUsuario;
                        db.SaveChanges();
                    }
                    respuesta = new ResponseWS();
                    respuesta.ErrorList = new List<string>();
                    respuesta.DescripcionId = "OK";
                    respuesta.StatusCode = 1;
                    respuesta.Response = "OK";
                }
                else
                {
                    respuesta = new ResponseWS();
                    respuesta.ErrorList = new List<string>();
                    respuesta.ErrorList.Add("IVTA018WS: APPriceDetails no tiene datos");
                    respuesta.DescripcionId = "";
                    respuesta.StatusCode = 0;
                    respuesta.Response = "";
                }

                return Ok(respuesta);
            }
            catch (Exception ex)
            {
                respuesta = new ResponseWS();
                respuesta.ErrorList = new List<string>();
                respuesta.ErrorList.Add("ERRORWS: " + ex);
                respuesta.DescripcionId = "ERROR";
                respuesta.StatusCode = 0;
                respuesta.Response = "ERROR";

                return Ok(respuesta);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FlintFoxPVPMaxExists(string id)
        {
            return db.FlintFoxPVPMax.Count(e => e.IdProd == id) > 0;
        }

       
        protected string retornarDataAreaId(string id)
        {
            client = new RestClient(sbUriHomologacionDynamic);
            string finalPath = $"{sbMetodoWsUriAx}/{id}";
            var request = new RestRequest(finalPath, Method.GET);

            var response = client.Execute(request);
            string respuesta="";

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var content = response.Content;
                ResponseWS responsews = JsonConvert.DeserializeObject<ResponseWS>(content);
                respuesta = responsews.Response;             
            }            
            return respuesta;
        }
    }
}