using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IPRO007.api.Models._001.Request
{
    public class APForecastSales
    {
        public string ItemId { get; set; } //codigo de articulo
        public decimal SalesQty { get; set; } //cantidad de ventas
        public string  StartDate { get; set; } //fecha DateTime
        public string InventLocationId { get; set; } // almacen
        public string WHSInventStatusId { get; set; } //Estado dle inventario /unidad de negocio
        public string EcoResItemStyleName { get; set; } // estilo
        public string MonthOfYear { get; set; } //mes

       // public decimal SalesQty { get; set; } //cantidad de ventas
        //public string InventLocationId { get; set; } // almacen
        //public string WHSInventStatusId { get; set; } //Estado del inventario /unidad de negocio
        //public string StartDate { get; set; } //fecha DateTime
    }
}
