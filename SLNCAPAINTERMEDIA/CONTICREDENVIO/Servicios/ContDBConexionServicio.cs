using DataBaseAccess;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using CONTICREDENVIO.Modelos;
using System.Threading.Tasks;
using System.Globalization;
using System.Data.SqlClient;

namespace CONTICREDENVIO.Servicios
{
    public interface IContDBConexionServicio
    {
        Parametro LeerParametros();
        List<Producto> LeerProductos();
        string ProcUnionProductosCalif();
        string ProcValidaInicio();
        string ProcActualizaEnvio();
        string ProcRespaldaAhistoricos();
    }
    public class ContDBConexionServicio : IContDBConexionServicio
    {
        private readonly IConfigurationRoot _config;
        private string Coneccion1 = null;
        private string Coneccion2 = null;
        private IContLOG _contLog;
        public ContDBConexionServicio(IConfigurationRoot config ,IContLOG contLOG)
        {
            _config = config;
            Coneccion1 = _config.GetSection("ConnectionString1").Value;
            Coneccion2 = _config.GetSection("ConnectionString2").Value;
            _contLog = contLOG;
        }
        public Parametro LeerParametros()
        {
            
            IDataLayer dal = DataLayer.GetInstance(DatabaseTypes.MSSql, Coneccion1);
            dal.Sql = "SELECT  p.CDESCRIPCION,p.CDESCRIPCIONCORTA FROM VW_ContiGenPClasificador p  order by CCODIGO ";
            DataTable _dt = dal.ExecuteDataTable();
            dal.ExecuteDataReader();

            Parametro Param = new Parametro();
           
            string dateAndTime = DateTime.Now.ToString("yyyy-MM-dd");
            Param.EffectiveDate = dateAndTime; // "2021-09-29T16:02:38.253Z";//Convert.ToDateTime(dateAndTime);//DateTime.ParseExact(dateAndTime, "yyyy-MM-dd h:mm tt", provider);  //"2021-09-24T00:00:00";//Convert.ToDateTime("2021-09-29T16:02:38.253Z"); // de donde tomo fecha
            string valor = string.Empty;
            string descripcion = string.Empty;
            foreach (DataRow fila in _dt.Rows)
            {
                valor = fila["CDESCRIPCIONCORTA"].ToString(); //"0014"; //
                descripcion = fila["CDESCRIPCION"].ToString();

                if (descripcion == "RequestType")
                    Param.RequestType = Convert.ToInt32(valor); 
                if (descripcion == "CustAccount")
                    Param.CustAccount = valor; 
                if (descripcion == "Quantity")
                    Param.Quantity = Convert.ToInt32(valor); ;
                if (descripcion == "ManualPrice")
                    Param.ManualPrice = Convert.ToInt32(valor); 
                if (descripcion == "Canal") 
                    Param.AttString = valor ;
                if (descripcion == "Plazo")
                    Param.AttString = Param.AttString+","+ valor;
                if (descripcion == "ClfClte")
                    Param.AttString = Param.AttString + "," + valor;

            }
            dal = null;
            return Param;
        }


        public List<Producto> LeerProductos()
        {

            IDataLayer dal = DataLayer.GetInstance(DatabaseTypes.MSSql, Coneccion1);
            dal.Sql = "SPP_Contingencia_LeeTablaProductos";
            dal.CommandType = CommandType.StoredProcedure;
            // dal.ExecuteScalar();

            // ant IDataLayer dal = DataLayer.GetInstance(DatabaseTypes.MSSql, Coneccion1);
            // dal.Sql = "SELECT  P.APDataAreaId,P.RegisterID , P.ItemId , P.inventSerialId, CASE A.Name WHEN 'Liberado a UN Crédito' THEN 'UN_Creditos' WHEN 'Liberado a UN Dismayor' THEN 'UN_Dismayor' END as name FROM FlintFoxProductWork P LEFT JOIN FlintFoxAttributes A ON P.ItemId = A.ItemId and Name in ('Liberado a UN Crédito', 'Liberado a UN Dismayor') where P.itemid in ('0014','AR24TRHQDWK','H-001-P','4655','GPTOU020SABWW-P','10000385','MHC-M80D','20000939','CL351','0074','10000385','SONYA-30','JZ2350-S-1','DG-TVLS49D1B-7','ES-HERO-15','SONYA-30','DG-TV65S73U','SONYA-30','310-20IAPI')";
            // dal.Sql = "SELECT  P.APDataAreaId,P.RegisterID , P.ItemId , P.inventSerialId, CASE A.Name WHEN 'Liberado a UN Crédito' THEN 'UN_Creditos' WHEN 'Liberado a UN Dismayor' THEN 'UN_Dismayor' END as name FROM FlintFoxProductWork P LEFT JOIN FlintFoxAttributes A ON P.ItemId = A.ItemId and Name in ('Liberado a UN Crédito', 'Liberado a UN Dismayor') "; //where P.itemid in ('0014','AR24TRHQDWK','H-001-P','4655','GPTOU020SABWW-P','10000385','MHC-M80D','20000939','CL351','0074','10000385','SONYA-30','JZ2350-S-1','DG-TVLS49D1B-7','ES-HERO-15','SONYA-30','DG-TV65S73U','SONYA-30')";
            // ant dal.Sql = "SELECT  P.APDataAreaId,P.RegisterID , P.ItemId , P.inventSerialId, CASE A.Name WHEN 'Liberado a UN Crédito' THEN 'UN_Creditos' WHEN 'Liberado a UN Dismayor' THEN 'UN_Dismayor' END as name FROM FlintFoxProductWork P LEFT JOIN FlintFoxAttributes A ON P.ItemId = A.ItemId and Name in ('Liberado a UN Crédito', 'Liberado a UN Dismayor')  where P.itemid in (SELECT CODIGO FROM ACTUALIZAPRECIOSULT)";
            //ant DataTable _dt = dal.ExecuteDataTable();

            DataTable _dt = dal.ExecuteDataTable();
            dal.ExecuteDataReader();
            List<Producto> ProductList = null;
            ProductList = new List<Producto>();
            Producto Prod = null;
            foreach (DataRow fila in _dt.Rows)
            {
                Prod = new Producto();
                Prod.Itemid =  fila["ItemId"].ToString(); //"0014"; //
                Prod.DataAreaId = fila["ApdataAreaId"].ToString();
                Prod.RegisterID = fila["RegisterID"].ToString();
                Prod.Serie = fila["inventSerialId"].ToString();
                Prod.EstadoInv = fila["name"].ToString();
                ProductList.Add(Prod);

            }
            dal = null;
            
            return ProductList;
        }
        public string ProcUnionProductosCalif()
        {
            IDataLayer dal = DataLayer.GetInstance(DatabaseTypes.MSSql, Coneccion1);
            dal.Sql = "SPP_Contingencia2";
            dal.CommandType = CommandType.StoredProcedure;
            IDbDataParameter error = dal.AddParameter("Error",
                                    DbType.String, ParameterDirection.Output);
            error.Size = 2000;
            dal.ExecuteScalar();
            string respuesta = error.Value.ToString();
            if (respuesta == null || respuesta == "")
                respuesta = "Finaliza union de productos calificados con éxito";
            _contLog.FileLogger("APIVTA018002", "ProcUnionProductosCalif: " + respuesta);
            return respuesta;

        }
        public string ProcValidaInicio()
        {
            IDataLayer dal = DataLayer.GetInstance(DatabaseTypes.MSSql, Coneccion1);
            dal.Sql = "SPP_Contingencia_ValidaInicioEnvio";
            dal.CommandType = CommandType.StoredProcedure;
            IDbDataParameter error = dal.AddParameter("Error",
                                    DbType.String, ParameterDirection.Output);
            error.Size = 2000;
            IDbDataParameter EstadoDescarga = dal.AddParameter("EstadoDescarga",
                                    DbType.String, ParameterDirection.Output);
            EstadoDescarga.Size = 2000;
            dal.ExecuteScalar();
            string respuesta = error.Value.ToString();
            string EstadoDescargaR = EstadoDescarga.Value.ToString();
            if (respuesta == null || respuesta == "")
                respuesta = "Valida Inicia Proceso Envio";
            _contLog.FileLogger("APIVTA018002", "ProcValidaInicio: " + respuesta);
            if (EstadoDescargaR == "T") //ya termino descarga, procedo con la contigencia
                EstadoDescargaR = "SI";
            else
                EstadoDescargaR = "NO";

            return EstadoDescargaR;
        }
        public string ProcActualizaEnvio()
        {
            IDataLayer dal = DataLayer.GetInstance(DatabaseTypes.MSSql, Coneccion1);
            dal.Sql = "SPP_Contingencia_ActualizaEnvio";
            dal.CommandType = CommandType.StoredProcedure;
            IDbDataParameter error = dal.AddParameter("Error",
                                    DbType.String, ParameterDirection.Output);
            error.Size = 2000;
            dal.ExecuteScalar();
            string respuesta = error.Value.ToString();
            if (respuesta == null || respuesta == "")
                respuesta = "Finaliza y actualiza estado de tabla de proceso";
            _contLog.FileLogger("APIVTA018002", "ProcActualizaEnvio: " + respuesta);
            return respuesta;

        }
        public string ProcRespaldaAhistoricos()
        {
            IDataLayer dal = DataLayer.GetInstance(DatabaseTypes.MSSql, Coneccion1);
            dal.Sql = "SPP_Contingencia_RespaldoPVPMAX_DESCUENTOS";
            dal.CommandType = CommandType.StoredProcedure;
            IDbDataParameter error = dal.AddParameter("Error",
                                    DbType.String, ParameterDirection.Output);
            error.Size = 2000;
            dal.ExecuteScalar();
            string respuesta = error.Value.ToString();
            if (respuesta == null || respuesta == "")
                respuesta = "Finaliza y actualiza estado de tabla de proceso";
            _contLog.FileLogger("APIVTA018002", "ProcRespaldaAhistoricos: " + respuesta);
            return respuesta;

        }
    }
}
