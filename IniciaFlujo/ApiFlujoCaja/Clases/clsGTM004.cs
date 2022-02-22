using ApiModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace ApiFlujoCaja
{
    public class clsGTM004:clsSIAC
    {

        public GTM004Response IGTM004(FlujoRequest Datos, ICAJ008Response iCAJ008)
        {
            DataTable DtFacturaMoto = null;
            GTM004Response res = new GTM004Response();
            if (bValidaFacturaMoto(iCAJ008, ref DtFacturaMoto))
            {
                res = RegistroFacturaMoto(Datos, DtFacturaMoto);
            }
            else
            {
                res.Estado = true;
                res.Mensaje = "No existe factura de motos.";
                res.CodigoError = 0;

            }
            return res;
        }
        private GTM004Response RegistroFacturaMoto(FlujoRequest Datos, DataTable DtFactura)
        {
            DataTable DtPedido = null;
            string EstadoEjecucion = "TRUE";
            ICRE009Response ObjetoMoto = null;
            ICRE006Response ObjetoCliente = null;
            string JsonResponse = string.Empty;
            GTM004Request IGTM004 = null;
            GTM004Response res = new GTM004Response();
            clsICRE006 ObjICRE006 = new clsICRE006();
            clsICRE009 ObjICRE009 = new clsICRE009();
            string Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlServicioGTM"];
            if (!ConsultaLogs(Datos, "IGTM004", ref JsonResponse))
            {
                //DtPedido = ConsultaPedidosSIAC(salessiac);  //Validar si esto se registra desde Dynamics
                if (DtPedido.Rows.Count > 0)
                {
                    foreach (DataRow Dr in DtPedido.Rows)
                    {
                        ObjetoCliente = ObjICRE006.ConsultaInfoCliente(Dr["CCODIGOEMPRESA"].ToString(), Dr["CIDENTIFICACION"].ToString());
                        foreach (DataRow Drow in DtFactura.Rows)
                        {
                            ObjetoMoto = ObjICRE009.goConsultaFacturaMoto(Drow["FACTURA"].ToString());
                            if (ObjetoMoto.errorList.Count == 0)
                            {
                                var lcObjetoMoto = ObjetoMoto.APICRE009001InvoiceOrderList[0].salesLineList[0];
                                IGTM004 = new GTM004Request()
                                {
                                    DataAreaId = Dr["CCODIGOEMPRESA"].ToString(),
                                    StoreId = Dr["CCODIGOALMACEN"].ToString(),
                                    InvoiceId = Drow["FACTURA"].ToString(),
                                    Id = Dr["CIDENTIFICACION"].ToString(),
                                    Names = Dr["NOMBRES"].ToString(),
                                    LastName = Dr["APELLIDOS"].ToString(),
                                    ConventionalTelephone = Dr["CTELEFONOCONVENCIONAL"].ToString(),
                                    CellPhone = Dr["CNUMEROCELULAR1"].ToString(),
                                    Email = Dr["CDIRECCIONCORREO"].ToString(),
                                    ItemId = Drow["ITEM"].ToString(),
                                    ItemDescription = Drow["DESCRIPCION"].ToString(),
                                    Serie = Drow["SERIE"].ToString(),
                                    Chassis = lcObjetoMoto.APSalesVehicle.ChassisSeries,
                                    Cpn = lcObjetoMoto.APSalesVehicle.CPNNumber,
                                    InvoiceDate = Convert.ToDateTime(Drow["FECHA"]),
                                    InvoiceHour = Drow["HORA"].ToString(),
                                    Establishment = Drow["SECUENCIAEMPRESA"].ToString(),
                                    EmissionPoint = Drow["SECUENCIACAJA"].ToString(),
                                    Sequential = Drow["SECUENCIAFACTURA"].ToString(),
                                    PaymentForm = Dr["FORMAPAGOPRINCIPAL"].ToString(),
                                    UserLogin = Dr["CUSUARIOINGRESO"].ToString(),
                                    AdmissionDate = Convert.ToDateTime(Dr["DFECHAINGRESO"]),
                                    IncomeTerminal = Dr["CTERMINALINGRESO"].ToString(),
                                    ProductionYear = lcObjetoMoto.APSalesVehicle.VehicleYear,
                                    Cylinder = lcObjetoMoto.APSalesVehicle.Displacement,
                                    Colour = lcObjetoMoto.Color,
                                    Model = lcObjetoMoto.APSalesVehicle.Model,
                                    CountryOrigin = lcObjetoMoto.APSalesVehicle.CountryRegionIdOrigin,
                                    Brand = lcObjetoMoto.Marca,
                                };
                            }
                            else
                            {
                                RegistrarLogs(Datos, "IGTM004", "", String.Concat("ICRE009 -", ObjetoMoto.errorList.FirstOrDefault()), string.Empty, "FALSE");
                                res.errorList = ObjetoMoto.errorList;
                                res.descripcionId = ObjetoMoto.errorList.FirstOrDefault();
                                res.statusCode = 900;
                                res.CodigoError = res.statusCode;
                                res.Estado = false;
                                return res;
                            }
                        }

                    }
                    var clients = new RestClient(Baseurl);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", "application/json");
                    var jsonstring = JsonConvert.SerializeObject(IGTM004);
                    if (RegistrarLogs(Datos, "IGTM004", jsonstring, string.Empty, string.Empty, "FALSE"))
                    {
                        request.AddParameter("application/json", jsonstring, ParameterType.RequestBody);
                        IRestResponse response = clients.Execute(request);
                        var content = response.Content;
                        res = JsonConvert.DeserializeObject<GTM004Response>(content);
                        res.CodigoError = 0;
                        res.Estado = true;
                        if (res.statusCode != 201)
                        {
                            res.CodigoError = res.statusCode;
                            res.Estado = false;
                            EstadoEjecucion = "FALSE";
                        }
                        var jsonresp = JsonConvert.SerializeObject(res);
                        RegistrarLogs(Datos, "IGTM004", "", content, string.Empty, EstadoEjecucion);
                    }
                    else
                    {
                        List<string> LstError = new List<string>();
                        LstError.Add("No se pudo registrar la respuesta del evento IGTM004 en la tabla de logs");
                        res.statusCode = 300;
                        res.errorList = LstError;
                        res.CodigoError = res.statusCode;
                        res.Estado = false;
                        return res;
                    }
                }
                else
                {
                    RegistrarLogs(Datos, "IGTM004", "", "No se encontró información del pedido.", string.Empty, "FALSE");
                    res.errorList = new List<string> { "No se encontró información del pedido" };
                    res.descripcionId = "No se encontró información del pedido";
                    res.statusCode = 900;
                    res.CodigoError = res.statusCode;
                    res.Estado = false;
                }
            }
            else
            {
                res = JsonConvert.DeserializeObject<GTM004Response>(JsonResponse);
            }

            return res;
        }
        private bool bValidaFacturaMoto(ICAJ008Response iCAJ008, ref DataTable DTable)
        {
            DataTable Dt = new DataTable();
            DataRow Dr;
            bool Retorno = false;
            try
            {
                Dt.Columns.Add("FACTURA");
                Dt.Columns.Add("FECHA");
                Dt.Columns.Add("HORA");
                Dt.Columns.Add("SECUENCIAEMPRESA");
                Dt.Columns.Add("SECUENCIACAJA");
                Dt.Columns.Add("SECUENCIAFACTURA");
                Dt.Columns.Add("ITEM");
                Dt.Columns.Add("DESCRIPCION");
                Dt.Columns.Add("SERIE");
                foreach (var ListaPedido in iCAJ008.documentInvoiceRequestTableList)
                {
                    var datosFactura = ConsultaDatosFactura(ListaPedido.SalesId, ListaPedido.SalesIdAccount);
                    var itemsFactura = GetItems(ListaPedido.SalesIdAccount);
                    foreach (var ListaFactura in ListaPedido.documentInvoiceRequestLinesList)
                    {
                        foreach (var ListaItemsFactura in ListaFactura.itemList)
                        {
                            foreach (var objDetalleFacturacion in datosFactura.DetalleFacturacion)
                            {
                                foreach (var objProductoFactura in objDetalleFacturacion.Producto)
                                {
                                    if (ListaItemsFactura.itemId == objProductoFactura.CodigoProducto)
                                    {
                                        if (objProductoFactura.ProductoMoto)
                                        {
                                            Dr = Dt.NewRow();
                                            Dr["FACTURA"] = ListaFactura.InvoiceId;
                                            Dr["FECHA"] = ListaFactura.InvoiceDate.ToShortDateString();
                                            Dr["HORA"] = ListaFactura.InvoiceDate.ToShortTimeString();
                                            Dr["SECUENCIAEMPRESA"] = ListaFactura.InvoiceId.Substring(0, 3);
                                            Dr["SECUENCIACAJA"] = ListaFactura.InvoiceId.Substring(4, 3);
                                            Dr["SECUENCIAFACTURA"] = ListaFactura.InvoiceId.Substring(8);
                                            Dr["ITEM"] = ListaItemsFactura.itemId;
                                            Dr["DESCRIPCION"] = objProductoFactura.descripcionProducto;
                                            Dr["SERIE"] = objProductoFactura.Serie;
                                            Dt.Rows.Add(Dr);
                                            DTable = Dt;
                                            Retorno = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception Ex) { }
            return Retorno;
        }
    }
}