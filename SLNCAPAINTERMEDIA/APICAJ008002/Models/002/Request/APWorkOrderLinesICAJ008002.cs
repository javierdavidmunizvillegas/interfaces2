using System;
using System.Collections.Generic;
using System.Text;

namespace APICAJ008002.Models._002.Request
{
    class APWorkOrderLinesICAJ008002
    {
        public string ItemId { get; set; }//Equipo
        public string Serial { get; set; }//Serie
        public string Model { get; set; }//Modelo
        public string Description { get; set; }//Descripción 
        public string Marca { get; set; }//Marca
        public string Line { get; set; }//Linea
        public string Group { get; set; }//Grupo
        public string SubGroup { get; set; }//SubGrupo
        public string Capacity { get; set; }//Capacidad



    }
}
