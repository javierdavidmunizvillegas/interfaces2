using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.ICAJ008
{
    public class CarteraDetalleProductos
    {
        public string ItemId { get; set; }
        public decimal Qty { get; set; }
        public decimal AmountLine { get; set; }
        public int OrdenItems { get; set; }
    }
}
