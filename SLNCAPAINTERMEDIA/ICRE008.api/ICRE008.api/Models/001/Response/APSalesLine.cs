using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE008.api.Models._001.Response
{
    public class APSalesLine
    {
        public string APCapacityId { get; set; }
        public string APComboId { get; set; }
        public string APGroupId { get; set; }
        public string APLineId { get; set; }
        public APSalesVehicle APSalesVehicle { get; set; }
        public string APSubGroupId { get; set; }
        public string BusinessUnit { get; set; }
        public string BusinessUnitDimensionF { get; set; }
        public string Color { get; set; }
        public string IdentificationItem { get; set; }
        public string InventLocationId { get; set; }
        public bool IsHomeDelivery { get; set; }
        public string ItemId { get; set; }
        public string ItemName { get; set; }
        public EcoResProductType ItemType { get; set; }
        public decimal LineAmount { get; set; }
        public decimal LineDisc { get; set; }
        public string Marca { get; set; }
        public decimal Qty { get; set; }
        public DateTime ReceiptDateRequested { get; set; }
        public decimal SalesPrice { get; set; }
        public string Serie { get; set; }
        public decimal SubTotalLinea { get; set; }
        public decimal Tax { get; set; }


    }
    public enum EcoResProductType { Item=1,service=2}
}
