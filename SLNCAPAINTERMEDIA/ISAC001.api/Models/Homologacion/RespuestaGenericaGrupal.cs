using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC001.api.Models.Homologacion
{
    public class RespuestaGenericaGrupal
    {
        public int StatusCode { get; set; }//Http Status Code
        public List<AXPEQUIVALENCIAGRUPO> Response { get; set; }//Contenido de respuesta del servidor
        public string DescripcionId { get; set; }//Descripción
        public List<string> ErrorList { get; set; }//Descripción
    }
}
