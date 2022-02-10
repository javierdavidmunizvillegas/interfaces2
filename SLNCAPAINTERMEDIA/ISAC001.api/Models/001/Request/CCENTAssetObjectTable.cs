using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC001.api.Models._001.Request
{
    public class CCENTAssetObjectTable
    {

        public string Active { get; set; }//Activo
        public string Name { get; set; }//nombre
        public string ObjectValidFrom { get; set; }//Vigente DAtetime
        public string SerialID { get; set; }//Numero de Serie
        public string Product { get; set; }//Fabricante/Marca
        public string Model { get; set; }//Modelo / codigo de articulo
        public string LastNameCust { get; set; }//Apellido cliente / atributos
        public string NameCust { get; set; }//Vigencia
        public string AccountNum { get; set; }//Codigo Cliente /Atributo
        public string NumOT { get; set; }//Numero orden de OT / atributo
        public string Observation { get; set; } // Observacion / atributo

    }
}
