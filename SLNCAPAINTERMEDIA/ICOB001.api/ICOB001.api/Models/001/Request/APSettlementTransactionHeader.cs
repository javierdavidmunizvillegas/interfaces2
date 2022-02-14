using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICOB001.api.Models._001.Request
{
    public class APSettlementTransactionHeader
    {
        [Required]
        public string VoucherSettlement { get; set; }

        [Required]
        public string IdReciboCobro { get; set; }

        [Required]
        public DateTime DateTrans { get; set; }

        public string InvoiceId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public List<APDocumentICOB001001> APDocumentList { get; set; }

        public Boolean StatusId { get; set; }
    }
}
