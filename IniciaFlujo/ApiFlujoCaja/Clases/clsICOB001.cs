using ApiModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace ApiFlujoCaja
{
    public class clsICOB001 : clsSIAC
    {
        public ICOB001Response ICOB001(FlujoRequest Datos, ICAJ008Response iCAJ008)
        {
            ICOB001Response res = new ICOB001Response();
            string JsonResponse = string.Empty;
            string vlAsientoContable = string.Empty;
            DataTable Dt = null;
            DataTable DtFormaPago = null;
            try
            {
                string Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlICOB001"]; //"http://192.168.82.43:83/ICOB001/APICOB001001";
                if (!ConsultaLogs(Datos, "ICOB001", ref JsonResponse))
                {
                    Dt = new DataTable();
                    var itemsFactura = GetItems(Datos.SalesIDSiac);
                    var datosFactura = ConsultaDatosFactura(Datos.SalesID, Datos.SalesIDSiac);
                    List<APSettlementTransactionHeader> ApCabecera = new List<APSettlementTransactionHeader>();
                    #region "Crea Tabla Facturas"
                    foreach (var tables in iCAJ008.documentInvoiceRequestTableList)
                    {
                        if (tables.documentInvoiceRequestLinesList.Count > 0)
                        {
                            Dt.Columns.Add("Factura");
                            Dt.Columns.Add("Voucher");
                            Dt.Columns.Add("Valor");
                            Dt.Columns.Add("IsServicio");
                            foreach (var lines in tables.documentInvoiceRequestLinesList)
                            {
                                DataRow Dr = Dt.NewRow();
                                Dr["Factura"] = lines.InvoiceId;
                                Dr["Voucher"] = lines.Voucher;
                                Dr["Valor"] = lines.TotalAmount;
                                Dr["IsServicio"] = false;
                                foreach (var Items in lines.itemList)
                                {
                                    foreach (var Detalle in datosFactura.DetalleFacturacion)
                                    {
                                        bool isExisteItem = false;
                                        foreach (var Producto in Detalle.Producto)
                                        {
                                            if (Items.itemId == Producto.CodigoProducto)
                                            {
                                                isExisteItem = true;
                                                break;
                                            }
                                        }
                                        if (isExisteItem)
                                        {
                                            if (Detalle.EsAsistenciaFacilita || Detalle.EsGarantiaExtendida)
                                            {
                                                Dr["IsServicio"] = true;
                                                break;
                                            }
                                        }
                                    }

                                }
                                Dt.Rows.Add(Dr);
                            }
                        }
                    }
                    #endregion
                    #region "Crea Tabla Forma Pagos"
                    DtFormaPago = ConsultaFormaPagoPedidos(Datos.SalesIDSiac, DBNull.Value);
                    #endregion

                    //Valido primero billete comprado para cruzarlo con la factura de Moto / Electrodomestico

                    foreach (DataRow Dr in DtFormaPago.Rows)
                    {
                        List<APDocumentICOB001001> ApDetalleFactura = new List<APDocumentICOB001001>();
                        decimal ValorCobroMedio = Convert.ToDecimal(Dr["SALDO"]);
                        if (Convert.ToInt32(Dr["CCODIGOFORMAPAGO"]) == 33)  //Billetes
                        {
                            foreach (DataRow DrRow in Dt.Rows)
                            {
                                if (!Convert.ToBoolean(DrRow["IsServicio"]))
                                {
                                    decimal ValorFactura = 0;
                                    ValorFactura = Convert.ToDecimal(DrRow["Valor"]);

                                    if (ValorCobroMedio >= ValorFactura)
                                    {
                                        ApDetalleFactura.Add(new APDocumentICOB001001
                                        {
                                            Amount = ValorFactura,
                                            InvoiceId = DrRow["Factura"].ToString(),
                                            Voucher = DrRow["Voucher"].ToString()
                                        });
                                        DrRow["Valor"] = 0;
                                        Dt.AcceptChanges();

                                        ValorCobroMedio = ValorCobroMedio - ValorFactura;
                                        Dr["SALDO"] = ValorCobroMedio;
                                        DtFormaPago.AcceptChanges();
                                    }
                                    else
                                    {
                                        ApDetalleFactura.Add(new APDocumentICOB001001
                                        {
                                            Amount = ValorCobroMedio,
                                            InvoiceId = DrRow["Factura"].ToString(),
                                            Voucher = DrRow["Voucher"].ToString()
                                        });
                                        DrRow["Valor"] = ValorFactura - ValorCobroMedio;
                                        Dt.AcceptChanges();
                                        Dr["SALDO"] = 0;
                                        DtFormaPago.AcceptChanges();
                                        goto NuevoMedio;
                                    }
                                }
                                if (ValorCobroMedio == 0)
                                {
                                    ValorCobroMedio = Convert.ToDecimal(Dr["Valor"]);
                                }
                            }
                        }
                    NuevoMedio:;
                        if (ApDetalleFactura.Count > 0)
                        {
                            vlAsientoContable = Dr["IDASIENTO"].ToString();   // Aqui se debe consultar el ID asiento para el Billete
                            ApCabecera.Add(new APSettlementTransactionHeader
                            {
                                Amount = ValorCobroMedio,
                                DateTrans = DateTime.Now,
                                IdReciboCobro = Datos.SalesIDSiac,
                                InvoiceId = string.Empty,
                                VoucherSettlement = vlAsientoContable,
                                APDocumentList = ApDetalleFactura
                            });
                        }
                    }

                    //Valido anticipos debido a que puede tener 1 o varios ID asientos

                    foreach (DataRow Dr in DtFormaPago.Rows)
                    {
                        List<APDocumentICOB001001> ApDetalleFactura = new List<APDocumentICOB001001>();
                        decimal ValorCobroMedioPrincipal = Convert.ToDecimal(Dr["SALDO"]);
                        if (Convert.ToInt32(Dr["CCODIGOFORMAPAGO"]) == 10)  //Anticipos
                        {
                            DataTable DtFormaPagoAnticipos = ConsultaFormaPagoPedidos(DBNull.Value, Dr["DOCUMENTO"].ToString());

                            foreach (DataRow DrAnticipo in DtFormaPagoAnticipos.Rows)
                            {
                                ApDetalleFactura = new List<APDocumentICOB001001>();
                                decimal ValorCobroMedio = Convert.ToDecimal(DrAnticipo["SALDO"]);
                                foreach (DataRow DrRow in Dt.Rows)
                                {
                                    decimal ValorFactura = 0;
                                    ValorFactura = Convert.ToDecimal(DrRow["Valor"]);

                                    if (ValorCobroMedio >= ValorFactura)
                                    {
                                        ApDetalleFactura.Add(new APDocumentICOB001001
                                        {
                                            Amount = ValorFactura,
                                            InvoiceId = DrRow["Factura"].ToString(),
                                            Voucher = DrRow["Voucher"].ToString()
                                        });
                                        DrRow["Valor"] = 0;
                                        Dt.AcceptChanges();

                                        ValorCobroMedio = ValorCobroMedio - ValorFactura;
                                        Dr["SALDO"] = ValorCobroMedio;
                                        DtFormaPago.AcceptChanges();
                                    }
                                    else
                                    {
                                        ApDetalleFactura.Add(new APDocumentICOB001001
                                        {
                                            Amount = ValorCobroMedio,
                                            InvoiceId = DrRow["Factura"].ToString(),
                                            Voucher = DrRow["Voucher"].ToString()
                                        });
                                        DrRow["Valor"] = ValorFactura - ValorCobroMedio;
                                        Dt.AcceptChanges();
                                        ValorCobroMedioPrincipal = ValorCobroMedioPrincipal - ValorCobroMedio;
                                        Dr["SALDO"] = ValorCobroMedioPrincipal;
                                        DtFormaPago.AcceptChanges();
                                        DrAnticipo["SALDO"] = 0;
                                        DtFormaPagoAnticipos.AcceptChanges();
                                        goto NuevoMedio;
                                    }

                                    if (ValorCobroMedio == 0)
                                    {
                                        ValorCobroMedio = Convert.ToDecimal(Dr["Valor"]);
                                    }
                                }

                            NuevoMedio:;
                                if (ApDetalleFactura.Count > 0)
                                {
                                    vlAsientoContable = DrAnticipo["IDASIENTO"].ToString();   // Aqui se debe consultar el ID asiento para el Billete
                                    ApCabecera.Add(new APSettlementTransactionHeader
                                    {
                                        Amount = ValorCobroMedio,
                                        DateTrans = DateTime.Now,
                                        IdReciboCobro = Datos.SalesIDSiac,
                                        InvoiceId = string.Empty,
                                        VoucherSettlement = vlAsientoContable,
                                        APDocumentList = ApDetalleFactura
                                    });
                                }
                            }
                        }

                    }


                    //Valido el resto de medios de pago con todas las facturas con saldos
                    foreach (DataRow Dr in DtFormaPago.Rows)
                    {
                        List<APDocumentICOB001001> ApDetalleFactura = new List<APDocumentICOB001001>();
                        decimal ValorCobroMedio = Convert.ToDecimal(Dr["SALDO"]);
                        // Billetes / Credito Facilito / Anticipos / Cheque Vista / Deposito Cheque
                        // A esta instancia validar que todos los otros mediso ya deben venir con el ID de asiento definitivo
                        if (Convert.ToInt32(Dr["CCODIGOFORMAPAGO"]) != 33 && Convert.ToInt32(Dr["CCODIGOFORMAPAGO"]) != 36 &&
                            Convert.ToInt32(Dr["CCODIGOFORMAPAGO"]) != 10 && Convert.ToInt32(Dr["CCODIGOFORMAPAGO"]) != 3 && Convert.ToInt32(Dr["CCODIGOFORMAPAGO"]) != 61)
                        {
                            foreach (DataRow DrRow in Dt.Rows)
                            {

                                decimal ValorFactura = 0;
                                ValorFactura = Convert.ToDecimal(DrRow["Valor"]);

                                if (ValorCobroMedio >= ValorFactura)
                                {
                                    ApDetalleFactura.Add(new APDocumentICOB001001
                                    {
                                        Amount = ValorFactura,
                                        InvoiceId = DrRow["Factura"].ToString(),
                                        Voucher = DrRow["Voucher"].ToString()
                                    });
                                    DrRow["Valor"] = 0;
                                    Dt.AcceptChanges();

                                    ValorCobroMedio = ValorCobroMedio - ValorFactura;
                                    Dr["SALDO"] = ValorCobroMedio;
                                    DtFormaPago.AcceptChanges();
                                }
                                else
                                {
                                    ApDetalleFactura.Add(new APDocumentICOB001001
                                    {
                                        Amount = ValorCobroMedio,
                                        InvoiceId = DrRow["Factura"].ToString(),
                                        Voucher = DrRow["Voucher"].ToString()
                                    });
                                    DrRow["Valor"] = ValorFactura - ValorCobroMedio;
                                    Dt.AcceptChanges();
                                    Dr["SALDO"] = 0;
                                    DtFormaPago.AcceptChanges();
                                    goto NuevoMedio;
                                }

                                if (ValorCobroMedio == 0)
                                {
                                    if (Convert.ToDecimal(Dr["SALDO"]) == 0)
                                    {
                                        ValorCobroMedio = Convert.ToDecimal(Dr["IVALOR"]);
                                    }
                                    else
                                    {
                                        ValorCobroMedio = Convert.ToDecimal(Dr["SALDO"]);
                                    }
                                }
                            }
                        }
                    NuevoMedio:;
                        if (ApDetalleFactura.Count > 0)
                        {
                            vlAsientoContable = Dr["IDASIENTO"].ToString();   // Aqui se debe consultar el ID asiento para anticipos
                            ApCabecera.Add(new APSettlementTransactionHeader
                            {
                                Amount = ValorCobroMedio,
                                DateTrans = DateTime.Now,
                                IdReciboCobro = Datos.SalesIDSiac,
                                InvoiceId = string.Empty,
                                VoucherSettlement = vlAsientoContable,
                                APDocumentList = ApDetalleFactura
                            });
                        }
                    }


                    ICOB001Request obj = new ICOB001Request()
                    {
                        Enviroment = "SIT",
                        DataAreaId = Datos.CodigoEmpresa,
                        CustAccount = Datos.CodigoCliente,
                        APSettlementTransactionHeaderList = ApCabecera
                    };

                    var jsonstring = JsonConvert.SerializeObject(obj);
                    if (RegistrarLogs(Datos, "ICOB001", jsonstring, string.Empty, string.Empty, string.Empty))
                    {
                        if (ApCabecera.Count > 0)
                        {
                            using (var client = new HttpClient())
                            {
                                client.CancelPendingRequests();

                                client.DefaultRequestHeaders.Clear();
                                client.BaseAddress = new Uri(Baseurl);
                                //client.Timeout = TimeSpan.FromSeconds(25);
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                var postTask = client.PostAsJsonAsync<ICOB001Request>("APICOB001001", obj);
                                try
                                {
                                    postTask.Wait();
                                }
                                catch (Exception ex)
                                {
                                    RegistrarLogs(Datos, "ICOB001", "", ex.Message, string.Empty, "FALSE");
                                    res.Estado = false;
                                    res.Mensaje = ex.Message;
                                    return res;
                                }

                                var result = postTask.Result;
                                if (result.IsSuccessStatusCode)
                                {
                                    string Valor = "TRUE";
                                    var readTask = result.Content.ReadAsStringAsync().Result;
                                    res = JsonConvert.DeserializeObject<ICOB001Response>(readTask);
                                    if (res.ErrorList.Count > 0)
                                    {
                                        Valor = "FALSE";
                                    }

                                    res.Estado = true;
                                    res.Mensaje = "OK";
                                    var jsonresp = JsonConvert.SerializeObject(res);

                                    if (RegistrarLogs(Datos, "ICOB001", "", jsonresp, string.Empty, Valor))
                                    {
                                        res.Estado = Convert.ToBoolean(Valor);
                                        res.Mensaje = "OK";
                                    }
                                    else
                                    {
                                        res.Estado = false;
                                        res.Mensaje = "No se pudo registrar la respuesta del evento ICAJ008 en la tabla de logs";
                                    }
                                }
                                else
                                {
                                    RegistrarLogs(Datos, "ICOB001", "", result.ToString(), string.Empty, "FALSE");
                                }
                            }
                        }
                        else
                        {
                            RegistrarLogs(Datos, "ICOB001", "", "Los medios de pago utilizados en el recaudo no se deben liquidar.", string.Empty, "FALSE");
                            res.Estado = true;
                            res.Mensaje = "OK";

                        }
                    }
                    else
                    {
                        res.Estado = false;
                        res.Mensaje = "No se pudo registrar la respuesta del evento ICOB001 en la tabla de logs";
                    }
                }
                else
                {
                    res = JsonConvert.DeserializeObject<ICOB001Response>(JsonResponse);
                }
                return res;
            }
            catch (Exception ex)
            {
                res.Estado = false;
                res.Mensaje = ex.Message;
                return res;
            }
        }

    }
}