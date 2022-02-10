using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ISAC017.api.Models._002.Request
{
    public class APReturnTableDisposition
    {
        [Required]
        public string ReturnItemNum { get; set; } //Número de orden de devolución
        //[Required]
        public string ItemId { get; set; } // Código del artículo
        [Required]
        public decimal Qty { get; set; } // Cantidad que registra
        [Required]
        public string DispositionCodeId { get; set; } // Código de disposición
        [Required]
        public string InventLocationId { get; set; } // Código del almacen
        //[Required]
        public string WMSLocationId { get; set; } // Código de localidad 
    }
}
