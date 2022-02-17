using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.ICAJ008
{
    public class CarteraDetalleNC
    {
        public decimal AmountNC { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceId { get; set; }
        public string VoucherNC { get; set; }
        public string VoucherProvision { get; set; }
    }
}
