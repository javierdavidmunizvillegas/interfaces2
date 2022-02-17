using ApiModels;
using Newtonsoft.Json;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Xml;
using System.Configuration;
using RestSharp;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;

namespace ApiFlujoCaja.Controllers
{
    public class FlujoController : ApiController
    {
        [HttpPost]
        [Route("api/iniciaflujo")]
        public Base IniciaFlujoC([FromBody] FlujoRequest datos)
        {
            Base Retorno = new Base();
            Retorno = EjecutaProcesoFacturacion(datos);
            return Retorno;

            /*
            //try
            //{
                
            //            ////var resGeneracionComisiones = GeneracionComisiones(datos.SalesID, datos.SalesIDSiac, es);
            //            ////if (resGeneracionComisiones.statusCode != 201)
            //            ////{
            //            //    //resp.response = false;
            //            //    //resp.messages = "ERROR - " + resGeneracionComisiones.errorList.FirstOrDefault() + " - [RESPUESTA: GeneracionComisiones]";
            //            //    //resp.Results = resicob001.ErrorList;
            //            //    //return resp;
            //            ////}


            //            //ExisteCreditoFacilito = bEncuentraMedioPago(datos.SalesIDSiac, datos.NumeroRecibo, "CF");
            //            //ExisteBilleteComprador = bEncuentraMedioPago(datos.SalesIDSiac, datos.NumeroRecibo, "BC");
            //            //if (ExisteCreditoFacilito)
            //            //{
            //            //    #region "Generacion Cartera - Aplica solo cuando el medio de pago es Crédito Facilito"
            //            //    var resGencartera = GeneracionCartera(datos.SalesID, datos.SalesIDSiac, datos.CodigoAlmacen, datos.CodigoCliente, datos.UsuarioIngreso, datos.TerminalIngreso, datos.Monto, rescaj008);
            //            //    if (resGencartera.StatusCode != "OK")
            //            //    {
            //            //        blResultadoFinal = false;
            //            //    }
            //            //    #endregion
            //            //}
            //            //else
            //            //{
            //            //    if (IsPedidoAsistencia)
            //            //    {
            //            //        #region "Asistencia Facilita - Aplica para  todas las formas de pago excepto Crédito Facilito"
            //            //        //var resAsis = AsistenciaFacilita(datos.CodigoAlmacen, datosFactura, rescaj008);
            //            //        //if (resAsis.codigoTransaccion != "200")
            //            //        //{
            //            //        //    //resp.response = false;
            //            //        //    //resp.messages = "ERROR - " + resAsis.descripcionTransaccion + " - [RESPUESTA: ASISTENCIA FACILITA]";
            //            //        //    //resp.Results.Add(resAsis.descripcionTransaccion);
            //            //        //    //return resp;
            //            //        //}
            //            //        #endregion
            //            //    }
            //            //}

            //            //if (ExisteBilleteComprador)
            //            //{
            //            //    #region "Billete Comprador - Aplica solo cuando el medio de pago es billete comprador / falta validar que no estoy usando asistencia"
            //            //    var resBilleteComprador = ServicioActualizaBilleteComprador(datos.SalesID, datos.SalesIDSiac, datos.TipoTransaccion.ToString(), datos.OrigenTransaccion);
            //            //    if (resBilleteComprador.response == false)
            //            //    {
            //            //        blResultadoFinal = false;
            //            //    }
            //            //    #endregion
            //            //}
            //            //DataTable DtFacturaMoto = null;
            //            //if (bValidaFacturaMoto(rescaj008, ref DtFacturaMoto))
            //            //{
            //            //    #region "Servicio GTM - Se llama solo cuando es una factura por moto"
            //            //    var resGTM = RegistroFacturaMoto(datos.SalesID, datos.SalesIDSiac, datos.TipoTransaccion.ToString(), datos.OrigenTransaccion, DtFacturaMoto);
            //            //    if (resGTM.statusCode != 200)
            //            //    {
            //            //        blResultadoFinal = false;
            //            //    }
            //            //    #endregion
            //            //}

            //            ////    //#region "Servicio de Asistencia Facilita Multinova"
            //            ////    //var resConsultaAFMultinova = ConsultaServicioAsistenciaMultiNova(datos.SalesID, datos.SalesIDSiac);
            //            ////    //if (resConsultaAFMultinova.response == false)
            //            ////    //{
            //            ////    //    resp.response = false;
            //            ////    //    resp.messages = "ERROR - " + resConsultaAFMultinova.messages + " - [RESPUESTA: CONSULTA_AF_MULTINOVA]";
            //            ////    //    resp.Results = new List<string> { resConsultaAFMultinova.messages };
            //            ////    //    return resp;
            //            ////    //}
            //            ////    //else
            //            ////    //{
            //            ////    //    var resConfirmaAFMultinova = ConfirmaServicioAsistenciaMultiNova (datos.SalesID, datos.SalesIDSiac, datos.UsuarioIngreso, resConsultaAFMultinova, rescaj008);
            //            ////    //    if (resConfirmaAFMultinova.response == false)
            //            ////    //    {
            //            ////    //        resp.response = false;
            //            ////    //        resp.messages = "ERROR - " + resConfirmaAFMultinova.messages + " - [RESPUESTA: CONFIRMA_AF_MULTINOVA]";
            //            ////    //        resp.Results = new List<string> { resConfirmaAFMultinova.messages };
            //            ////    //        return resp;
            //            ////    //    }
            //            ////    //}
            //            ////    //#endregion
            //            //#endregion
             
            //        //#region "Servicio de generacion de billete comprador"

            //        //#endregion

            //        //#region "Servicio de bonos"



            //        //foreach (var item in rescaj008.documentInvoiceRequestTableList)
            //        //{
            //        //    foreach (var line in item.documentInvoiceRequestLinesList)
            //        //    {
            //        //        if(resp.Results == null)
            //        //        {
            //        //            resp.Results = new List<string>();
            //        //        }
            //        //        resp.Results.Add(line.InvoiceId);
            //        //    }
            //        //}

            //}
            //catch (Exception ex)
            //{
            //    Retorno.Estado = blResultadoFinal;
            //    Retorno.CodigoError = 999;
            //    Retorno.Mensaje = ex.Message;
            //    return Retorno;
            //}
            */
        }
        
   /*
        //private ICRE004Response Icre004(string codigoempresa, string Pedido)
        //{
        //    ICRE004Response res = new ICRE004Response();
        //    try
        //    {
        //        string Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlICRE004"]; //"http://192.168.82.43:83/ICRE004/APICRE004001";

        //        List<int> dcolist = new List<int>();
        //        dcolist.Add(0);
        //        List<int> statuslist = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 12, 13 };
        //        ICRE004Request obj = new ICRE004Request()
        //        {
        //            DataAreaId = codigoempresa,
        //            Enviroment = "SIT",
        //            APPaymModeGeneral = "",
        //            CustAccountList = new List<string>(),
        //            DateStart = DateTime.Now.AddDays(-15),
        //            DateEnd = DateTime.Now,
        //            DocumentStatusList = dcolist,
        //            PaymentRequest = true,
        //            PurchOrderFormNumList = new List<string> { Pedido },
        //            SalesIdList = new List<string>(),
        //            SalesResponsiblePersonnalNumberList = new List<string>(),
        //            SalesStatusList = statuslist
        //        };

        //        var jsonreq = JsonConvert.SerializeObject(obj);

        //        using (var client = new HttpClient())
        //        {
        //            client.CancelPendingRequests();

        //            client.DefaultRequestHeaders.Clear();
        //            client.BaseAddress = new Uri(Baseurl);
        //            //client.Timeout = TimeSpan.FromSeconds(25);
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            var postTask = client.PostAsJsonAsync<ICRE004Request>("APICRE004001", obj);
        //            try
        //            {
        //                postTask.Wait();
        //            }
        //            catch (Exception ex)
        //            {
        //                res.response = false;
        //                res.messages = ex.Message;
        //                return res;
        //            }

        //            var result = postTask.Result;
        //            if (result.IsSuccessStatusCode)
        //            {
        //                var readTask = result.Content.ReadAsStringAsync().Result;

        //                res = JsonConvert.DeserializeObject<ICRE004Response>(readTask);
        //                res.response = true;
        //                res.messages = "OK";


        //                return res;
        //            }

        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw;
        //    }
            */


        /*
        //   private GeneracionCarteraResponse GeneracionCartera(string salesid, string salessiac, string storesiac, string codigocliente, string usuario, string terminal, decimal valor, ICAJ008Response iCAJ008)
        //{
        //    GeneracionCarteraResponse res = new GeneracionCarteraResponse();
        //    string Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlGeneracionCartera"]; //"http://192.168.82.43:83/FacturacionDespacho/api/Cartera/GenerarCartera";

        //    List<CarteraDetalleVM> carteraDetalles = new List<CarteraDetalleVM>();
            
        //    foreach (var tables in iCAJ008.documentInvoiceRequestTableList)
        //    {
                
        //        foreach (var lines in tables.documentInvoiceRequestLinesList)
        //        {
        //            List<CarteraDetalleProductos> detalleProductos = new List<CarteraDetalleProductos>();
        //            foreach (var items in lines.itemList)
        //            {
        //                detalleProductos.Add(new CarteraDetalleProductos
        //                {
        //                    ItemId = items.itemId,
        //                    AmountLine = items.amountLine,
        //                    OrdenItems = items.ordenItems,
        //                    Qty = items.qty
        //                });
        //            }
        //            carteraDetalles.Add(new CarteraDetalleVM
        //            {
        //                InvoiceDate = lines.InvoiceDate,
        //                InvoiceId = lines.InvoiceId,
        //                SecuenciaFacturacion = lines.secuenciaFacturacion,
        //                TotalAmount = lines.TotalAmount,
        //                Voucher = lines.Voucher,
        //                ItemList = detalleProductos
        //            });
        //        }
        //    }
        //    GeneracionCarteraRequest obj = new GeneracionCarteraRequest
        //    {
        //        CustAccount = codigocliente,
        //        SalesId = salesid,
        //        SalesIdAccount = Convert.ToInt64(salessiac),
        //        Store = storesiac,
        //        PostingProfile = "CXC",
        //        User = usuario,
        //        Terminal = terminal,
        //        TotalAmount = valor,
        //        DocumentInvoiceRequestLinesList = carteraDetalles
        //    };

        //    var jsonstring = JsonConvert.SerializeObject(obj);
        //    var ress = RegistrarTrx(2, salesid, salessiac, "", "", false, false, "", "","",jsonstring);

        //    using (var client = new HttpClient())
        //    {
        //        client.CancelPendingRequests();

        //        client.DefaultRequestHeaders.Clear();
        //        client.BaseAddress = new Uri(Baseurl);
        //        //client.Timeout = TimeSpan.FromSeconds(25);
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var postTask = client.PostAsJsonAsync<GeneracionCarteraRequest>("ConsultaDatosFactura", obj);
        //        try
        //        {
        //            postTask.Wait();
        //        }
        //        catch (Exception ex)
        //        {
        //            res.response = false;
        //            res.messages = ex.Message;
        //            return res;
        //        }

        //        var result = postTask.Result;
        //        if (result.IsSuccessStatusCode)
        //        {
        //            var readTask = result.Content.ReadAsStringAsync().Result;

        //            res = JsonConvert.DeserializeObject<GeneracionCarteraResponse>(readTask);
        //            res.response = true;
        //            res.messages = "OK";
        //            var jsonresp = JsonConvert.SerializeObject(res);
        //            var resss = RegistrarTrx(10, salesid, salessiac, "", "", false, false, "", jsonstring, "", "", "", "", "", "", jsonresp);

        //            return res;
        //        }

        //        return res;
        //    }
        //}

        //private AsistenciaResponse AsistenciaFacilita(string storesiac, ConsultaDatosFacturaResponse itemsFac, ICAJ008Response iCAJ008)
        //{
        //    AsistenciaResponse res = new AsistenciaResponse();
        //    try
        //    {
        //        string Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlAsistenciaFacilita"]; //"http://192.168.82.43:83/AsistenciaFacilitaDynamics/api/Cobro/Registrar";

        //        var a1 = iCAJ008.documentInvoiceRequestTableList.FirstOrDefault();
        //        var a2 = a1.documentInvoiceRequestLinesList.FirstOrDefault();

        //        var item = itemsFac.DetalleFacturacion.Where(x => x.EsAsistenciaFacilita == true || x.EsGarantiaExtendida == true).FirstOrDefault();
        //        var producto = item.Producto.Where(x => x.EsGarantiaExtendida == true).FirstOrDefault();
        //        AsistenciaRequest obj = new AsistenciaRequest()
        //        {
        //            codigoProductoSeguro= producto.CodigoProducto,
        //            numeroFactura= a2.InvoiceId.ToString(),
        //            codigoSucursal= storesiac,
        //            prima=producto.SaldoFinanciar,
        //            fechaPago=DateTime.Now,
        //            numeroCuota=item.NumeroCuota

        //        };

        //        using (var client = new HttpClient())
        //        {
        //            client.CancelPendingRequests();

        //            client.DefaultRequestHeaders.Clear();
        //            client.BaseAddress = new Uri(Baseurl);
        //            //client.Timeout = TimeSpan.FromSeconds(25);
        //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //            var postTask = client.PostAsJsonAsync<AsistenciaRequest>("Registrar", obj);
        //            try
        //            {
        //                postTask.Wait();
        //            }
        //            catch (Exception ex)
        //            {
        //                res.response = false;
        //                res.messages = ex.Message;
        //                return res;
        //            }

        //            var result = postTask.Result;
        //            if (result.IsSuccessStatusCode)
        //            {
        //                var readTask = result.Content.ReadAsStringAsync().Result;

        //                res = JsonConvert.DeserializeObject<AsistenciaResponse>(readTask);
        //                res.response = true;
        //                res.messages = "OK";
                        
        //                return res;
        //            }
        //            else
        //            {
        //                res.response = false;
        //                res.messages = result.ReasonPhrase;
        //            }

        //            return res;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        res.response = false;
        //        res.messages = ex.Message;
        //        return res;
        //    }
        //}
            */



        /*
                #region "Javier Muñiz - Llamado Interfaces"

                private GeneracionComisionesResponse GeneracionComisiones(string salesid, string salessiac, ICAJ008Response iCAJ008)
                {
                    GeneracionComisionesResponse res = new GeneracionComisionesResponse();
                    string Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlGeneracionComisiones"]; //"http://192.168.82.43:82/ComisionesDynamics/api/Comisiones/ActualizarFA";
                    //var jsonstring = JsonConvert.SerializeObject(iCAJ008.documentInvoiceRequestTableList.FirstOrDefault());
                    //var ress = RegistrarTrx(2, salesid, salessiac, "", "", false, false, "", "", "", jsonstring);
                    var jsonstring = "";
                    using (var client = new HttpClient())
                    {
                        client.CancelPendingRequests();

                        client.DefaultRequestHeaders.Clear();
                        client.BaseAddress = new Uri(Baseurl);
                        //client.Timeout = TimeSpan.FromSeconds(25);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var postTask = client.PostAsJsonAsync<ICAJ008Response>("GeneraComisiones", iCAJ008);
                        try
                        {
                            postTask.Wait();
                        }
                        catch (Exception ex)
                        {
                            res.errorList = new List<string> { ex.Message };
                            res.descripcionId = ex.Message;
                            res.statusCode = 900;
                            return res;
                        }

                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsStringAsync().Result;

                            res = JsonConvert.DeserializeObject<GeneracionComisionesResponse>(readTask);
                            //res.response = "true";
                            //res.descripcionId = "OK";
                            var jsonresp = JsonConvert.SerializeObject(res);
                            var resss = RegistrarTrx(10, salesid, salessiac, "", "", false, false, "", jsonstring, "", "", "", "", "", "", jsonresp);

                            return res;
                        }

                        return res;
                    }
                }


                //private ActualizaBilleteCompradorResponse ServicioActualizaBilleteComprador(string salesid, string salessiac, string tipotrx, string origentrx)
                //{
                //    const string Comillas = "\"";
                //    string JsonResponse = string.Empty;
                //    ActualizaBilleteCompradorResponse res = null;
                //    string Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlActualizaBilleteComprador"]; //"http://192.168.82.13:82/BilleteComprador/api/ActualizarBC";
                //    if (!ConsultaLogs(salesid, salessiac, tipotrx, origentrx, "ervicio Billete Comprador", ref JsonResponse))
                //    {
                //        var jsonstring = @"{" + "\n" + @"    ""NumeroPedido"": " + Comillas + salessiac + Comillas + "" + "\n" + @"}";
                //        if (RegistrarLogs(salesid, salessiac, tipotrx, origentrx, "Servicio Billete Comprador", jsonstring, string.Empty, string.Empty, string.Empty))
                //        {
                //            var clienteRest = new RestClient(Baseurl);
                //            clienteRest.Timeout = -1;
                //            var request = new RestRequest(Method.POST);
                //            request.AddHeader("Content-Type", "application/json");

                //            request.AddParameter("application/json", jsonstring, ParameterType.RequestBody);
                //            IRestResponse response = clienteRest.Execute(request);
                //            var content = response.Content;
                //            res = JsonConvert.DeserializeObject<ActualizaBilleteCompradorResponse>(content);
                //            if (res.response )
                //            {
                //                var jsonresp = JsonConvert.SerializeObject(res);
                //                RegistrarLogs(salesid, salessiac, tipotrx, origentrx, "Servicio Billete Comprador", "", jsonresp, string.Empty, "TRUE");
                //            }
                //            else
                //            {
                //                RegistrarLogs(salesid, salessiac, tipotrx, origentrx, "Servicio Billete Comprador", "", res.messages, string.Empty, "TRUE");
                //                res.response = false;
                //                res.messages = res.messages;
                //            }
                //        }
                //        else
                //        {
                //            res = new ActualizaBilleteCompradorResponse();
                //            res.response = false;
                //            res.messages = "No se pudo registrar el evento Servicio Billete Comprador en la tabla de logs";
                //        }
                //    }
                //    else
                //    {
                //        res = JsonConvert.DeserializeObject<ActualizaBilleteCompradorResponse>(JsonResponse);
                //    }
                //  return res;
                //}



                private VTA028ConsultaServicioResponse ConsultaServicioAsistenciaMultiNova(string salesid, string salessiac)
                {
                    VTA028ConsultaServicioResponse res = new VTA028ConsultaServicioResponse();
                    string Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlConsultaServicioMultinova"];

                    VTA028ConsultaServicioRequest VTA028 = new VTA028ConsultaServicioRequest()
                    {
                        NumeroPedidoSiac =  Convert.ToInt32(salessiac)
                    };

                    var jsonstring = JsonConvert.SerializeObject(VTA028);
                    var ress = RegistrarTrx(2, salesid, salessiac, "", "", false, false, "", "", "", jsonstring);


                    using (var client = new HttpClient())
                    {
                        client.CancelPendingRequests();
                        client.DefaultRequestHeaders.Clear();
                        client.BaseAddress = new Uri(Baseurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        var postTask = client.PostAsJsonAsync<VTA028ConsultaServicioRequest>("GeneraComisiones", VTA028);
                        try
                        {
                            postTask.Wait();
                        }
                        catch (Exception ex)
                        {
                            res.Estado = false;
                            res.Mensaje = ex.Message;
                            return res;
                        }
                        var result = postTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsStringAsync().Result;
                            res = JsonConvert.DeserializeObject<VTA028ConsultaServicioResponse>(readTask);
                            var jsonresp = JsonConvert.SerializeObject(res);
                            var resss = RegistrarTrx(10, salesid, salessiac, "", "", false, false, "", jsonstring, "", "", "", "", "", "", jsonresp);
                            return res;
                        }
                        return res;
                    }
                }


                #endregion 

                #region "Javier Muñiz - Validaciones"


                //private bool bEncuentraMedioPago(string PEDIDOSIAC, string NUMERORECIBO, string Tipo)
                //{
                //    bool retorno = false;
                //    string strMedioPago = string.Empty;
                //    int CodMedioPago = 0;
                //    DataTable DtFormaPago = null;
                //    try
                //    {
                //        DtFormaPago = ConsultaFormaPagoPedidos(PEDIDOSIAC, NUMERORECIBO);
                //        switch (Tipo)
                //        {
                //            case "CF":
                //                strMedioPago = "99 - CREDITO FACILITO";
                //                CodMedioPago = 36;
                //                break;
                //            case "BC":
                //                strMedioPago = "08 - BILLETES";
                //                CodMedioPago = 33;
                //                break;
                //        }
                //        foreach (DataRow Dr in DtFormaPago.Rows)
                //        {
                //            if (Convert.ToInt32(Dr["CCODIGOFORMAPAGO"]) == CodMedioPago)
                //            {
                //                retorno = true;
                //                break;
                //            }
                //        }
                //    }
                //    catch (Exception Ex) { }
                //    return retorno;
                //}

                //private bool bValidaFacturaMoto(ICAJ008Response iCAJ008,  ref DataTable DTable)
                //{
                //    DataTable Dt = new DataTable();
                //    DataRow Dr;
                //    bool Retorno = false;
                //    try
                //    {
                //        Dt.Columns.Add("FACTURA");
                //        Dt.Columns.Add("FECHA");
                //        Dt.Columns.Add("HORA");
                //        Dt.Columns.Add("SECUENCIAEMPRESA");
                //        Dt.Columns.Add("SECUENCIACAJA");
                //        Dt.Columns.Add("SECUENCIAFACTURA");
                //        Dt.Columns.Add("ITEM");
                //        Dt.Columns.Add("DESCRIPCION");
                //        Dt.Columns.Add("SERIE");
                //        foreach (var ListaPedido in iCAJ008.documentInvoiceRequestTableList)
                //        {
                //            var datosFactura = ConsultaDatosFactura(ListaPedido.SalesId, ListaPedido.SalesIdAccount);
                //            var itemsFactura = GetItems(ListaPedido.SalesIdAccount);
                //            foreach (var ListaFactura in ListaPedido.documentInvoiceRequestLinesList)
                //            {
                //                foreach (var ListaItemsFactura in ListaFactura.itemList)
                //                {
                //                    foreach (var objDetalleFacturacion in datosFactura.DetalleFacturacion)
                //                    {
                //                        foreach (var objProductoFactura in objDetalleFacturacion.Producto)
                //                        {
                //                            if(ListaItemsFactura.itemId == objProductoFactura.CodigoProducto)
                //                            {
                //                                if (objProductoFactura.ProductoMoto)
                //                                {
                //                                    Dr = Dt.NewRow();
                //                                    Dr["FACTURA"] = ListaFactura.InvoiceId;
                //                                    Dr["FECHA"] = ListaFactura.InvoiceDate.ToShortDateString();
                //                                    Dr["HORA"] = ListaFactura.InvoiceDate.ToShortTimeString();
                //                                    Dr["SECUENCIAEMPRESA"] = ListaFactura.InvoiceId.Substring(0, 3);
                //                                    Dr["SECUENCIACAJA"] = ListaFactura.InvoiceId.Substring(4, 3);
                //                                    Dr["SECUENCIAFACTURA"] = ListaFactura.InvoiceId.Substring(8);
                //                                    Dr["ITEM"] = ListaItemsFactura.itemId;
                //                                    Dr["DESCRIPCION"] = objProductoFactura.descripcionProducto;
                //                                    Dr["SERIE"] = objProductoFactura.Serie;
                //                                    Dt.Rows.Add(Dr);
                //                                    DTable = Dt;
                //                                    Retorno = true;
                //                                    break;
                //                                }
                //                            }
                //                        }
                //                    }
                //                }

                //            }
                //        }
                //    }
                //    catch(Exception Ex) { }
                //    return Retorno;
                //}
                #endregion



                #region "Consultas SQL"
                private string RegistrarTrx(int tipo, string salesid, string salessiac, string tipotrx, string origentrx, bool asistencia, bool moto, string jreqconfpedido = "", string jreqicaj008 = "", string jreqicob001 = "", string jreqgencartera = "", string jreqGTM = "", string jreqBC = "", string jreqasistencia = "", string jreqbono = "", string jrespuesta = "")
                {
                    string strText = string.Empty;
                    XmlDocument xmlDoc = new XmlDocument();
                    SQLConexion ObjCon = new SQLConexion();
                    SqlCommand cmd = new SqlCommand();
                    try
                    {
                        DataObject dobj = new DataObject();
                        dobj.SW_ProductoName = "Desarrollo";
                        dobj.SW_SPName = "dbo.SP_CAJREGISTRO_FLUJO";
                        dobj.SW_Base = "SIAC";
                        dobj.SW_TimeOut = 0;

                        List<DataParameters> par = new List<DataParameters>();

                        par.Add(new DataParameters { SW_Parametro = "@tipo", SW_Value = tipo, SW_Size = 15, SW_Type = DataParameters.SWDataType.Int });
                        par.Add(new DataParameters { SW_Parametro = "@Salesid", SW_Value = salesid, SW_Size = 50, SW_Type = DataParameters.SWDataType.String });
                        par.Add(new DataParameters { SW_Parametro = "@Salessiac", SW_Value = salessiac, SW_Size = 50, SW_Type = DataParameters.SWDataType.String });
                        par.Add(new DataParameters { SW_Parametro = "@Tipotrx", SW_Value = tipotrx, SW_Size = 50, SW_Type = DataParameters.SWDataType.String });
                        par.Add(new DataParameters { SW_Parametro = "@Origentrx", SW_Value = origentrx, SW_Size = 50, SW_Type = DataParameters.SWDataType.String });
                        if (asistencia)
                        {
                            par.Add(new DataParameters { SW_Parametro = "@TieneAsistencia", SW_Value = 1, SW_Size = 50, SW_Type = DataParameters.SWDataType.Int });
                        }
                        else
                        {
                            par.Add(new DataParameters { SW_Parametro = "@TieneAsistencia", SW_Value = 0, SW_Size = 50, SW_Type = DataParameters.SWDataType.Int });

                        }
                        if (moto)
                        {
                            par.Add(new DataParameters { SW_Parametro = "@TieneMoto", SW_Value = 1, SW_Size = 50, SW_Type = DataParameters.SWDataType.Int });
                        }
                        else
                        {
                            par.Add(new DataParameters { SW_Parametro = "@TieneMoto", SW_Value = 0, SW_Size = 50, SW_Type = DataParameters.SWDataType.Int });
                        }

                        par.Add(new DataParameters { SW_Parametro = "@ReqConfirmaPedido", SW_Value = jreqconfpedido, SW_Size = 50000, SW_Type = DataParameters.SWDataType.String });
                        par.Add(new DataParameters { SW_Parametro = "@ReqIcaj008", SW_Value = jreqicaj008, SW_Size = 50000, SW_Type = DataParameters.SWDataType.String });
                        par.Add(new DataParameters { SW_Parametro = "@ReqIcob001", SW_Value = jreqicob001, SW_Size = 50000, SW_Type = DataParameters.SWDataType.String });
                        par.Add(new DataParameters { SW_Parametro = "@ReqGeneraCartera", SW_Value = jreqgencartera, SW_Size = 50000, SW_Type = DataParameters.SWDataType.String });
                        par.Add(new DataParameters { SW_Parametro = "@ReqGTM", SW_Value = jreqGTM, SW_Size = 50000, SW_Type = DataParameters.SWDataType.String });
                        par.Add(new DataParameters { SW_Parametro = "@ReqBC", SW_Value = jreqBC, SW_Size = 50000, SW_Type = DataParameters.SWDataType.String });
                        par.Add(new DataParameters { SW_Parametro = "@ReqAsistencia", SW_Value = jreqasistencia, SW_Size = 50000, SW_Type = DataParameters.SWDataType.String });
                        par.Add(new DataParameters { SW_Parametro = "@ReqBono", SW_Value = jreqbono, SW_Size = 50000, SW_Type = DataParameters.SWDataType.String });
                        par.Add(new DataParameters { SW_Parametro = "@RespJson", SW_Value = jrespuesta, SW_Size = 50000, SW_Type = DataParameters.SWDataType.String });

                        dobj.SW_Parameters = par;
                        DataSet ds = new DataSet();
                        ds = ObjCon.Execute(dobj);

                        if (ds != null)
                        {
                            xmlDoc.LoadXml(ds.GetXml());
                        }
                        else
                        {
                            strText = "";
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (ObjCon.EstadoConexion()) { ObjCon.Close(); }
                        if (cmd != null) { cmd.Connection = null; }
                        cmd = null;

                    }

                    return strText = xmlDoc.OuterXml;
                }


                private DataTable ConsultaPedidosSIAC(object PEDIDOSIAC)
                {
                    DataTable Dt = null;
                    XmlDocument xmlDoc = new XmlDocument();
                    SQLConexion ObjCon = new SQLConexion();
                    SqlCommand cmd = new SqlCommand();
                    try
                    {

                        DataObject dobj = new DataObject();
                        dobj.SW_ProductoName = "Desarrollo";
                        dobj.SW_SPName = "dbo.PROC_CONSULTAINFOPEDIDOS";
                        dobj.SW_Base = "SIAC";
                        dobj.SW_TimeOut = 0;

                        List<DataParameters> par = new List<DataParameters>();

                        par.Add(new DataParameters { SW_Parametro = "@PEDIDOSIAC", SW_Value = PEDIDOSIAC, SW_Size = 9, SW_Type = DataParameters.SWDataType.Decimal });
                        dobj.SW_Parameters = par;
                        DataSet ds = new DataSet();
                        ds = ObjCon.Execute(dobj);

                        if (ds != null)
                        {
                            if (ds.Tables.Count > 0)
                            {
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    Dt = ds.Tables[0];
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {
                        if (ObjCon.EstadoConexion()) { ObjCon.Close(); }
                        if (cmd != null) { cmd.Connection = null; }
                        cmd = null;

                    }
                    return Dt;
                }




                public string ConsultaCajFLujo2(int tipo, string salesid)
                {
                    SqlConnection SqlConn = new SqlConnection();
                    SqlCommand SqlCmd;
                    SqlDataAdapter SqlAdap;
                    DataSet ds = new DataSet();
                    string strText = string.Empty;
                    XmlDocument xmlDoc = new XmlDocument();
                    try
                    {
                        SqlConn.ConnectionString = "Data Source=192.168.82.13;Initial Catalog=SIAC;User ID=AppCrecob;Password=Appcrecob;Connection Timeout=0";
                        SqlConn.Open();
                        SqlCmd = new SqlCommand("SP_CAJREGISTRO_FLUJO");
                        SqlCmd.CommandType = CommandType.StoredProcedure;
                        SqlCmd.Connection = SqlConn;
                        SqlCmd.Parameters.Add("@tipo", SqlDbType.Decimal, 4).Value = tipo;
                        SqlCmd.Parameters.Add("@Salessiac", SqlDbType.VarChar, 20).Value = salesid;
                        SqlAdap = new SqlDataAdapter(SqlCmd);
                        SqlAdap.Fill(ds);
                        SqlConn.Close();
                        if (ds != null)
                        {
                            xmlDoc.LoadXml(ds.GetXml());
                            strText = xmlDoc.OuterXml;
                        }
                        else
                        {
                            strText = "";
                        }

                    }
                    catch (Exception Ex)
                    {
                        SqlCmd = null;
                        SqlConn = null;
                    }
                    finally
                    {
                        SqlCmd = null;
                        SqlConn = null;
                    }
                    return strText;
                }
                #endregion 
                */


        private Base EjecutaProcesoFacturacion(FlujoRequest datos)
        {
            Base Retorno = new Base();
            bool blResultadoFinal = true;
            clsSIAC ObjSIAC = new clsSIAC();
            clsConfirmacionPedidos objConfirmacionPedidos = new clsConfirmacionPedidos();
            clsICAJ008 ObjICAJ008 = new clsICAJ008();
            string JsonResponse = string.Empty;
            try
            {
                if (!ObjSIAC.ConsultaLogs(datos, "Inicia Flujo", ref JsonResponse))
                {
                    ObjSIAC.RegistrarLogs(datos, "Inicia Flujo", JsonConvert.SerializeObject(datos), string.Empty, string.Empty, "FALSE");
                    
                    var resc = objConfirmacionPedidos.ConfirmarPedido(datos);
                    if (resc.CodigoMensaje != "200")
                    {
                        Retorno.CodigoError = 100;
                        Retorno.Mensaje = "Error en la confirmación del pedido.";
                        Retorno.Estado = false;
                        return Retorno;
                    }

                    var respICAJ008 = ObjICAJ008.ICAJ008(datos);
                    if (respICAJ008.statusId == false)
                    {
                        Retorno.CodigoError = 200;
                        Retorno.Mensaje = "Error en la facturación del pedido.";
                        Retorno.Estado = true;
                        return Retorno;
                    }


                    if (respICAJ008.documentInvoiceRequestTableList.Count > 0)
                    {
                        Retorno = EjecutaProcesoLiquidacion(datos, respICAJ008);
                    }
                    else
                    {
                        Retorno.Estado = blResultadoFinal;
                        Retorno.CodigoError = 0;
                        Retorno.Mensaje = "Proceso finalizado con exito.";
                        ObjSIAC.RegistrarLogs(datos, "Inicia Flujo", JsonConvert.SerializeObject(datos), JsonConvert.SerializeObject(Retorno), string.Empty, blResultadoFinal.ToString().ToUpper());
                    }
                }
            }
            catch(Exception Ex)
            {
                Retorno.CodigoError = 90;
                Retorno.Mensaje = string.Concat("EjecutaProcesoFacturacion: ", Ex.Message);
                Retorno.Estado = false;
            }
            return Retorno;
        }
        private Base EjecutaProcesoLiquidacion(FlujoRequest datos, ICAJ008Response RespCAJ008)
        {
            Base Retorno = new Base();
            bool blResultadoFinal = true;
            string JsonResponse = string.Empty;
            
            clsSIAC ObjSIAC = new clsSIAC();
            clsICOB001 ObjICB001 = new clsICOB001();
            clsGTM004  ObjGTM004 = new clsGTM004();
            try
            {
                if (!ObjSIAC.ConsultaLogs(datos, "Inicia Flujo", ref JsonResponse))
                {
                    if(datos.OrigenTransaccion != "CAJA")
                        ObjSIAC.RegistrarLogs(datos, "Inicia Flujo", JsonConvert.SerializeObject(datos), string.Empty, string.Empty, "FALSE");

                    var RespICOB001 = ObjICB001.ICOB001(datos, RespCAJ008);
                    if (RespICOB001.StatusId == false)
                    {
                        Retorno.CodigoError = 300;
                        Retorno.Mensaje = "Error en la liquidación de los asientos.";
                        Retorno.Estado = false;
                        return Retorno;
                    }

                    #region "Llamados de servicios secundarios"
                    var RespIGTM004 = ObjGTM004.IGTM004(datos, RespCAJ008);
                    if (RespICOB001.StatusId == false)
                    {
                        Retorno.CodigoError = 400;
                        Retorno.Mensaje = "Error en la notificación de facturación de Moto.";
                        Retorno.Estado = false;
                        blResultadoFinal = false;
                    }
                    #endregion

                    Retorno.Estado = blResultadoFinal;
                    Retorno.CodigoError = 0;
                    Retorno.Mensaje = "Proceso finalizado con exito.";
                    ObjSIAC.RegistrarLogs(datos, "Inicia Flujo", JsonConvert.SerializeObject(datos), JsonConvert.SerializeObject(Retorno), string.Empty, blResultadoFinal.ToString().ToUpper());
                }
            }
            catch (Exception Ex)
            {
                Retorno.CodigoError = 90;
                Retorno.Mensaje = string.Concat("EjecutaProcesoLiquidacion: ", Ex.Message);
                Retorno.Estado = false;
            }
            return Retorno;
        }


    }
}