using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE001.Models
{
    public class APContactInfoContract
    {
        public Int64 RecId { get; set; }
        public string Description { get; set; }
        public EnumType Type { get; set; }
        public string Locator { get; set; }        
        public string Extension { get; set; }
        public Boolean IsPrimary { get; set; }
        public Boolean FacturacionElectronica { get; set; }
        
    }
    public enum EnumType { Telefono = 1, Direccion_de_correo_electronico = 2, URL = 3, 
        TELEX = 4, FAX = 5, Facebook = 6, Twitter = 7, LinkedIn = 8 }
}
