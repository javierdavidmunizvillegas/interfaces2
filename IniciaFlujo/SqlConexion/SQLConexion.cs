using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConexion
{
    public class SQLConexion
    {

        private System.Data.SqlClient.SqlConnection _SQLConexion;
        private System.Data.SqlClient.SqlTransaction _SQLTransaccion;
        private string _Key = "";

        private DataObject _DataObjectConf;
        public DataObject DataObjectConf
        {
            get { return _DataObjectConf; }
            set { _DataObjectConf = value; }
        }

        public SQLConexion()
        {
            try
            {
                _Key = "Default";
            }
            catch { }
        }

        public SQLConexion(string Key)
        {
            try
            {
                _Key = Key;
            }
            catch { }
        }

        private string _HostName;
        public string HostName
        {
            get { return _HostName; }
        }

        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
        }

        private bool _ConexionDeConfianza;
        public bool ConexionDeConfianza
        {
            get { return _ConexionDeConfianza; }
        }

        private string _Base = string.Empty;
        public string Base
        {
            get { return _Base; }
            set { _Base = value; }
        }

        private int _TiempoDeEspera = 15;
        public int TiempoDeEspera
        {
            get { return this._TiempoDeEspera; }
            set { this._TiempoDeEspera = value; }
        }

        public string ObtenerCadenaConexion(string ArchivoXML)
        {
            return this._ObtenerCadenaConexion(ArchivoXML);
        }

        public string ObtenerCadenaConexion()
        {
            return this._ObtenerCadenaConexion(_Key);
        }

        private string _ObtenerCadenaConexion(string conexion)
        {
            string loConnectionString = null;

            try
            {
                loConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[conexion].ConnectionString;
                loConnectionString = loConnectionString.Replace("DefaultDB", Base);

            }
            catch (Exception ex)
            { }

            return loConnectionString;
        }

        public System.Data.SqlClient.SqlConnection Conectar()
        {
            try
            {
                this._SQLConexion = new System.Data.SqlClient.SqlConnection(ObtenerCadenaConexion());
                this._SQLConexion.Open();
                return this._SQLConexion;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public System.Data.SqlClient.SqlTransaction IniciarTransaccion(System.Data.SqlClient.SqlTransaction SQLTransaccion)
        {

            try
            {
                SQLTransaccion = this._SQLConexion.BeginTransaction();
            }
            catch
            {
                throw;
            }

            return SQLTransaccion;
        }

        public bool IniciarTransaccion()
        {
            bool Resultado = false;

            try
            {
                this._SQLTransaccion = this._SQLConexion.BeginTransaction();
                Resultado = true;
            }
            catch
            {
                Resultado = false;
                throw;
            }

            return Resultado;
        }

        public void Commit(System.Data.SqlClient.SqlTransaction SQLTransaccion)
        {
            try
            {
                SQLTransaccion.Commit();
                SQLTransaccion = null;
            }
            catch
            {
                throw;
            }
        }

        public void Commit()
        {
            try
            {
                this._SQLTransaccion.Commit();
                this._SQLTransaccion = null;
            }
            catch
            {
                throw;
            }
        }

        public void Rollback(System.Data.SqlClient.SqlTransaction SQLTransaccion)
        {
            try
            {
                if ((SQLTransaccion != null))
                {
                    SQLTransaccion.Rollback();
                    SQLTransaccion = null;
                }
            }
            catch
            {
                throw;
            }
        }

        public void Rollback()
        {
            try
            {
                if ((_SQLTransaccion != null))
                {
                    this._SQLTransaccion.Rollback();
                    this._SQLTransaccion = null;
                }

            }
            catch
            {
                throw;
            }
        }

        public void Close()
        {
            try
            {
                var _with1 = this._SQLConexion;
                _with1.Close();
            }
            catch
            {
                throw;
            }
        }

        public bool EstadoConexion()
        {
            try
            {
                if ((this._SQLConexion != null) && (this._SQLConexion.State == ConnectionState.Open))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }

        public SqlDataReader ExecuteReader(string SQL)
        {
            SqlDataReader DataReader = default(SqlDataReader);
            SqlCommand SQLComm = default(SqlCommand);

            try
            {
                this.Conectar();

                SQLComm = new SqlCommand(SQL, this._SQLConexion);
                DataReader = SQLComm.ExecuteReader();

                return DataReader;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void TimeOutSqlCommand(ref SqlCommand Cm, string Product)
        {
            string Archivo = "";
            try
            {
                if (Cm != null)
                {
                    DataSet Ds = new DataSet();
                    DataTable Dt = new DataTable();
                    int TimeOut = 0;

                    Archivo = System.Environment.GetEnvironmentVariable("windir") + "\\Configuration.xml";
                    Ds.ReadXml(Archivo);
                    Dt = Ds.Tables[0];
                    Dt.DefaultView.RowFilter = "Product='" + Product + "'";

                    TimeOut = int.Parse(Dt.Rows[0]["TimeOut"].ToString());

                    Cm.CommandTimeout = TimeOut * 1000;
                }

            }
            catch (Exception ex)
            {
            }

        }

        public DataSet Execute(DataObject DO)
        {
            DataSet Ds = new DataSet("Datos");
            try
            {
                Ds.Clear();
                _Key = DO.SW_ProductoName;
                SqlCommand ObjCm = new SqlCommand();
                SqlDataAdapter ObjDA = new SqlDataAdapter();

                try
                {
                    _Base = DO.SW_Base;
                    ObjCm.CommandText = DO.SW_SPName;
                    ObjCm.CommandType = CommandType.StoredProcedure;
                    ObjCm.Parameters.Clear();

                    if (DO.SW_Parameters != null)
                    {

                        foreach (DataParameters Dp in DO.SW_Parameters)
                        {
                            switch (Dp.SW_Type.ToString())
                            {
                                case "Int":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.Int).Value = Dp.SW_Value;
                                    break;
                                case "String":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.VarChar, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "Date":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.DateTime).Value = Dp.SW_Value;
                                    break;
                                case "Double":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.Float, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "Decimal":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.Decimal, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "GUI":
                                case "Gui":
                                    if (Dp.SW_Value != null && Dp.SW_Value.ToString() != "")
                                    {
                                        ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.UniqueIdentifier).Value = new Guid(Dp.SW_Value.ToString());
                                    }
                                    else
                                    {
                                        ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.UniqueIdentifier).Value = DBNull.Value;
                                    }
                                    break;
                            }

                            if (Dp.SW_Direction.ToUpper() == "O" || Dp.SW_Direction.ToUpper() == "OUT" || Dp.SW_Direction.ToUpper() == "OUTPUT")
                            {
                                ObjCm.Parameters[Dp.SW_Parametro].Direction = ParameterDirection.Output;
                            }
                        }
                    }

                    ObjCm.Connection = Conectar();
                    ObjCm.CommandTimeout = DO.SW_TimeOut;
                    ObjDA.SelectCommand = ObjCm;

                    ObjDA.Fill(Ds);
                    
                    if (Ds != null && Ds.Tables.Count > 0)
                    {
                        DataTable Dt = Ds.Tables[0];
                    }
                    else
                    {
                        Ds = null;
                    }
                    foreach (DataParameters Dp in DO.SW_Parameters)
                    {
                        if (Dp.SW_Direction.ToUpper() == "O" || Dp.SW_Direction.ToUpper() == "OUT" || Dp.SW_Direction.ToUpper() == "OUTPUT")
                        {
                            ObjCm.Parameters[Dp.SW_Parametro].Direction = ParameterDirection.Output;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (EstadoConexion() == true)
                    {
                        Close();
                    }

                    if (ObjCm != null)
                    {
                        ObjCm.Connection = null;
                    }
                    ObjCm = null;
                }
            }
            catch (Exception ex) { }
            return Ds;
        }
        public bool ExecutenNonQuery(DataObject DO)
        {
            bool Retorno = false;
            try
            {
                _Key = DO.SW_ProductoName;
                SqlCommand ObjCm = new SqlCommand();
                SqlDataAdapter ObjDA = new SqlDataAdapter();

                try
                {
                    _Base = DO.SW_Base;
                    ObjCm.CommandText = DO.SW_SPName;
                    ObjCm.CommandType = CommandType.StoredProcedure;
                    ObjCm.Parameters.Clear();

                    if (DO.SW_Parameters != null)
                    {

                        foreach (DataParameters Dp in DO.SW_Parameters)
                        {
                            switch (Dp.SW_Type.ToString())
                            {
                                case "Int":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.Int).Value = Dp.SW_Value;
                                    break;
                                case "String":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.VarChar, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "Date":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.DateTime).Value = Dp.SW_Value;
                                    break;
                                case "Double":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.Float, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "Decimal":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.Decimal, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "GUI":
                                case "Gui":
                                    if (Dp.SW_Value != null && Dp.SW_Value.ToString() != "")
                                    {
                                        ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.UniqueIdentifier).Value = new Guid(Dp.SW_Value.ToString());
                                    }
                                    else
                                    {
                                        ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.UniqueIdentifier).Value = DBNull.Value;
                                    }
                                    break;
                            }

                            if (Dp.SW_Direction.ToUpper() == "O" || Dp.SW_Direction.ToUpper() == "OUT" || Dp.SW_Direction.ToUpper() == "OUTPUT")
                            {
                                ObjCm.Parameters[Dp.SW_Parametro].Direction = ParameterDirection.Output;
                            }
                        }
                    }

                    ObjCm.Connection = Conectar();
                    ObjCm.CommandTimeout = DO.SW_TimeOut;
                    ObjCm.ExecuteNonQuery();
                    Retorno = true;

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (EstadoConexion() == true)
                    {
                        Close();
                    }

                    if (ObjCm != null)
                    {
                        ObjCm.Connection = null;
                    }
                    ObjCm = null;
                }
            }
            catch (Exception ex) { }
            return Retorno;
        }

        public DataSet ExecuteOut(ref DataObject DO)
        {
            DataSet Ds = new DataSet("Datos");
            try
            {
                Ds.Clear();
                _Key = DO.SW_ProductoName;
                SqlCommand ObjCm = new SqlCommand();
                SqlDataAdapter ObjDA = new SqlDataAdapter();

                try
                {

                    _Base = DO.SW_Base;

                    ObjCm.CommandText = DO.SW_SPName;
                    ObjCm.CommandType = CommandType.StoredProcedure;

                    ObjCm.Parameters.Clear();

                    if (DO.SW_Parameters != null)
                    {

                        foreach (DataParameters Dp in DO.SW_Parameters)
                        {
                            switch (Dp.SW_Type.ToString())
                            {
                                case "Int":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.Int, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "String":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.VarChar, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "Date":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.DateTime, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "Double":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.Float, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "Decimal":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.Decimal, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "GUI":
                                case "Gui":
                                    if (Dp.SW_Value != null && Dp.SW_Value.ToString() != "")
                                    {
                                        ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.UniqueIdentifier).Value = new Guid(Dp.SW_Value.ToString());
                                    }
                                    else
                                    {
                                        ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.UniqueIdentifier).Value = DBNull.Value;
                                    }
                                    break;
                            }

                            if (Dp.SW_Direction.ToUpper() == "O" || Dp.SW_Direction.ToUpper() == "OUT" || Dp.SW_Direction.ToUpper() == "OUTPUT")
                            {
                                ObjCm.Parameters[Dp.SW_Parametro].Direction = ParameterDirection.Output;
                            }
                        }
                    }

                    ObjCm.Connection = Conectar();
                    ObjCm.CommandTimeout = DO.SW_TimeOut;
                    ObjDA.SelectCommand = ObjCm;

                    ObjDA.Fill(Ds);

                    if (Ds != null && Ds.Tables.Count > 0 /*&& Ds.Tables[0].Rows.Count > 0*/)
                    {
                        DataTable Dt = Ds.Tables[0];
                    }
                    else
                    {
                        Ds = null;
                    }

                    foreach (DataParameters Dp in DO.SW_Parameters)
                    {
                        if (Dp.SW_Direction.ToUpper() == "O" || Dp.SW_Direction.ToUpper() == "OUT" || Dp.SW_Direction.ToUpper() == "OUTPUT")
                        {
                            Dp.SW_Value = ObjCm.Parameters[Dp.SW_Parametro].Value.ToString();

                            if (Dp.SW_Parametro == "@CodResultado")
                            {
                                DO.CodResultado = Dp.SW_Value.ToString();
                            }
                            if (Dp.SW_Parametro == "@Mensaje")
                            {
                                DO.Mensaje = Dp.SW_Value.ToString();
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
                    if (EstadoConexion() == true)
                    {
                        Close();
                    }

                    if (ObjCm != null)
                    {
                        ObjCm.Connection = null;
                    }
                    ObjCm = null;
                }
            }
            catch (Exception ex)
            {
            }
            return Ds;
        }


        public void ExecuteNoQuery()
        {
            DataSet Ds = new DataSet("Datos");
            try
            {
                Ds.Clear();

                if (_DataObjectConf.SW_ProductoName != null)
                {
                    _Key = _DataObjectConf.SW_ProductoName;
                }
                SqlCommand ObjCm = new SqlCommand();
                SqlDataAdapter ObjDA = new SqlDataAdapter();

                try
                {
                    _Base = _DataObjectConf.SW_Base;

                    ObjCm.CommandText = _DataObjectConf.SW_SPName;
                    ObjCm.CommandType = CommandType.StoredProcedure;

                    ObjCm.Parameters.Clear();

                    if (_DataObjectConf.SW_Parameters != null)
                    {

                        foreach (DataParameters Dp in _DataObjectConf.SW_Parameters)
                        {
                            switch (Dp.SW_Type.ToString())
                            {
                                case "Int":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.Int, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "String":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.VarChar, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "Date":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.DateTime, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "Double":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.Float, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;
                                case "Decimal":
                                    ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.Decimal, Dp.SW_Size).Value = Dp.SW_Value;
                                    break;

                                case "GUI":
                                case "Gui":
                                    if (Dp.SW_Value != null && Dp.SW_Value.ToString() != "")
                                    {
                                        ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.UniqueIdentifier).Value = new Guid(Dp.SW_Value.ToString());
                                    }
                                    else
                                    {
                                        ObjCm.Parameters.Add(Dp.SW_Parametro, SqlDbType.UniqueIdentifier).Value = DBNull.Value;
                                    }
                                    break;

                            }

                            if (Dp.SW_Direction != null && (Dp.SW_Direction.ToUpper() == "O" || Dp.SW_Direction.ToUpper() == "OUT" || Dp.SW_Direction.ToUpper() == "OUTPUT"))
                            {
                                ObjCm.Parameters[Dp.SW_Parametro].Direction = ParameterDirection.Output;
                            }
                        }


                    }

                    ObjCm.Connection = Conectar();
                    ObjCm.CommandTimeout = _DataObjectConf.SW_TimeOut;
                    ObjDA.SelectCommand = ObjCm;
                    ObjDA.Fill(Ds);

                    if (Ds != null && Ds.Tables.Count > 0 /*&& Ds.Tables[0].Rows.Count > 0*/)
                    {
                        DataTable Dt = Ds.Tables[0];
                    }
                    else
                    {
                        Ds = null;
                    }

                    foreach (DataParameters Dp in _DataObjectConf.SW_Parameters)
                    {
                        if (Dp.SW_Direction != null && (Dp.SW_Direction.ToUpper() == "O" || Dp.SW_Direction.ToUpper() == "OUT" || Dp.SW_Direction.ToUpper() == "OUTPUT"))
                        {
                            Dp.SW_Value = ObjDA.SelectCommand.Parameters[Dp.SW_Parametro].Value.ToString();
                        }
                    }

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    if (EstadoConexion() == true)
                    {
                        Close();
                    }

                    if (ObjCm != null)
                    {
                        ObjCm.Connection = null;
                    }
                    ObjCm = null;
                }
            }

            catch (Exception ex) { }
        }


    }
}
