using ApiModels;
using ApiModels.CarteraDTO;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiFlujoCaja
{
    public class clsGeneracionCartera: clsSIAC
    {
        public ResponseCartera GeneracionCartera(FlujoRequest Datos, ICAJ008Response parametros)
        {
            ResponseCartera respuesta = new ResponseCartera();
            string JsonResponse = string.Empty;
            RestClient client = null;
            string Baseurl = "";
            Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlGeneracionCartera"];

            if (!ConsultaLogs(Datos, "GeneracionCartera - CJ008", ref JsonResponse))
            {
                try
                {
                    List<APDocumentInvoiceRequestTableICAJ008001> listaDocumentos = parametros.documentInvoiceRequestTableList;
                    List<DocumentInvoice> listaDocumentosCartera = new List<DocumentInvoice>();
                    foreach (var item in listaDocumentos)
                    {
                        ApiModels.CarteraDTO.DocumentInvoice document = new ApiModels.CarteraDTO.DocumentInvoice();
                        document.CusAccount = item.CustAccount;
                        document.PostingProfile = item.PostingProfile;
                        document.SalesId = item.SalesId;
                        document.SalesIdAccount = item.SalesIdAccount;
                        document.Store = item.Store;
                        document.TotalAmount = item.TotalAmount;
                        document.User = "FacturacionDespacho";
                        document.Terminal = "172.1.1.1";

                        //document.ProvisionNOcList = listaDocumentos[0].ProvisionNOcList;
                        //document.SalesOriginId = listaDocumentos[0].SalesOriginId;

                        //if (string.IsNullOrEmpty(listaDocumentos[0].User))
                        //{
                        //    document.User = "FacturacionDespacho";
                        // }
                        //else
                        //{
                        //    document.User = listaDocumentos[0].;
                        //}

                        List<DocumentInvoiceLine> documentInvoiceRequestLineList = new List<DocumentInvoiceLine>();
                        foreach (var itemLinea in item.documentInvoiceRequestLinesList)
                        {
                            DocumentInvoiceLine linea = new DocumentInvoiceLine();
                            linea.InvoiceDate = itemLinea.InvoiceDate;
                            linea.InvoiceId = itemLinea.InvoiceId;
                            linea.SecuenciaFacturacion = itemLinea.secuenciaFacturacion;
                            linea.TotalAmount = itemLinea.TotalAmount;
                            linea.Voucher = itemLinea.Voucher;

                            //ITEM LIST
                            List<ItemList> listaItemList = new List<ItemList>();
                            foreach (var itemList in itemLinea.itemList)
                            {
                                ItemList lista = new ItemList();
                                lista.ItemId = itemList.itemId;
                                lista.Qty = itemList.qty;
                                lista.AmountLine = Convert.ToDecimal(itemList.amountLine);
                                lista.OrdenItems = itemList.ordenItems;

                                listaItemList.Add(lista);
                            }
                            linea.ItemList = listaItemList;
                            documentInvoiceRequestLineList.Add(linea);
                        }

                        document.Lista = documentInvoiceRequestLineList;
                        listaDocumentosCartera.Add(document);

                    }

                    //CONSUMO DE LA API DE CARTERA
                    client = new RestClient(Baseurl);  
                    foreach (var elemento in listaDocumentosCartera)
                    {
                        if (RegistrarLogs(Datos, "GeneracionCartera - CJ008", JsonConvert.SerializeObject(elemento), "", "", "FALSE"))
                        {
                            var requestCartera = new RestRequest(Baseurl, Method.POST, DataFormat.Json);

                            requestCartera.AddParameter("application/json", JsonConvert.SerializeObject(elemento), ParameterType.RequestBody);
                            var responseApi = client.Post(requestCartera);
                            var content = responseApi.Content;
                            var resultado = JsonConvert.DeserializeObject<ApiModels.CarteraDTO.ResponseCartera>(content);
                            respuesta = resultado;
                            if (resultado.StatusCode.Equals("OK"))
                            {
                                respuesta.Estado = true;
                                respuesta.CodigoError = 0;
                                RegistrarLogs(Datos, "GeneracionCartera - CJ008", JsonConvert.SerializeObject(elemento), JsonConvert.SerializeObject(content), string.Empty, "TRUE");
                            }
                            else
                            {
                                respuesta.Estado = false;
                                respuesta.CodigoError = 400;
                                RegistrarLogs(Datos, "GeneracionCartera - CJ008", JsonConvert.SerializeObject(elemento), JsonConvert.SerializeObject(content), string.Empty, "FALSE");
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    respuesta.Estado = false;
                    respuesta.CodigoError = 99;
                    RegistrarLogs(Datos, "GeneracionCartera - CJ008", string.Empty, ex.Message, string.Empty, "FALSE");
                }
            }
            return respuesta;
        }


    }
}