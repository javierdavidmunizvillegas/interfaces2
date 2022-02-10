using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICAJ008.api.Models._001.Response
{
    public class ItemListResponse
    {
        public string ItemId { get; set; }//Código del producto
        public int Qty { get; set; }//Cantidad
        public decimal AmountLine { get; set; }//Cantidad
        public int OrdenItems { get; set; }//Cantidad

    }
}
