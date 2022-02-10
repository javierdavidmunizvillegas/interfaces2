using System;
using System.Collections.Generic;
using System.Text;

namespace ICOB007.Models._001.Request
{
    public class PICOB007001MessageRequest_siac
    {
        public string DataAreaId { get; set; }//Id de la compañía
        public string Enviroment { get; set; }//Entorno
        public string SessionId { get; set; }//Id de sesión
        public string sucursal { get; set; }
        public string tipO_ATENCION { get; set; }
        public string codigO_TALLER { get; set; }
        public string origen { get; set; }
        public string ccodigodynamic { get; set; }
        public string tipopersona { get; set; }
        public string tipodocumento { get; set; }
        public string cedula { get; set; }
        public string nombrE_CLIENTE { get; set; }
        public string nombrE1_CLIENTE { get; set; }
        public string nombrE2_CLIENTE { get; set; }
        public string apellidO1_CLIENTE { get; set; }
        public string apellidO2_CLIENTE { get; set; }
        public string pais { get; set; }
        public string provincia { get; set; }
        public string canton { get; set; }
        public string calle { get; set; }
        public string nocalle { get; set; }
        public string telefono { get; set; }
        public string email { get; set; }
        public string almacen { get; set; }
        public string factura { get; set; }
        public string facT_SRI { get; set; }
        public string fechA_FACTURA { get; set; }
        public string equipo { get; set; }
        public string serie { get; set; }
        public string modelO_EQUIPO { get; set; }
        public string descripcioN_EQUIPO { get; set; }
        public string marca { get; set; }
        public string linea { get; set; }
        public string capacidad { get; set; }
        public string subgrupo { get; set; }
        public string tipO_ARTICULO { get; set; }


    }
}
