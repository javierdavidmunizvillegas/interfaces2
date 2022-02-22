using ApiModels;
using Newtonsoft.Json;
using RestSharp;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace ApiFlujoCaja
{
    public class clsSIAC
    {
        public bool RegistrarLogs(FlujoRequest Datos,  string PROCESO, string REQUEST, string RESPONSE, string COMENTARIO, string ESTADO)
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

                par.Add(new DataParameters { SW_Parametro = "@ALMACEN", SW_Value = Datos.CodigoAlmacen, SW_Size = 20, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@CCODCAJA", SW_Value = Datos.CodigoCaja, SW_Size = 20, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@CCODUSUARIO", SW_Value = Datos.UsuarioIngreso, SW_Size = 20, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@ORDENVENTADYNAMICS", SW_Value = Datos.SalesID, SW_Size = 20, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@PEDIDOSIAC", SW_Value = Datos.SalesIDSiac, SW_Size = 50, SW_Type = DataParameters.SWDataType.Decimal });
                par.Add(new DataParameters { SW_Parametro = "@NUMERODERECIBO", SW_Value = Datos.NumeroRecibo, SW_Size = 50, SW_Type = DataParameters.SWDataType.Decimal });
                par.Add(new DataParameters { SW_Parametro = "@ORIGEN", SW_Value = Datos.OrigenTransaccion, SW_Size = 25, SW_Type = DataParameters.SWDataType.String });
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
        public bool ConsultaLogs(FlujoRequest Datos, string PROCESO, ref string RESPONSE)
        {
            bool Retorno = false;
            XmlDocument xmlDoc = new XmlDocument();
            SQLConexion ObjCon = new SQLConexion();
            SqlCommand cmd = new SqlCommand();
            try
            {

                DataObject dobj = new DataObject();
                dobj.SW_ProductoName = "Desarrollo";
                dobj.SW_SPName = "dbo.PROC_CONSULTACAJPFLUJO";
                dobj.SW_Base = "SIAC";
                dobj.SW_TimeOut = 0;

                List<DataParameters> par = new List<DataParameters>();

                par.Add(new DataParameters { SW_Parametro = "@CCODCAJA", SW_Value = Datos.CodigoCaja, SW_Size = 20, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@CCODUSUARIO", SW_Value = Datos.UsuarioIngreso, SW_Size = 20, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@NUMERODERECIBO", SW_Value = Datos.NumeroRecibo, SW_Size = 50, SW_Type = DataParameters.SWDataType.Decimal });
                par.Add(new DataParameters { SW_Parametro = "@ORDENVENTADYNAMICS", SW_Value = Datos.SalesID, SW_Size = 20, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@PEDIDOSIAC", SW_Value = Datos.SalesIDSiac, SW_Size = 50, SW_Type = DataParameters.SWDataType.Decimal });
                par.Add(new DataParameters { SW_Parametro = "@ORIGEN", SW_Value = Datos.OrigenTransaccion, SW_Size = 25, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@PROCESO", SW_Value = PROCESO, SW_Size = 50000, SW_Type = DataParameters.SWDataType.String }); dobj.SW_Parameters = par;
                dobj.SW_Parameters = par;
                DataSet ds = new DataSet();
                ds = ObjCon.Execute(dobj);

                if (ds != null)
                {
                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            RESPONSE = ds.Tables[0].Rows[0]["RESPONSE"].ToString();
                            Retorno = Convert.ToBoolean(ds.Tables[0].Rows[0]["ESTADO"].ToString());
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
            return Retorno;
        }
        private string ConsultaCajFLujo(int tipo, string salesid)
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
                par.Add(new DataParameters { SW_Parametro = "@Salessiac", SW_Value = salesid, SW_Size = 50, SW_Type = DataParameters.SWDataType.String });

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
        public ConsultaDatosFacturaResponse ConsultaDatosFactura(string salesid, string salessiac)
        {
            ConsultaDatosFacturaResponse res = new ConsultaDatosFacturaResponse();
            try
            {
                string baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlConsultaDatosFac"];
                var clients = new RestClient(baseurl);
                clients.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                ConsultaDatosFacturaRequest objs = new ConsultaDatosFacturaRequest()
                {
                    numeroPedido = Convert.ToInt32(salessiac),
                    SalesId = salesid,
                };
                var body = JsonConvert.SerializeObject(objs);
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = clients.Execute(request);
                var content = response.Content;
                res = JsonConvert.DeserializeObject<ConsultaDatosFacturaResponse>(content);
                res.Estado = true;
                res.Mensaje = "OK";
                return res;
            }
            catch (Exception ex)
            {
                res.Estado = false;
                res.Mensaje = ex.Message;
                return res;
            }
        }
        public CajaItemsFlujo GetItems(string salesid)
        {
            CajaItemsFlujo obj = new CajaItemsFlujo();
            try
            {
                var res = ConsultaCajFLujo(7, salesid);

                if (res != "<Datos />")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(res);

                    XElement xmldoc = XElement.Load(new XmlNodeReader(doc));
                    var _cab =
                    from xml in xmldoc.Descendants("Table")
                    select new Cabecera
                    {
                        CustAccount = (string)xml.Element("CustAccount"),
                        SalesId = (string)xml.Element("SalesId"),
                        SalesOrigin = (string)xml.Element("SalesOrigin"),
                        PostingProfile = (string)xml.Element("PostingProfile"),
                        NumeroPedido = (string)xml.Element("NumeroPedido"),
                        SecuenciaGroup = (int)xml.Element("SecuenciaGroup"),
                    };
                    var _items =
                    from xml in xmldoc.Descendants("Table1")
                    select new Detalle
                    {
                        NumeroPedido = (string)xml.Element("NumeroPedido"),
                        SecuenciaGroup = (int)xml.Element("SecuenciaGroup"),
                        ItemId = (string)xml.Element("ItemId"),
                        Serial = (string)xml.Element("Serial"),
                        Cantidad = (int)xml.Element("Cantidad"),
                        EsMoto = (string)xml.Element("EsMoto"),
                    };
                    var _fin =
                    from xml in xmldoc.Descendants("Table2")
                    select new CajaItemsFlujo
                    {
                        EsMoto = (int)xml.Element("EsMoto"),
                    };

                    foreach (var item in _cab)
                    {
                        obj.Cabecera.Add(new Cabecera
                        {
                            CustAccount = item.CustAccount,
                            SalesId = item.SalesId,
                            SalesOrigin = item.SalesOrigin,
                            PostingProfile = item.PostingProfile,
                            NumeroPedido = item.NumeroPedido,
                            SecuenciaGroup = item.SecuenciaGroup
                        });
                    }
                    foreach (var item in _items)
                    {
                        obj.Detalle.Add(new Detalle
                        {
                            NumeroPedido = item.NumeroPedido,
                            SecuenciaGroup = item.SecuenciaGroup,
                            ItemId = item.ItemId,
                            Serial = item.Serial,
                            Cantidad = item.Cantidad,
                            EsMoto = item.EsMoto
                        });
                    }
                    obj.EsMoto = _fin.FirstOrDefault().EsMoto;
                }

                return obj;
            }
            catch (Exception e)
            {
                return obj;
            }

        }
        public string NumeroSecuencia(string Tienda, string Tipo)
        {
            string Secuencia = string.Empty;
            try
            {
                const string Comillas = "\"";
                string baseurl = System.Configuration.ConfigurationManager.AppSettings["UrlNumeroSecuencia"];
                var client = new RestClient(baseurl);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                var body = @"[" + "\n" + @"   " + Comillas + Tienda + Comillas + "" + "\n" + @"]";
                request.AddParameter("application/json", body, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                var content = response.Content;
                var resultado = JsonConvert.DeserializeObject<SecuenciaResponse>(content);
                if (resultado != null)
                {
                    foreach (AxTiendaGrupoSecuenciaDto Respuesta in resultado.response)
                    {
                        switch (Tipo)
                        {
                            case "FAE":
                                Secuencia = Respuesta.FacturaElectronica;
                                break;
                            case "NCE":
                                Secuencia = Respuesta.NotaCreditoFiscal;
                                break;
                            case "NCI":
                                Secuencia = Respuesta.NotaCreditoInterna;
                                break;
                            case "NCB":
                                Secuencia = Respuesta.NotaCreditoBilleteComprador;
                                break;
                            default:
                                break;
                        }
                        break;
                    }
                }
                return Secuencia;
            }
            catch (Exception Ex)
            {
                throw;
            }
        }
        public DataTable ConsultaFormaPagoPedidos(object PEDIDOSIAC, object RECIBO)
        {
            DataTable Dt = null;
            XmlDocument xmlDoc = new XmlDocument();
            SQLConexion ObjCon = new SQLConexion();
            SqlCommand cmd = new SqlCommand();
            try
            {

                DataObject dobj = new DataObject();
                dobj.SW_ProductoName = "Desarrollo";
                dobj.SW_SPName = "dbo.PROC_CONSULTACAJRECIBOROMAPAGO";
                dobj.SW_Base = "SIAC";
                dobj.SW_TimeOut = 0;

                List<DataParameters> par = new List<DataParameters>();

                par.Add(new DataParameters { SW_Parametro = "@PEDIDOSIAC", SW_Value = PEDIDOSIAC, SW_Size = 50, SW_Type = DataParameters.SWDataType.Decimal });
                par.Add(new DataParameters { SW_Parametro = "@RECIBO", SW_Value = RECIBO, SW_Size = 50, SW_Type = DataParameters.SWDataType.Decimal });
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
    }
}