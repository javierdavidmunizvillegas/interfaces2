using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE006.api.Models._001.Response
{
    public class APCustTable
    {
        public DirPartyType Type { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string LastName { get; set; }
        public string NameAlias { get; set; }
        public string NameOrganization { get; set; }
        public string NameAliasOrganization { get; set; }
        public string CustGroup { get; set; }
        public string LanguageId { get; set; }
        public string Currency { get; set; }
        public int BirthDay { get; set; }
        public MonthsOfYear BirthMonth { get; set; }
        public int BirthYear { get; set; }
        public DateTime CustomerSince { get; set; }       
        public string TaxGroup { get; set; }
        public IdentificationType IdentificationType { get; set; }
        public string TaxVATNum { get; set; }
        public List<APFinancialDimension> APFinancialDimensionList { get; set; }
        public List<APLogisticsPostalAddress> APLogisticsPostalAddressList { get; set; }
        public List<APContactInfo> APContactInfoList { get; set; }
        public APIndependetEntrepContractICRE001001 APIndependetEntrep { get; set; }
        public string APCodeIndependent { get; set; }
        public string AccountNum { get; set; }
        public string PaymMode { get; set; }
        public APTaxVATNumTable APTaxVATNumTable { get; set; }

    }
    public enum MonthsOfYear { January=1, February=2, March=3, April=4, May=5, June=6, July=7, August=8, September=9, October=10, November=11, December =12}
    public enum DirPartyType { persona = 1, organizacion = 2 }
    public enum IdentificationType { Ninguno = 0, RUC = 1, Cedula = 2, Pasaporte = 3, Consumidor_final = 4 }
}
