using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.api.Models._004.Response
{
    public class APCreditNoteLine
    {
        public string ItemId { get; set; } // Código de producto
        public decimal Qty { get; set; } // Cantidad
        public string DispositionCode { get; set; } // Código de disposición

    }
}
