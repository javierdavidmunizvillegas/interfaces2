using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE002.api.Models
{
    public class APCustTableContract
    {
        
        public DirPartyType Type { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string NameAlias { get; set; }
        public string NameOrganization { get; set; }
        public string NameAliasOrganization { get; set; }
        public string CustGroup { get; set; }
        public string Currency { get; set; }        
        public string LanguageId { get; set; }
        public int BirthDay { get; set; }
        public MonthsOfYear BirthMonth { get; set; }
        public int BirthYear { get; set; }
        public DateTime CustomerSince { get; set; }        
        public string TaxGroup { get; set; }        
        public IdentificationType IdentificationType { get; set; }        
        public string TaxVATNum { get; set; }        
        public APTaxVATNumTableContract APTaxVATNumTable { get; set; }
        public List<APFinancialDimension> APFinancialDimensionList { get; set; }
        public List<APLogisticsPostalAddressContract> APLogisticsPostalAddressList { get; set; }
        public List<APContactInfoContract> APContactInfoList { get; set; }
        public string APCodeIndependent { get; set; }
        public APIndependetEntrepContractICRE001001 APIndependetEntrep { get; set; }                
        public string PaymMode { get; set; }
        public string AccountNum { get; set; }
    }
    public enum IdentificationType { RUC = 1, CD = 2, Passaport = 3, CF = 4 }
    public enum DirPartyType { persona = 1, organizacion = 2 }
    public enum MonthsOfYear { Enero = 1, Febrero = 2, Marzo = 3, Abril = 4, Mayo = 5, Junio = 6, Julio = 7, Agosto = 8, Septiembre = 9, Octubre = 10, Noviembre = 11, Diciembre = 12 }
}

