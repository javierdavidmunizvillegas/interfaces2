using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE006.api.Models._001.Response
{
    public class APTaxVATNumTable
    {
        public string CountryRegionId { get; set; }
        public IdentificationType IdentificationType { get; set; }
        public string VATNum { get; set; }
        public string Name { get; set; }
        public TypePerson TypePerson { get; set; }
        public Boolean RelatedParty { get; set; }
    }

    public enum TypePerson { Natural = 1, Legal = 2 }
}
