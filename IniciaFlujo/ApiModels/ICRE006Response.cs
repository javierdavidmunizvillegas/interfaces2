using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class ICRE006Request
    {
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }
        public string VATNum { get; set; }
    }

    public class ICRE006Response
    {
        public ClienteDto apCustTable { get; set; }
        public string SessionId { get; set; }
        public bool statusId { get; set; }
        public List<string> errorList { get; set; }
    }

    public class ClienteDto
    {
        public int type { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public string lastName { get; set; }
        public string nameAlias { get; set; }
        public string nameOrganization { get; set; }
        public string nameAliasOrganization { get; set; }
        public string custGroup { get; set; }
        public string languageId { get; set; }
        public string currency { get; set; }
        public int birthDay { get; set; }
        public int birthMonth { get; set; }
        public int birthYear { get; set; }
        public string customerSince { get; set; }
        public string taxGroup { get; set; }
        public int identificationType { get; set; }
        public string taxVATNum { get; set; }
        public List<DimensionFinancieraDto> apFinancialDimensionList { get; set; }
        public List<DireccionDto> apLogisticsPostalAddressList { get; set; }
        public List<ClienteContactoDto> apContactInfoList { get; set; }
        public EmprendedorRelacionDto apIndependetEntrep { get; set; }
        public string apCodeIndependent { get; set; }
        public string accountNum { get; set; }
         public string paymMode { get; set; }
        public IdentificacionFiscalDto apTaxVATNumTable { get; set; }
    }

    public class DimensionFinancieraDto
    {
        public string name { get; set; }
        public string valor { get; set; }
    }

    public class DireccionDto
    {
        public string street { get; set; }
        public string city { get; set; }
        public string countryRegionId { get; set; }
        public string state { get; set; }
        public List<ClienteContactoDto> apContactInfoList { get; set; }
        public string description { get; set; }
        public bool isPrimary { get; set; }
        public string district { get; set; }
        public string streetNumber { get; set; }
        public int role { get; set; }
        public long recId { get; set; }
        public long locationId { get; set; }
    }

    public class ClienteContactoDto
    {
        public bool facturacionElectronica { get; set; }
        public string description { get; set; }
        public int type  { get; set; }
        public string locator { get; set; }
        public string extension { get; set; }
        public bool isPrimary{ get; set; }
        public long recId { get; set; }
    }

    public class EmprendedorRelacionDto
    {
        public string vatNum { get; set; }
        public string accountNum { get; set; }
        public string apCodeIndependentVend { get; set; }
        public int apRelationStatus { get; set; }
        public DateTime dateEndRelation { get; set; }
        public DateTime dateEntryReq { get; set; }
        public DateTime dateStartRelation { get; set; }
    }

    public class IdentificacionFiscalDto
    {
        public string countryRegionId { get; set; }
        public string vatNum { get; set; }
        public string name { get; set; }
        public bool relatedParty { get; set; }
        public int identificationType { get; set; }
        public int typePerson { get; set; }
    }
}
