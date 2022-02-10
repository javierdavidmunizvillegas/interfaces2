using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ009.api.Models._001.Request
{
    public class APCustInvoiceServicesLineContract
    {
        [Required]
        public String NumLine { get; set; } //Linea
        [Required]
        public Decimal Qty { get; set; } //Cantidad
        [Required]
        public Decimal Amount { get; set; } //Monto
        [Required]
        public string ReasonSIAC { get; set; } //Motivo SIAC

    }
}
