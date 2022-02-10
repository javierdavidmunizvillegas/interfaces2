using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE012.api.Models._001.Response
{
    public class APSalesLine
    {
        public string ItemId { get; set; }
        public string APLineId { get; set; }
        public string APGroupId { get; set; }
        public string APSubGroupId { get; set; }
        public string APCategoryId { get; set; }
        public string ItemName { get; set; }
        public EcoResProductType ItemType { get; set; }
        public string InventLocationId { get; set; }
        public decimal Qty { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal LineAmount { get; set; }
        public decimal Tax { get; set; }
        public decimal SubTotalLinea { get; set; }
        public decimal LineDisc { get; set; }
        public string Serie { get; set; }
        public string Marca { get; set; }
        public string Color { get; set; }
        public string WMSLocationId { get; set; }
        

    }
    public enum EcoResProductType { Item =1, service  =2}
}
