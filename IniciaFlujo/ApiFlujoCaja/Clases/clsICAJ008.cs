using ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http.Headers;
using System.Net.Http;

namespace ApiFlujoCaja
{
    public class clsICAJ008:clsSIAC
    {

        public ICAJ008Response ICAJ008(FlujoRequest Datos)
        {
            ICAJ008Response res = new ICAJ008Response(); 
            string JsonResponse = string.Empty;
            bool isMoto = false;
            string Mensaje = "Factura con: ";
            try
            {
                string Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlICAJ008"]; //"http://192.168.82.43:83/ICAJ008/APICAJ008001";

                if (!ConsultaLogs(Datos, "ICAJ008", ref JsonResponse))
                {
                    var datosFactura = ConsultaDatosFactura(Datos.SalesID, Datos.SalesIDSiac);
                    var itemsFactura = GetItems(Datos.SalesIDSiac);
                    List<APDocumentInvoiceTableICAJ008001> aPDocuments = new List<APDocumentInvoiceTableICAJ008001>();

                    var listaFac = datosFactura.DetalleFacturacion.GroupBy(u => u.AgrupadorFacturacion)
                                    .Select(grp => grp.ToList())
                                    .ToList();


                    foreach (var item in listaFac)
                    {
                        List<APDocumentInvoiceLinesICAJ008001> ditems = new List<APDocumentInvoiceLinesICAJ008001>();

                        int IntProductoFactura = 0;
                        foreach (var detalleFac in item)
                        {
                            if (detalleFac.EsAsistenciaFacilita || detalleFac.EsGarantiaExtendida || detalleFac.EsMatricula)
                            {
                                #region  "Agrupa mensaje para el tipo de factura"
                                if (detalleFac.EsAsistenciaFacilita && !detalleFac.EsGarantiaExtendida)
                                {
                                    Mensaje = string.Concat(Mensaje, "Asistencia Facilita", ", ");
                                }
                                if (detalleFac.EsGarantiaExtendida)
                                {
                                    Mensaje = string.Concat(Mensaje, "Garantia Extendida", ", ");
                                }
                                if (detalleFac.EsMatricula)
                                {
                                    Mensaje = string.Concat(Mensaje, "Matricula", ", ");
                                }
                                //bPedidoAsistencia = detalleFac.EsAsistenciaFacilita;
                                #endregion
                                int PosicionProducto = 0;
                                foreach (var producto in detalleFac.Producto)
                                {
                                    if (!isMoto && detalleFac.EsGarantiaExtendida) //Esto es por un electrodomestico con garantia extendida, pero puede entrar por si existe una asistencia facilita
                                    {
                                        // No hace nada porque es una garantia extendia arelacionada a un producto principal 
                                        if (!detalleFac.EsGarantiaExtendida)
                                        {
                                            if (PosicionProducto == IntProductoFactura)
                                            {
                                                ditems.Add(new APDocumentInvoiceLinesICAJ008001
                                                {
                                                    ItemId = producto.CodigoProducto,
                                                    Serial = producto.Serie,
                                                    Qty = producto.Cantidad
                                                });
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (PosicionProducto == IntProductoFactura)
                                        {
                                            ditems.Add(new APDocumentInvoiceLinesICAJ008001
                                            {
                                                ItemId = producto.CodigoProducto,
                                                Serial = producto.Serie,
                                                Qty = producto.Cantidad
                                            });
                                        }
                                    }
                                    PosicionProducto = PosicionProducto + 1;
                                }

                            }
                            else
                            {
                                int PosicionProducto = 0;
                                foreach (var producto in detalleFac.Producto)
                                {
                                    if (producto.ProductoMoto)
                                    {
                                        if (PosicionProducto == IntProductoFactura)
                                        {
                                            Mensaje = string.Concat(Mensaje, "Moto", ", ");
                                            isMoto = true;
                                            //bPedidoMoto = true;
                                            ditems.Add(new APDocumentInvoiceLinesICAJ008001
                                            {
                                                ItemId = producto.CodigoProducto,
                                                Serial = producto.Serie,
                                                Qty = producto.Cantidad
                                            });
                                        }
                                    }
                                    else if (producto.ProductoMarketplace)
                                    {
                                        Mensaje = string.Concat(Mensaje, "Producto MarketPlace", ", ");
                                    }
                                    else
                                    {
                                        if (isMoto)
                                        {
                                            if (PosicionProducto == IntProductoFactura)
                                            {
                                                Mensaje = string.Concat(Mensaje, producto.descripcionProducto, ", ");
                                                ditems.Add(new APDocumentInvoiceLinesICAJ008001
                                                {
                                                    ItemId = producto.CodigoProducto,
                                                    Serial = producto.Serie,
                                                    Qty = producto.Cantidad
                                                });
                                            }
                                        }
                                        else
                                        {
                                            if (PosicionProducto == IntProductoFactura)
                                            {
                                                Mensaje = string.Concat(Mensaje, producto.descripcionProducto, ", ");
                                            }
                                        }
                                    }
                                    PosicionProducto = PosicionProducto + 1;
                                }
                            }
                            IntProductoFactura = IntProductoFactura + 1;
                        }
                        if (ditems.Count > 0)
                        {
                            string numsec = NumeroSecuencia(Datos.CodigoTIendaSIAC, "FAE");
                            aPDocuments.Add(new APDocumentInvoiceTableICAJ008001
                            {
                                CustAccount = itemsFactura.Cabecera[0].CustAccount,
                                DocumentDate = datosFactura.FechaOrdenVenta,
                                InvoiceDate = datosFactura.FechaOrdenVenta,
                                InvoiceId = "",
                                NumberSecuence = numsec,
                                PostingProfile = itemsFactura.Cabecera[0].PostingProfile,
                                SalesId = datosFactura.NumeroOrdenVenta,
                                SalesOrigin = itemsFactura.Cabecera[0].SalesOrigin,
                                DocumentInvoiceLinesList = ditems
                            });
                        }


                    }
                    if (aPDocuments.Count > 0)
                    {
                        ICAJ008Request obj = new ICAJ008Request()
                        {
                            DataAreaId = Datos.CodigoEmpresa,
                            Enviroment = "SIT",
                            APDocumentInvoiceTableICAJ008001 = aPDocuments
                        };

                        Mensaje = Mensaje.Substring(0, Mensaje.Length - 2);
                        var jsonstring = JsonConvert.SerializeObject(obj);
                        if (RegistrarLogs(Datos, "ICAJ008", jsonstring, string.Empty, Mensaje, "FALSE"))
                        {
                            using (var client = new HttpClient())
                            {
                                client.CancelPendingRequests();

                                client.DefaultRequestHeaders.Clear();
                                client.BaseAddress = new Uri(Baseurl);
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                var postTask = client.PostAsJsonAsync<ICAJ008Request>("APICAJ008001", obj);
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
                                    string Valor = "TRUE";
                                    var readTask = result.Content.ReadAsStringAsync().Result;

                                    res = JsonConvert.DeserializeObject<ICAJ008Response>(readTask);
                                    if (res.errorList.Count > 0)
                                    {
                                        Valor = "FALSE";
                                    }

                                    var jsonresp = JsonConvert.SerializeObject(res);
                                    if (RegistrarLogs(Datos, "ICAJ008", "", jsonresp, Mensaje, Valor))
                                    {
                                        if (res.errorList.Count <= 0)
                                        {
                                            res.Estado = true;
                                            res.Mensaje = "OK";
                                        }
                                        else
                                        {
                                            res.Estado = false;
                                            res.Mensaje = "Respuesta de Dynamics con errores";
                                        }
                                    }
                                    else
                                    {
                                        res.statusId = true;
                                        res.Estado = false;
                                        res.Mensaje = "No se pudo registrar la respuesta del evento ICAJ008 en la tabla de logs";
                                    }
                                    return res;
                                }
                                else
                                {
                                    var jsonresp = JsonConvert.SerializeObject(result);
                                    //var resss = RegistrarTrx(8, salesid, salessiac, "", "", false, false, "", jsonstring, "", "", "", "", "", "", jsonresp);
                                    if (RegistrarLogs(Datos, "ICAJ008", "", jsonresp, Mensaje, "FALSE"))
                                    {
                                        res.Estado = true;
                                        res.Mensaje = "OK";
                                    }
                                    else
                                    {
                                        res.statusId = true;
                                        res.Estado = false;
                                        res.Mensaje = "No se pudo registrar la respuesta del evento ICAJ008 en la tabla de logs";
                                    }
                                }
                                return res;
                            }
                        }
                        else
                        {
                            res.Estado = false;
                            res.Mensaje = "No se pudo registrar el evento ICAJ008 en la tabla de logs";
                            return res;
                        }
                    }
                    else
                    {
                        if (RegistrarLogs(Datos, "ICAJ008", "", "EL PEDIDO NO CONTIENE ITEMS PARA FACTURAR", Mensaje, "TRUE"))
                        {
                            res.statusId = true;
                            return res;
                        }
                        else
                        {
                            res.statusId = true;
                            res.Estado = false;
                            res.Mensaje = "No se pudo registrar la respuesta del evento ICAJ008 en la tabla de logs";
                            return res;
                        }
                    }
                }
                else
                {
                    res = JsonConvert.DeserializeObject<ICAJ008Response>(JsonResponse);
                    return res;
                }
            }
            catch (Exception ex)
            {
                RegistrarLogs(Datos, "ICAJ008", "", ex.Message, Mensaje, "FALSE");
                res.Estado = false;
                res.Mensaje = ex.Message;
                return res;
            }
        }
    }
}