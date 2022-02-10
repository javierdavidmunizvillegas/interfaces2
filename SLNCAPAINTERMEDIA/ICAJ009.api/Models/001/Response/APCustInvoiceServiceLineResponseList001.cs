using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ009.api.Models._001.Response
{
    public class APCustInvoiceServiceLineResponseList001
    {
        public Decimal LineNumber { get; set; } //numero d elinea LineNum Decimal
        public string LineDescriptions { get; set; } // Descripciones de las líneas ItemFreeTxt String(1000)

        public Decimal LineAmount { get; set; } // Monto de la línea Amount Decimal
        public Decimal TaxAmount { get; set; } // Monto del Impuesto Amount Decimal
    }
}
