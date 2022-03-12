using ApiModels;
using SqlConexion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using System.Xml.Linq;

namespace ApiBilleteComprador.Controllers
{
    public class BCController : ApiController
    {
        [HttpPost]
        [Route("api/ConsultaBCxPedido")]
        public async Task<BCPedidoResult> ConsultaBCPedido([FromBody] BCRequest datos)
        {
            BCPedidoResult resp = new BCPedidoResult();
            try
            {
                var res = BCConsultar(1, datos.NumeroPedido);

                if (res != "<Datos />")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(res);

                    XElement xmldoc = XElement.Load(new XmlNodeReader(doc));

                    var _res =
                    from xml in xmldoc.Descendants("Table")
                    select new BCPedidoResult
                    {
                        NumeroPedido = (string)xml.Element("INUMEROPEDIDOORIGEN"),
                        CantidadBC = (int)xml.Element("CANTIDAD")
                    };
                    var _res2 =
                    from xml in xmldoc.Descendants("Table1")
                    select new DetalleBC
                    {
                        NumeroBillete = (string)xml.Element("CNUMEROBILLETE"),
                        Usado = (string)xml.Element("USADO"),
                        FechaVencimiento = (string)xml.Element("DFECHAVENCIMIENTO"),
                        IdProvisional = (string)xml.Element("IIDASIENTOPROVISION"),
                        Monto = (decimal)xml.Element("IVALOR")

                    };

                    resp.NumeroPedido = _res.FirstOrDefault().NumeroPedido;
                    resp.CantidadBC= _res.FirstOrDefault().CantidadBC;

                    foreach (var item in _res2)
                    {
                        resp.DetallesBC.Add(new DetalleBC
                        {
                            NumeroBillete = item.NumeroBillete,
                            Usado = item.Usado,
                            FechaVencimiento = item.FechaVencimiento,
                            IdProvisional = item.IdProvisional,
                            Monto = item.Monto
                        });
                    }

                    resp.response = true;
                    resp.messages = "OK";
                    return resp;
                }
                else
                {
                    resp.response = false;
                    resp.messages = "SIN RESULTADOS";
                    return resp;
                }
            }
            catch (Exception ex)
            {
                resp.response = false;
                resp.messages = ex.Message;
                return resp;
            }
        }
        [HttpPost]
        [Route("api/ConsultarBC")]
        public async Task<BCResult> ConsultarBC([FromBody] BCRequest datos)
        {
            BCResult resp = new BCResult();
            try
            {
                var res = BCConsultar(2, datos.NumeroPedido);

                if (res != "<Datos />")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(res);

                    XElement xmldoc = XElement.Load(new XmlNodeReader(doc));

                    var _res =
                    from xml in xmldoc.Descendants("Table")
                    select new BCResult
                    {
                        NumeroBillete = (string)xml.Element("CNUMEROBILLETE"),
                        Usado = (string)xml.Element("USADO"),
                    };
                    resp.NumeroBillete = _res.FirstOrDefault().NumeroBillete;
                    resp.Usado = _res.FirstOrDefault().Usado;
                    resp.response = true;
                    resp.messages = "OK";
                    return resp;
                }
                else
                {
                    resp.response = false;
                    resp.messages = "SIN RESULTADOS";
                    return resp;
                }
            }
            catch (Exception ex)
            {
                resp.response = false;
                resp.messages = ex.Message;
                return resp;
            }
        }
        [HttpPost]
        [Route("api/registrarBC")]
        public async Task<BCResult> RegistrarBC([FromBody] BCRegistro datos)
        {
            BCResult resp = new BCResult();
            try
            {
                var res = BCRegistrar(datos.Cedula,datos.Valor,datos.NumeroPedido,datos.IdProvisional);

                if (res != "<Datos />")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(res);

                    XElement xmldoc = XElement.Load(new XmlNodeReader(doc));
                    
                    var _res =
                    from xml in xmldoc.Descendants("Table")
                    select new BCResult
                    {
                        NumeroBillete = (string)xml.Element("CNUMEROBILLETE"),
                        Cedula = (string)xml.Element("CCEDULA"),
                        Id = (string)xml.Element("ISECUENCIA"),
                        FechaVencimiento = (string)xml.Element("DFECHAVENCIMIENTO"),
                        Usado = "NO",
                    };

                    resp.NumeroBillete = _res.FirstOrDefault().NumeroBillete;
                    resp.Id = _res.FirstOrDefault().Id;
                    resp.Cedula = _res.FirstOrDefault().Cedula;
                    resp.FechaVencimiento = _res.FirstOrDefault().FechaVencimiento;
                    resp.Usado = _res.FirstOrDefault().Usado;
                    resp.response = true;
                    resp.messages = "Registrado Correctamente";
                    return resp;
                }
                else
                {
                    resp.response = false;
                    resp.messages = "Error al Registrar";
                    return resp;
                }
            }
            catch (Exception ex)
            {
                resp.response = false;
                resp.messages = ex.Message;
                return resp;
            }
        }

        [HttpPost]
        [Route("api/ActualizarBC")]
        public async Task<BCResult> ActualizarBC([FromBody] BCRequest datos)
        {
            BCResult resp = new BCResult();
            try
            {
                var res = BCActualizarUtilizado("U", datos.NumeroPedido);

                if (res != "<Datos />")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(res);

                    XElement xmldoc = XElement.Load(new XmlNodeReader(doc));

                    var _res =
                    from xml in xmldoc.Descendants("Table")
                    select new BCResult
                    {
                        NumeroBillete = (string)xml.Element("CNUMEROBILLETE")
                    };
                    resp.NumeroBillete = _res.FirstOrDefault().NumeroBillete;                
                    resp.response = true;
                    resp.messages = "OK";
                    return resp;
                }
                else
                {
                    resp.response = false;
                    resp.messages = "SIN RESULTADOS";
                    return resp;
                }
            }
            catch (Exception ex)
            {
                resp.response = false;
                resp.messages = ex.Message;
                return resp;
            }
        }

        [HttpPost]
        [Route("api/ActCamposBC")]
        public async Task<BCResult> ActCamposBC([FromBody] BCCamposActualizar datos)
        {
            BCResult resp = new BCResult();
            try
            {
                var res = BCActCampos(datos.NumeroFacturAplicado,datos.NumeroPedidoAplicado,datos.ValorNC,datos.NumeroNC,datos.Recibo,datos.NumeroPedidoOrigen);

                if (res != "<Datos />")
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(res);

                    XElement xmldoc = XElement.Load(new XmlNodeReader(doc));

                    var _res =
                    from xml in xmldoc.Descendants("Table")
                    select new BCResult
                    {
                        NumeroBillete = (string)xml.Element("CNUMEROBILLETE")
                    };
                    resp.NumeroBillete = _res.FirstOrDefault().NumeroBillete;
                    resp.response = true;
                    resp.messages = "OK";
                    return resp;
                }
                else
                {
                    resp.response = false;
                    resp.messages = "SIN RESULTADOS";
                    return resp;
                }
            }
            catch (Exception ex)
            {
                resp.response = false;
                resp.messages = ex.Message;
                return resp;
            }
        }



        private string BCConsultar(int tipo,string orden)
        {
            string strText = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            SQLConexion ObjCon = new SQLConexion();
            SqlCommand cmd = new SqlCommand();
            try
            {
                DataObject dobj = new DataObject();
                dobj.SW_ProductoName = "Desarrollo";
                dobj.SW_SPName = "dbo.BCCONSULTAS";
                dobj.SW_Base = "SIAC";
                dobj.SW_TimeOut = 0;

                List<DataParameters> par = new List<DataParameters>();

                par.Add(new DataParameters { SW_Parametro = "@tipo", SW_Value = tipo, SW_Size = 50, SW_Type = DataParameters.SWDataType.Int });
                par.Add(new DataParameters { SW_Parametro = "@orden", SW_Value = orden, SW_Size = 50, SW_Type = DataParameters.SWDataType.String });

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
        private string BCRegistrar(string cedula, string valor, string pedido, string asientoprov)
        {
            string strText = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            SQLConexion ObjCon = new SQLConexion();
            SqlCommand cmd = new SqlCommand();
            try
            {
                DataObject dobj = new DataObject();
                dobj.SW_ProductoName = "Desarrollo";
                dobj.SW_SPName = "dbo.SP_BC_PROVISIONAL";
                dobj.SW_Base = "SIAC";
                dobj.SW_TimeOut = 0;

                List<DataParameters> par = new List<DataParameters>();

                par.Add(new DataParameters { SW_Parametro = "@sCedula", SW_Value = cedula, SW_Size = 15, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@dValor", SW_Value = valor, SW_Size = 50, SW_Type = DataParameters.SWDataType.Decimal });
                par.Add(new DataParameters { SW_Parametro = "@dNumPedOrg", SW_Value = pedido, SW_Size = 50, SW_Type = DataParameters.SWDataType.Decimal });
                par.Add(new DataParameters { SW_Parametro = "@dNumAsiProv", SW_Value = asientoprov, SW_Size = 50, SW_Type = DataParameters.SWDataType.String });

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

        private string BCActualizarUtilizado(string estado, string orden)
        {
            string strText = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            SQLConexion ObjCon = new SQLConexion();
            SqlCommand cmd = new SqlCommand();
            try
            {
                DataObject dobj = new DataObject();
                dobj.SW_ProductoName = "Desarrollo";
                dobj.SW_SPName = "dbo.BCACTUALIZACION";
                dobj.SW_Base = "SIAC";
                dobj.SW_TimeOut = 0;

                List<DataParameters> par = new List<DataParameters>();

                par.Add(new DataParameters { SW_Parametro = "@estado", SW_Value = estado, SW_Size = 1, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@orden", SW_Value = orden, SW_Size = 50, SW_Type = DataParameters.SWDataType.String });

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

        private string BCActCampos( string NumeroFacturAplicado, decimal NumeroPedidoAplicado, decimal ValorNC, string NumeroNC, int Recibo, decimal NumeroPedidoOrigen )
        {
            string strText = string.Empty;
            XmlDocument xmlDoc = new XmlDocument();
            SQLConexion ObjCon = new SQLConexion();
            SqlCommand cmd = new SqlCommand();
            try
            {
                DataObject dobj = new DataObject();
                dobj.SW_ProductoName = "Desarrollo";
                dobj.SW_SPName = "dbo.BCACTUALIZACIONCAMPOS";
                dobj.SW_Base = "SIAC";
                dobj.SW_TimeOut = 0;

                List<DataParameters> par = new List<DataParameters>();

               
                par.Add(new DataParameters { SW_Parametro = "@numfactaplicado", SW_Value = NumeroFacturAplicado, SW_Size = 1, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@numpedidoaplicado", SW_Value = NumeroPedidoAplicado, SW_Size = 50, SW_Type = DataParameters.SWDataType.Decimal });
                par.Add(new DataParameters { SW_Parametro = "@valornc", SW_Value = ValorNC, SW_Size = 50, SW_Type = DataParameters.SWDataType.Decimal });
                par.Add(new DataParameters { SW_Parametro = "@numeronc", SW_Value = NumeroNC, SW_Size = 50, SW_Type = DataParameters.SWDataType.String });
                par.Add(new DataParameters { SW_Parametro = "@recibo", SW_Value = Recibo, SW_Size = 50, SW_Type = DataParameters.SWDataType.Int });
                par.Add(new DataParameters { SW_Parametro = "@numpedidoorigen", SW_Value = NumeroPedidoOrigen, SW_Size = 50, SW_Type = DataParameters.SWDataType.Decimal });

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
