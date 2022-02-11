using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE004.api.Models._001.Response
{
    public class APSalesLine
    {
        public DateTime APComboPromotionEndDate { get; set; }
        public string APComboPromotionId { get; set; }
        public decimal APComboPromotionQtyLimit { get; set; }
        public DateTime APComboPromotionStartDate { get; set; }
        public decimal APContributionValue { get; set; }
        public Boolean APHomeDelivery { get; set; }
        public DateTime APInstallationDate { get; set; }
        public string APItemPrimarySecondary { get; set; }
        public decimal APMarginAmount { get; set; }
        public string APMarginType { get; set; }
        public decimal APPromotionPO { get; set; }
        public string APSalesNumberAF { get; set; }
        public decimal APSalesPriceOfert { get; set; }
        public string APUserCreateCombo { get; set; }
        public string ConfigId { get; set; }
        public string DataAreaIdStockOwn { get; set; }
        public string InventLocationDescription { get; set; }
        public string InventLocationDescriptionStock { get; set; }
        public string InventLocationIdStock { get; set; }
        public string InventSerialId { get; set; }
        public string InventStatusID { get; set; }
        public string ItemId { get; set; }
        public string ItemIdAF { get; set; }
        public decimal LineDisc { get; set; }
        public decimal Qty { get; set; }
        public DateTime ReceiptDateRequested { get; set; }
        public decimal SalesPrice { get; set; }
        public string TAMFundID { get; set; }               
        public string WMSLocationDescription { get; set; }
        public string WMSLocationDescriptionStock { get; set; }
        public string WmsLocationIdStock { get; set; }    
    }
}
