using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICXP003.api.Models._001.Request
{
    public class APTaxVATNumTable
    {      
        public string CountryIdentification { get; set; }      
        public APECIdentificationType TypeIdentification { get; set; }      
        public string RUC { get; set; }      
        public string CompanyName { get; set; }      
        public APECTypePerson Type { get; set; }      
        public Boolean RelatedParty { get; set; }
    }
    public enum APECIdentificationType { RUC = 1, CD = 2, Passaport = 3, CF = 4 }
    public enum APECTypePerson { Natural = 1, Legal = 2 }
}
