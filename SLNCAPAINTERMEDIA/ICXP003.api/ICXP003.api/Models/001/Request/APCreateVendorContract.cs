using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICXP003.api.Models._001.Request
{
    public class APCreateVendorContract
    {
      
        public string APCodeIndependentVend { get; set; }      
        public string VATNum { get; set; }      
        public VendorType VendorType { get; set; }      
        public string Name { get; set; }      
        public string MiddleName { get; set; }      
        public string LastName { get; set; }      
        public string FirstNameSearch { get; set; }      
        public string VendorGroup { get; set; }      
        public string Language { get; set; }      
        public string TaxGroup { get; set; }      
        public string PaymentMethod { get; set; }      
        public string Currency { get; set; }       
        public string BankAccount { get; set; }       
        public List<APBankAccount> BankAccountList { get; set; }      
        public List<APLogisticsPostalAddress> LogisticsPostalAddressList { get; set; }      
        public List<APContactInfo> ContactInfoList { get; set; }      
        public APTaxVATNumTable TaxVATNumTable { get; set; }      
        public List<APLedgerJournalTransICXP003> LedgerJournalTrans { get; set; }
        public string AccountNum { get; set; }

    }
    public enum VendorType { Persona = 1, Organizacion = 2 }
}
