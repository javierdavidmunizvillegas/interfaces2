using System;
using System.Collections.Generic;
using System.Text;

namespace APICAJ008002.Models._003.Request
{
    class ItemListResponse
    {
        public string ItemId { get; set; }//Código del producto
        public int Qty { get; set; }//Cantidad
        public decimal AmountLine { get; set; }//Cantidad
        public int OrdenItems { get; set; }//Cantidad
    }
}
