using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConexion
{
    public class DataParameters
    {
        public enum SWDataType
        {
            Int = 1,
            String = 2,
            Date = 3,
            Double = 4,
            Gui = 5,
            Decimal = 6
        }

        public string SW_Parametro { get; set; }
        public object SW_Value { get; set; }
        public Int32 SW_Size { get; set; }
        public SWDataType SW_Type { get; set; }
        public string SW_Direction { get; set; }

        public void CargaParametros(string Parametro, object Value, Int32 Size, SWDataType Type)
        {
            try
            {
                SW_Parametro = Parametro;
                SW_Value = Value;
                SW_Size = Size;
                SW_Type = Type;
            }
            catch (Exception) { }
        }

        public DataParameters()
        {
            SW_Direction = "IN";
        }

        public DataParameters(string Parametro, object Value, Int32 Size, SWDataType Type)
        {
            try
            {
                SW_Parametro = Parametro;
                SW_Value = Value;
                SW_Size = Size;
                SW_Type = Type;
                SW_Direction = "IN";
            }
            catch (Exception) { }
        }

        public DataParameters(string Parametro, object Value, Int32 Size, SWDataType Type, string Direction)
        {
            try
            {
                SW_Parametro = Parametro;
                SW_Value = Value;
                SW_Size = Size;
                SW_Type = Type;
                SW_Direction = Direction;
            }
            catch (Exception) { }
        }
    }
}
