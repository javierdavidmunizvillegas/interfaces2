using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE006.api.Models._001.Response
{
    public class APContactInfo
    {
        public string Description { get; set; }
        public LogisticsElectronicAddressMethodType Type { get; set; }
        public string Locator { get; set; }
        public string Extension { get; set; }
        public bool IsPrimary { get; set; }
        public bool FacturacionElectronica { get; set; }
        public Int64 RecId { get; set; }

    }
    public enum LogisticsElectronicAddressMethodType { Telefono=1,Direccion_correo_electronico=2,URL=3,Telex=4,Fax=5,Facebook=6,Twitter=7,LinkedIn=8}
}
