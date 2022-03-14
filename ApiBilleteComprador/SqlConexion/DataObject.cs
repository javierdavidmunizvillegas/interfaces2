using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlConexion
{
    public class DataObject
    {
        public string SW_SPName { get; set; }
        public List<DataParameters> SW_Parameters { get; set; }
        public string SW_ProductoName { get; set; }
        public string SW_Base { get; set; }
        public int SW_TimeOut { get; set; }
        public string CodResultado { get; set; }
        public string Mensaje { get; set; }

        public void Add(List<DataParameters> Parameters)
        {
            SW_Parameters = Parameters;
        }

        public void AddOut(List<DataParameters> Parameters)
        {

            if (Parameters != null)
            {
                DataParameters ObjPar = new DataParameters();
                ObjPar = new DataParameters { SW_Parametro = "@CodResultado", SW_Value = "", SW_Size = 3, SW_Type = DataParameters.SWDataType.String, SW_Direction = "OUT" };
                Parameters.Add(ObjPar);
                ObjPar = new DataParameters { SW_Parametro = "@Mensaje", SW_Value = "", SW_Size = 100, SW_Type = DataParameters.SWDataType.String, SW_Direction = "OUT" };
                Parameters.Add(ObjPar);
            }
            SW_Parameters = Parameters;
        }
    }
}
