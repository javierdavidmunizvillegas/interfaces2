using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels.ICAJ008
{
    public class CarteraDetalleVM
    {
        public int SecuenciaFacturacion { get; set; }
        public string Voucher { get; set; }
        public string InvoiceId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<CarteraDetalleProductos> ItemList { get; set; }
        public CarteraDetalleVM()
        {
            ItemList = new List<CarteraDetalleProductos>();
        }
    }
}
