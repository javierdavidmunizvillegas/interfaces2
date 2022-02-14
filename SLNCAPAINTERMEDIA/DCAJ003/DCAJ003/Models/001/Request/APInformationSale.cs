using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ003.Models._001.Request
{
    public class APInformationSale
    {
        public string Address { get; set; }
        public string APPaymModeGeneral { get; set; }
        public string AttributeProductRelation { get; set; }
        public DateTime BirthDate { get; set; }
        public string CityName { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string DescriptionGroupRelation { get; set; }
        public string DescriptionSubGroupRelation { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentType { get; set; }
        public string Email { get; set; }
        public DateTime EndAssistanceRelation { get; set; }
        public DateTime EndInvoiceDateRelation { get; set; }
        public DateTime EndValidity { get; set; }
        public DateTime EndValidityDate { get; set; }
        public decimal InsuredAmount { get; set; }
        public string Invoice { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime InvoiceDateRelation { get; set; }
        public string ItemId { get; set; }
        public string ItemNameRelation { get; set; }
        public string Mobile { get; set; }
        public int MonthsCoverage { get; set; }
        public int MonthsCoverageRelation { get; set; }
        public int MonthsGrace { get; set; }
        public string Name1 { get; set; }
        public string Name2 { get; set; }
        public int NumberInstallments { get; set; }
        public string NumCertificado { get; set; }
        public string PaymentMethod { get; set; }
        public string Phone { get; set; }
        public decimal PremiumAmount { get; set; }
        public string PremiumCurrency { get; set; }
        public decimal RegisterType { get; set; }
        public string SalesResponsibleName { get; set; }
        public string SalesResponsibleNumber { get; set; }
        public string SerialProductRelation { get; set; }
        public DateTime StartAssistanceRelation { get; set; }
        public DateTime StartValidity { get; set; }
        public string StoreDescription { get; set; }
        public string StoreId { get; set; }
        public string SubGroupCategory { get; set; }
        public string Surname1 { get; set; } 
        public string Surname2 { get; set; }
        public string TypeProductRelation { get; set; }         
        public string ItemIdRelation { get; set; } 
       
    }
}
