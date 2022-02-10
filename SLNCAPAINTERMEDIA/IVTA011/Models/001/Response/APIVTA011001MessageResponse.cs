using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA011.Models._001.Request
{
    class APIVTA011001MessageResponse
    {
        // 001 Periodo vigente para multinova
        public string SessionId { get; set; }//id de la sesion Guid
        public string StartDate { get; set; }//Fecha inicial periodo, Datetime
        public string EndDate { get; set; } //Fecha final periodo ,Datetime
        public string PeriodoId  { get; set; }//Periodo
    }

}
