using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE001.Models
{
    public class APTaxVATNumTableContract
    {
        
        public string CountryRegionId { get; set; }        
        public IdentificationType IdentificationType { get; set; }        
        public string VATNum { get; set; }        
        public string Name { get; set; }        
        public APECTypePerson TypePerson { get; set; }        
        public Boolean RelatedParty { get; set; }
    }
    
    public enum APECTypePerson { Natural = 1, Legal = 2 }
}
