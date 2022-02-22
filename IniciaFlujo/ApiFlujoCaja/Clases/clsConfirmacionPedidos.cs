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
    public class clsConfirmacionPedidos:clsSIAC
    {

        public VTAResponse ConfirmarPedido(FlujoRequest Datos)
        {
            VTAResponse res = new VTAResponse();
            string JsonResponse = string.Empty;
            DataTable DtFormaPago = null;
            List<VTA029PurchaserTicketList> ListaBilletes = new List<VTA029PurchaserTicketList>();
            bool AplicaBillete = false;
            try
            {
                string Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlConfirmacion"]; //"http://192.168.82.43:83/FacturacionDespacho/api/Confirmar/ConfirmarPedidoSiac";
                if (!ConsultaLogs(Datos, "ConfirmarPedido - IVTA19", ref JsonResponse))
                {
                    DtFormaPago = ConsultaFormaPagoPedidos(Datos.SalesIDSiac, Datos.NumeroRecibo);
                    foreach (DataRow Dr in DtFormaPago.Rows)
                    {
                        if (Convert.ToInt32(Dr["CCODIGOFORMAPAGO"]) == Convert.ToInt32(Diccionario.FormaPagoSIAC.BilleteComprador))  //Billetes
                        {
                            VTA029PurchaserTicketList InfoBillete = new VTA029PurchaserTicketList();
                            AplicaBillete = true;
                            InfoBillete.PurchaserTicketNumber = Dr["DOCUMENTO"].ToString();
                            InfoBillete.PurchaserTicketProvisionId = Dr["IIDASIENTOPROVISION"].ToString();
                            InfoBillete.PurchaserTicketAmount = Convert.ToDecimal(Dr["SALDO"]);
                            ListaBilletes.Add(InfoBillete);
                        }
                    }

                    VTA029Request obj = new VTA029Request()
                    {
                        SalesId = Datos.SalesID,
                        SalesIdSiac = Datos.SalesIDSiac,
                        User = Datos.UsuarioIngreso,
                        Terminal = Datos.TerminalIngreso,
                        Source = Datos.SalesOrigin,
                        HasCreditNotePurchaserTicket = AplicaBillete,
                        PurchaserTicketList = ListaBilletes,
                        InvoiceId = Datos.InvoiceId,
                        Motive = Datos.Motive,
                        Amount = Datos.Monto,
                        Cpn = Datos.CPN,
                        Voucher = Datos.Voucher
                    };

                    var jsonreq = JsonConvert.SerializeObject(obj);
                    if (RegistrarLogs(Datos, "ConfirmarPedido - IVTA19", jsonreq, "", "", "FALSE"))
                    {
                        using (var client = new HttpClient())
                        {
                            client.CancelPendingRequests();

                            client.DefaultRequestHeaders.Clear();
                            client.BaseAddress = new Uri(Baseurl);
                            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                            var postTask = client.PostAsJsonAsync<VTA029Request>("ConfirmarPedidoSiac", obj);
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

                                res = JsonConvert.DeserializeObject<VTAResponse>(readTask);
                                res.Estado = true;
                                res.Mensaje = "OK";

                                if (res.MensajeError.Count > 0)
                                {
                                    Valor = "FALSE";
                                }


                                var jsonresp = JsonConvert.SerializeObject(res);
                                if (RegistrarLogs(Datos, "ConfirmarPedido - IVTA19", jsonreq, jsonresp, "", Valor))
                                {
                                    return res;
                                }
                                else
                                {
                                    res.Estado = false;
                                    res.Mensaje = "No se pudo registrar la respuesta del evento ConfirmarPedido en la tabla de logs";
                                }

                            }
                            else
                            {
                                var jsonresp = JsonConvert.SerializeObject(result);
                                if (RegistrarLogs(Datos, "ConfirmarPedido - IVTA19", jsonreq, jsonresp, "", "FALSE"))
                                {
                                    return res;
                                }
                                else
                                {
                                    res.Estado = false;
                                    res.Mensaje = "No se pudo registrar la respuesta del evento ConfirmarPedido en la tabla de logs";
                                }
                            }
                        }
                    }
                    else
                    {
                        res.Estado = false;
                        res.Mensaje = "No se pudo registrar el evento ConfirmarPedido en la tabla de logs";
                    }
                }
                else
                {
                    res = JsonConvert.DeserializeObject<VTAResponse>(JsonResponse);
                    //res.CodigoMensaje = "200";
                    //res.response = true;
                    //res.messages = "El proceso seejcuto anteriormente";
                }
            }
            catch (Exception Ex)
            { 
            }
            return res;
        }
    }
}