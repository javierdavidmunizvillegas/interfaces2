using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DCAJ003.Models._002.Request
{
    public class APInformationReturn
    {
        public string SubGroupCategory { get; set; }         
        public string Invoice { get; set; }
        public string NumCertificado { get; set; }       
        public string ItemId { get; set; }         
        public string APPaymModeGeneral { get; set; }         
        public string StoreId { get; set; }
        public decimal StatusCertificate { get; set; }
        public string ReasonRefund { get; set; }       
        public decimal InsuredAmount { get; set; }         
        public decimal PremiumAmount { get; set; }         
        public string PremiumCurrency { get; set; }         
        public DateTime InvoiceDate { get; set; }         
        public DateTime EndValidity { get; set; }
    
    }
}
