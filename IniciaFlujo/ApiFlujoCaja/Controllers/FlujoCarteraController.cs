using ApiModels;
using ApiModels.CarteraDTO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace ApiFlujoCaja.Controllers
{
    public class FlujoCarteraController : ApiController
    {
        [HttpPost]
        [Route("api/inicioFlujoCartera")]
        public HttpResponseMessage IniciaFlujo(

            [FromBody] ICAJ008Response jObjectParametros

        )
        {
            JObject jsonValores = new JObject();
            RestClient client = null;
            string Baseurl = "";

            try
            {

                List<APDocumentInvoiceRequestTableICAJ008001> listaDocumentos = jObjectParametros.documentInvoiceRequestTableList;

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
                Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlGeneracionCartera"];
                client = new RestClient(Baseurl);

                foreach (var elemento in listaDocumentosCartera)
                {
                    //RegistrarLogs(elemento.SalesId, elemento.SalesIdAccount, "99", "Dynamics", "Generacion de Cartera", JsonConvert.SerializeObject(elemento), string.Empty, string.Empty, "FALSE");

                    var requestCartera = new RestRequest(Baseurl, Method.POST, DataFormat.Json);

                    requestCartera.AddParameter("application/json", JsonConvert.SerializeObject(elemento), ParameterType.RequestBody);
                    var responseApi = client.Post(requestCartera);
                    var content = responseApi.Content;
                    var resultado = JsonConvert.DeserializeObject<ApiModels.CarteraDTO.ResponseCartera>(content);
                    if (resultado.StatusCode.Equals("OK"))
                    {
                        RegistrarLogs(elemento.SalesId, elemento.SalesIdAccount, "99", "Dynamics", "Generacion de Cartera", JsonConvert.SerializeObject(elemento), JsonConvert.SerializeObject(content), string.Empty, "TRUE");
                    }
                    else
                    {
                        RegistrarLogs(elemento.SalesId, elemento.SalesIdAccount, "99", "Dynamics", "Generacion de Cartera", JsonConvert.SerializeObject(elemento), JsonConvert.SerializeObject(content), string.Empty, "FALSE");
                    }

                }





                //LLAMADA A SERVICIO DE COMISIONES - INICIO
                Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlComisiones"];
                client = new RestClient(Baseurl);

                //CREACION DE PARAMETROS
                List<ApiModels.ComisionesDTO.DocumentInvoice> listaDocumentosFactura = new List<ApiModels.ComisionesDTO.DocumentInvoice>();

                foreach (var item in listaDocumentos)
                {
                    int secuencia = 0;
                    foreach (var itemLista in item.documentInvoiceRequestLinesList)
                    {
                        ApiModels.ComisionesDTO.DocumentInvoice documento = new ApiModels.ComisionesDTO.DocumentInvoice();
                        documento.SalesId = item.SalesId;
                        documento.CustAccount = item.CustAccount;
                        documento.SalesOrigin = item.SalesIdAccount;
                        documento.NumberSecuence = (secuencia + 1).ToString();
                        documento.DocumentDate = DateTime.Now;
                        documento.InvoiceId = itemLista.InvoiceId; //Esta en el detalle
                        documento.InvoiceDate = itemLista.InvoiceDate; //Esta en el detalle

                        List<ApiModels.ComisionesDTO.DocumentInvoiceLine> listaDocumentLinea = new List<ApiModels.ComisionesDTO.DocumentInvoiceLine>();
                        foreach (var itemProducto in itemLista.itemList)
                        {
                            ApiModels.ComisionesDTO.DocumentInvoiceLine linea = new ApiModels.ComisionesDTO.DocumentInvoiceLine();
                            linea.ItemId = itemProducto.itemId;
                            linea.Serial = "";
                            linea.Cantidad = itemProducto.qty;
                            linea.PostingProfile = item.PostingProfile;

                            listaDocumentLinea.Add(linea);

                        }
                        documento.ListaDocumentInvoiceLine = listaDocumentLinea;
                        listaDocumentosFactura.Add(documento);
                        secuencia++;
                    }

                    //CONSUMO DE LA API DE COMISIONES - INICIO
                    ApiModels.ComisionesDTO.RequestComisiones jsonComision = new ApiModels.ComisionesDTO.RequestComisiones();
                    jsonComision.DataAreaId = "0001";
                    jsonComision.Entorno = "SIT";
                    //requestComision.Sesion = jObjectParametros.Sesion;
                    jsonComision.DocumenInvoiceList = listaDocumentosFactura;


                    var requestComision = new RestRequest(Baseurl, Method.POST, DataFormat.Json);
                    requestComision.AddParameter("application/json", JsonConvert.SerializeObject(jsonComision), ParameterType.RequestBody);
                    var responseComision = client.Post(requestComision);
                    var contentComision = responseComision.Content;
                    var resultadoComision = JsonConvert.DeserializeObject<ApiModels.ComisionesDTO.ResponseComisiones>(contentComision);
                    if (resultadoComision.StatusCode == "200" || resultadoComision.StatusCode == "201")
                    {
                        RegistrarLogs(item.SalesId, item.SalesIdAccount, "100", "Dynamics", "Comisiones", JsonConvert.SerializeObject(jsonComision), JsonConvert.SerializeObject(contentComision), string.Empty, "TRUE");
                    }
                    else
                    {
                        RegistrarLogs(item.SalesId, item.SalesIdAccount, "100", "Dynamics", "Comisiones", JsonConvert.SerializeObject(jsonComision), JsonConvert.SerializeObject(contentComision), string.Empty, "FALSE");
                    }
                    //LLAMADA SERVICIO COMISIONES - FIN

                }


                //CONSUMO DEASISTENCIA FACILITA
                string numeroFactura = "";
                string codigoSucursal = "";
                string identificacionCliente = "";
                string medioPago = "";
                decimal valorPrima = 0;
                int numeroProducto = 0;
                Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlAsistenciaFacilita"];
                foreach (var itemDocumento in listaDocumentos)
                {
                    //CONSULTANDO LOS DATOS DE LA FACTURA
                    var datosFactura = ConsultaDatosFactura(itemDocumento.SalesId, itemDocumento.SalesIdAccount);

                    if (datosFactura.FormaPago.ToUpper().Equals("EFECTIVO"))
                        medioPago = "EF";
                    else
                        medioPago = "Plan Financiero";

                    //RRECORRIENDO ELL DETALLE DE LOS ITEMS DE LA FACTURA
                    foreach (var item in datosFactura.DetalleFacturacion)
                    {
                        if (item.EsAsistenciaFacilita)
                        {
                            foreach (var itemProducto in listaDocumentos)
                            {
                                codigoSucursal = itemProducto.Store;
                                numeroProducto = int.Parse(itemProducto.SalesIdAccount);
                                identificacionCliente = itemProducto.CustAccount;
                                foreach (var productos in itemProducto.documentInvoiceRequestLinesList)
                                {
                                    foreach (var itemLinea in productos.itemList)
                                    {
                                        foreach (var detalle in item.Producto)
                                        {
                                            valorPrima = detalle.SaldoFinanciar;
                                            if (detalle.CodigoProducto == itemLinea.itemId)
                                            {
                                                numeroFactura = productos.InvoiceId;
                                                break;
                                            }
                                        }
                                    }
                                }

                            }

                            //CONSUMO DE ASISTENCIA FACILITA - INI
                            dynamic jsonRequestFacilita = new JObject();
                            jsonRequestFacilita.CodigoProductoSeguro = numeroProducto;
                            jsonRequestFacilita.NumeroFactura = numeroFactura;
                            jsonRequestFacilita.NumeroRecibo = 3611;
                            jsonRequestFacilita.NumeroCertificado = "";
                            jsonRequestFacilita.CodigoSucursal = codigoSucursal;
                            jsonRequestFacilita.Sucursal = codigoSucursal;
                            jsonRequestFacilita.Identificacion = identificacionCliente;//comprobante.CIDCONTRAPARTIDADIARIOGENERAL;
                            jsonRequestFacilita.MedioPago = medioPago;
                            jsonRequestFacilita.Prima = valorPrima;
                            jsonRequestFacilita.NumeroOperacion = numeroProducto;
                            jsonRequestFacilita.FechaPago = DateTime.Now;
                            jsonRequestFacilita.NumeroCuota = 1;

                            client = new RestClient(Baseurl);
                            var requestAsistencia = new RestRequest(Baseurl, Method.POST, DataFormat.Json);
                            requestAsistencia.AddParameter("application/json", JsonConvert.SerializeObject(jsonRequestFacilita), ParameterType.RequestBody);
                            var responseAsistencia = client.Post(requestAsistencia);
                            var contentAsistencia = responseAsistencia.Content;
                            var jsonRespuesta = JObject.Parse(contentAsistencia);
                            if (jsonRespuesta["codigoTransaccion"].ToString().Equals("200"))
                            {
                                RegistrarLogs(itemDocumento.SalesId, itemDocumento.SalesIdAccount, "101", "Dynamics", "Asistencia Facilita", JsonConvert.SerializeObject(jsonRequestFacilita), JsonConvert.SerializeObject(jsonRespuesta), string.Empty, "TRUE");
                            }
                            else
                            {
                                RegistrarLogs(itemDocumento.SalesId, itemDocumento.SalesIdAccount, "101", "Dynamics", "Asistencia Facilita", JsonConvert.SerializeObject(jsonRequestFacilita), JsonConvert.SerializeObject(jsonRespuesta), string.Empty, "FALSE");
                            }

                        }

                    }
                }


                //CONSUMO DE KIT MULTINOVA - INICIO

                //Validar si la fforma de pago es credito facilito
                foreach (var itemDocumento in listaDocumentos)
                {
                    var bandera = VerificarPedidoKitMultinova(itemDocumento.SalesIdAccount, itemDocumento.SalesId);
                    if (bandera)
                    {
                        foreach (var item in itemDocumento.documentInvoiceRequestLinesList)
                        {
                            NotificarFacturaPagadKitNova(item.InvoiceId, int.Parse(itemDocumento.SalesIdAccount), itemDocumento.SalesId);
                        }
                    }
                }
                //CONSUMO KIT MULTINOVA - FIN


                //CONSUMO DE BONOS - INICIO
                string codigoProducto = "";
                string numeroPedido = "";
                string ordenVenta = "";
                string informacionPedido = "";
                string nombreAlmacen = "";
                XmlDocument doc = new XmlDocument();
                foreach (var itemDocumento in listaDocumentos)
                {

                    //CONSULTANDO INFORMACIUON DEL PEDIDO
                    informacionPedido = ConsultarInfoPedido(itemDocumento.SalesIdAccount);
                    if (informacionPedido != "<Datos />")
                    {
                        XmlDocument docA = new XmlDocument();
                        docA.LoadXml(informacionPedido);
                        XElement xmldoc = XElement.Load(new XmlNodeReader(docA));

                        XmlNodeList list = docA.DocumentElement.GetElementsByTagName("Table");
                        if (list.Count != 0)
                        {
                            for (int i = 0; i < list[0].ChildNodes.Count; i++)
                            {
                                XmlNode child = list[0].ChildNodes[i];
                                if (child.Name.Equals("CCODIGOALMACEN"))
                                {
                                    nombreAlmacen = child.InnerText.ToString();
                                }

                            }
                        }
                    }

                    //CONSULTANDO LOS DATOS DE LA FACTURA
                    var datosFactura = ConsultaDatosFactura(itemDocumento.SalesId, itemDocumento.SalesIdAccount);

                    if (datosFactura.FormaPago.ToUpper().Equals("EFECTIVO"))
                        medioPago = "EF";
                    else
                        medioPago = "Plan Financiero";

                    //RRECORRIENDO ELL DETALLE DE LOS ITEMS DE LA FACTURA
                    foreach (var item in datosFactura.DetalleFacturacion)
                    {
                        foreach (var itemProducto in listaDocumentos)
                        {
                            numeroPedido = itemProducto.SalesId;
                            ordenVenta = itemProducto.SalesIdAccount;
                            foreach (var productos in itemProducto.documentInvoiceRequestLinesList)
                            {
                                foreach (var itemLinea in productos.itemList)
                                {
                                    foreach (var detalle in item.Producto)
                                    {
                                        valorPrima = detalle.SaldoFinanciar;
                                        if (detalle.CodigoProducto == itemLinea.itemId)
                                        {
                                            numeroFactura = productos.InvoiceId;
                                            codigoProducto = detalle.CodigoProducto;
                                            break;
                                        }
                                    }
                                }
                            }

                        }
                    }
                }

                Baseurl = System.Configuration.ConfigurationManager.AppSettings["Bono002"];

                dynamic aplicadoObject = new JObject();
                aplicadoObject.combos = true;
                aplicadoObject.descuentoEmpleado = true;
                aplicadoObject.repuesto = true;
                aplicadoObject.descuentos = true;
                aplicadoObject.cedula = true;
                aplicadoObject.servicio = true;
                aplicadoObject.billeteComprador = true;
                aplicadoObject.ruc = true;
                aplicadoObject.producto = true;
                aplicadoObject.precioNeto = true;
                aplicadoObject.regalo = true;
                aplicadoObject.precioNetoConIva = true;

                dynamic condicionObjeto = new JObject();
                condicionObjeto.tipo = "PRODUCTO";
                condicionObjeto.codigo = codigoProducto;

                dynamic formaPagoObjeto = new JObject();
                formaPagoObjeto.credito = true;
                formaPagoObjeto.efectivo = true;
                formaPagoObjeto.tarjeta = true;

                var jsonBonos = new
                {
                    almacen = nombreAlmacen,
                    condiciones = condicionObjeto,
                    aplicadoEn = aplicadoObject,
                    formaPago = formaPagoObjeto
                };

                client = new RestClient(Baseurl);
                var request = new RestRequest(Baseurl, Method.POST, DataFormat.Json);
                request.AddParameter("application/json", JsonConvert.SerializeObject(jsonBonos), ParameterType.RequestBody);
                var responseBono = client.Post(request);
                var contentBono = responseBono.Content;
                var jsonRespuestaBono = JObject.Parse(contentBono);
                if (jsonRespuestaBono["codigo"].ToString().Equals("200"))
                {
                    RegistrarLogs(ordenVenta, numeroPedido, "101", "Dynamics", "Asistencia Facilita", JsonConvert.SerializeObject(jsonBonos), JsonConvert.SerializeObject(jsonRespuestaBono), string.Empty, "TRUE");
                }
                else
                {
                    RegistrarLogs(ordenVenta, numeroPedido, "101", "Dynamics", "Asistencia Facilita", JsonConvert.SerializeObject(jsonBonos), JsonConvert.SerializeObject(jsonRespuestaBono), string.Empty, "FALSE");
                }

                //CONSUMO DE BONOS - FIN

                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StringContent("OK", Encoding.UTF8, "application/json");
                return response;

            }
            catch (Exception ex)
            {
                var jsonError = new
                {
                    error = true,
                    codigo = -1,
                    mensaje = ex.Message
                };
                var response = Request.CreateResponse(HttpStatusCode.BadRequest);
                response.Content = new StringContent(jsonError.ToString(), Encoding.UTF8, "application/json");
                return response;
            }

        }


        public bool VerificarPedidoKitMultinova(string numeroPedido, string ordenVenta)
        {
            bool resultado = false;

            using (var cliente = new HttpClient())
            {
                try
                {
                    string baseUrk = System.Configuration.ConfigurationManager.AppSettings["Multinova003"].ToString() + "/" + numeroPedido;
                    var respuesta = cliente.GetAsync(baseUrk).Result;
                    var contenido = respuesta.Content.ReadAsStringAsync();
                    var jsonRespuesta = JObject.Parse(contenido.Result);

                    if (int.Parse(jsonRespuesta["Codigo"].ToString()) == 200)
                    {
                        RegistrarLogs(ordenVenta, numeroPedido, "102", "Dynamics", "Verificar KitMultinova", baseUrk, JsonConvert.SerializeObject(jsonRespuesta), string.Empty, "TRUE");
                        var data = jsonRespuesta["Data"];
                        if (data["Existe"].ToString() == "true")
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        RegistrarLogs(ordenVenta, numeroPedido, "102", "Dynamics", "Verificar KitMultinova", baseUrk, JsonConvert.SerializeObject(jsonRespuesta), string.Empty, "FALSE");
                        return false;
                    }

                    return resultado;

                }
                catch (Exception errorInformacion)
                {
                    return false;
                }
            }
        }


        public JObject NotificarFacturaPagadKitNova(string numeroFactura, int numeroPedido, string ordenVenta)
        {
            using (var cliente = new HttpClient())
            {
                try
                {
                    string baseUrk = System.Configuration.ConfigurationManager.AppSettings["Multinova004"].ToString();

                    var jsonRequest = new
                    {
                        NumeroPedidoSiac = numeroPedido,
                        CodigoFactura = numeroFactura,
                        NumeroOrdenVenta = ordenVenta
                    };

                    RestClient client = new RestClient(baseUrk);
                    var request = new RestRequest(baseUrk, Method.POST, DataFormat.Json);
                    request.AddParameter("application/json", JsonConvert.SerializeObject(jsonRequest), ParameterType.RequestBody);
                    var responseBono = client.Post(request);
                    var contentBono = responseBono.Content;


                    var respuesta = cliente.GetAsync(baseUrk).Result;
                    var contenido = respuesta.Content.ReadAsStringAsync();
                    var jsonRespuesta = JObject.Parse(contenido.Result);

                    if (int.Parse(jsonRespuesta["Codigo"].ToString()) == 200)
                    {
                        RegistrarLogs(ordenVenta, numeroPedido.ToString(), "103", "Dynamics", "Verificar KitMultinova", baseUrk, JsonConvert.SerializeObject(jsonRespuesta), string.Empty, "TRUE");
                    }
                    else
                    {
                        RegistrarLogs(ordenVenta, numeroPedido.ToString(), "103", "Dynamics", "Verificar KitMultinova", baseUrk, JsonConvert.SerializeObject(jsonRespuesta), string.Empty, "FALSE");
                    }

                    return jsonRespuesta;

                }
                catch (Exception errorInformacion)
                {
                    var jsonError = new
                    {
                        error = true,
                        mensaje = string.Format("Mensaje : {0} ", errorInformacion.Message),
                    };

                    return JObject.Parse(jsonError.ToString());
                }
            }
        }


        private ConsultaDatosFacturaResponse ConsultaDatosFactura(string salesid, string salessiac)
        {
            ConsultaDatosFacturaResponse res = new ConsultaDatosFacturaResponse();
            string Baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlConsultaDatosFac"]; //"http://192.168.82.44:4966/tomapedido/api/orden-venta/ConsultaDatosFactura";

            ConsultaDatosFacturaRequest obj = new ConsultaDatosFacturaRequest()
            {
                numeroPedido = int.Parse(salessiac),
                SalesId = salesid,
            };
            using (var client = new HttpClient())
            {
                client.CancelPendingRequests();

                client.DefaultRequestHeaders.Clear();
                client.BaseAddress = new Uri(Baseurl);
                //client.Timeout = TimeSpan.FromSeconds(25);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var postTask = client.PostAsJsonAsync<ConsultaDatosFacturaRequest>("ConsultaDatosFactura", obj);
                try
                {
                    postTask.Wait();
                }
                catch (Exception ex)
                {
                    res.response = false;
                    res.messages = ex.Message;
                    return res;
                }

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync().Result;

                    res = JsonConvert.DeserializeObject<ConsultaDatosFacturaResponse>(readTask);
                    var jsonresp = JsonConvert.SerializeObject(res);
                    res.response = true;
                    res.messages = "OK";

                    return res;
                }

                return res;
            }
        }


        private bool RegistrarLogs(string ORDENVENTADYNAMICS, string PEDIDOSIAC, string TIPOTRANSACCION, string ORIGEN, string PROCESO, string REQUEST, string RESPONSE, string COMENTARIO, string ESTADO)
        {
            bool Retorno = false;
            XmlDocument xmlDoc = new XmlDocument();
            SQLConexion ObjCon = new SQLConexion();
            SqlCommand cmd = new SqlCommand();
            try
            {

                DataObject dobj = new DataObject();
                dobj.SW_ProductoName = "Desarrollo";
                dobj.SW_SPName = "dbo.PROC_CAJPFLUJO";
                dobj.SW_Base = "SIAC";
                dobj.SW_TimeOut = 0;

                List<DataParameters> par = new List<DataParameters>();

                par.Add(new DataParameters { SW_Parametro = "@ORDENVENTADYNAMICS", SW_Value = ORDENVENTADYNAMICS, SW_Size = 20, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@PEDIDOSIAC", SW_Value = PEDIDOSIAC, SW_Size = 50, SW_Type = DataParameters.SWDataType.Decimal });
                par.Add(new DataParameters { SW_Parametro = "@TIPOTRANSACCION", SW_Value = TIPOTRANSACCION, SW_Size = 25, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@ORIGEN", SW_Value = ORIGEN, SW_Size = 25, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@PROCESO", SW_Value = PROCESO, SW_Size = 50000, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@REQUEST", SW_Value = REQUEST, SW_Size = int.MaxValue, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@RESPONSE", SW_Value = RESPONSE, SW_Size = int.MaxValue, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@COMENTARIO", SW_Value = COMENTARIO, SW_Size = int.MaxValue, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@ESTADO", SW_Value = ESTADO, SW_Size = 10, SW_Type = DataParameters.SWDataType.String });
                dobj.SW_Parameters = par;
                Retorno = ObjCon.ExecutenNonQuery(dobj);
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
            return Retorno;
        }

        private string ConsultarInfoPedido(string numeroPedido)
        {
            bool Retorno = false;
            DataTable dt = new DataTable();
            XmlDocument xmlDoc = new XmlDocument();
            SQLConexion ObjCon = new SQLConexion();
            SqlCommand cmd = new SqlCommand();
            string strText = string.Empty;

            try
            {

                DataObject dobj = new DataObject();
                dobj.SW_ProductoName = "Desarrollo";
                dobj.SW_SPName = "dbo.PROC_CONSULTAINFOPEDIDOS";
                dobj.SW_Base = "SIAC";
                dobj.SW_TimeOut = 0;

                List<DataParameters> par = new List<DataParameters>();
                par.Add(new DataParameters { SW_Parametro = "@PEDIDOSIAC", SW_Value = numeroPedido, SW_Size = 50, SW_Type = DataParameters.SWDataType.Decimal });
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


    }
}
