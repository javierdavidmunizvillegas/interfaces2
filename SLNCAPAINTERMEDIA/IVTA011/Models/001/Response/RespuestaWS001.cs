using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA011.Models._001.Response
{
    class RespuestaWS001
    {
        public string StartDate  { get; set; }//fecha incial
        public string EndDate  { get; set; }//fecha final
        public string PeriodoId { get; set; }//periodo
        public bool StatusId { get; set; }//Estado true = ok y False = Error
        public List<string> ErrorList { get; set; }//Listado de errores

    }
}
