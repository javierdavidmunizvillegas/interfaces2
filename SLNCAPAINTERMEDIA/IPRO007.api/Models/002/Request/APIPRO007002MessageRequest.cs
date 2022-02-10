using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO007.api.Models._002.Request
{
    public class APIPRO007002MessageRequest
    {
        // 002 consultar , insertar pronostico de la demanda
        public string DataAreaId { get; set; }//Id de la compañía 

        public string SessionId { get; set; }//guid
        public string ItemId { get; set; }//codigo articulo
        public string TransDate { get; set; }// fecha consulta
        public string Enviroment { get; set; } //Id del ambiente
    }
}
