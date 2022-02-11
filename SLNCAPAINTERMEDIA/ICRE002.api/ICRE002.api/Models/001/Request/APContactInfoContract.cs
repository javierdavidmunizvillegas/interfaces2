using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE002.api.Models
{
    public class APContactInfoContract
    {
        
      
        public string Description { get; set; }
        public LogisticsElectronicAddressMethodType Type { get; set; }
        public string Locator { get; set; }
        public string Extension { get; set; }
        public bool IsPrimary { get; set; }
        public bool FacturacionElectronica { get; set; }
        public Int64 RecId { get; set; }
    }
    public enum LogisticsElectronicAddressMethodType
    {
        telefono = 1,
        direccion_de_correo_electronico = 2,
        URL = 3,
        Telex = 4,
        fax = 5,
        facebook = 6,
        twitter = 7,
        LinkedIn = 8

    }
}

