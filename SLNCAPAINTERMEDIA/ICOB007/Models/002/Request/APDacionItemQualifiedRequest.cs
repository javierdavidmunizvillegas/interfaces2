using System;
using System.Collections.Generic;
using System.Text;

namespace ICOB007.Models._002.Request
{
    public class APDacionItemQualifiedRequest
    {
        public string ItemId { get; set; }//Equipo
        public string Serie { get; set; }//Serie
        public string ItemName { get; set; }//Nombre del equipo
        public string Mark { get; set; }//Marca
        public string Line { get; set; }//Línea
        public string Group { get; set; }//Grupo    
        public string SubGroup { get; set; }//SubGrupo
        public string Capacity { get; set; }//Capacidad 
        public string NumberOT { get; set; }//Número de OT
        public string ItemIdQualified { get; set; }//Código de producto calificado
        public decimal Qty { get; set; }//Cantidad
        public decimal PriceUnit { get; set; }//Precio unitario }ojo noo esta en JSON
        public string Qualified { get; set; }//Calificado  int OJO
        public string Observation { get; set; }//Observaciones

    }
}
