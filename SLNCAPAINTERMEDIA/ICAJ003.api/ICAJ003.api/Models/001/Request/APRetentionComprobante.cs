using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ003.api.Models._001.Request
{
    public class APRetentionComprobante
    {        
        public string InvoiceId { get; set; }        
        public DateTime TransDate { get; set; }        
        public string DocumentNum { get; set; }        
        public string Authorization { get; set; }        
        public string APTaxType { get; set; }        
        public decimal PercentRetention { get; set; }        
        public string APRetentionCode { get; set; }        
        public decimal Base { get; set; }        
        public decimal AmountTax { get; set; }        
        public Boolean IsElectronic { get; set; }        
        public decimal AmountTotal { get; set; }

    }
}
