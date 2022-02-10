using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA004.api.Models._001.Request
{
    public class APSalesLineIVTA004001
    {
        public string ItemId { get; set; }
        public string InventSerialId { get; set; }
        public string TAMFundID { get; set; }
        public string StyleId { get; set; }
        public string APComboPromotionId { get; set; }
        public decimal APContributionValue { get; set; }
        public string APUserCreateCombo { get; set; }
        public string APPrimarySecondary { get; set; }
        public int APComboPromotionQtyLimit { get; set; }
        public string APComboPromotionStartDate { get; set; }
        public string APComboPromotionEndDate { get; set; }
        public decimal APPromotionPO { get; set; }
        public string InventLocationIdDelivery { get; set; }
        public string WMSLocationIdDelivery { get; set; }
        public string InventStatusID { get; set; }
        public string ReceiptDateRequested { get; set; }
        public bool APHomeDelivery { get; set; }
        public string APInstallationDate { get; set; }
        public decimal QTYQuantity { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal APSalesCost { get; set; }
        public decimal APSalesPriceOffert { get; set; }
        public decimal APPromotionDiscount { get; set; }
        public decimal APComboDiscount { get; set; }
        public decimal APDiscountMargin { get; set; }
        public decimal APPaymModeDiscount { get; set; }
        public decimal APInventLocationIdDiscount { get; set; }
        public decimal APTermDiscount { get; set; }
        public decimal APInitialFeeDiscount { get; set; }
        public decimal LineDisc { get; set; }
        public string DataAreaIdStockOwn { get; set; }
        public string InventLocationIdStock { get; set; }
        public string WMSLocationIdStock { get; set; }
        public string APSalesNumberAF { get; set; }
        public string APItemIdAF { get; set; }
        public string APDiscountMarginId { get; set; }
        public string APECInvoiceDetail { get; set; }
        public int APNumLine { get; set; }
        public int APCreatePurchaseOrder { get; set; }
        public int APInvoiceSequence { get; set; }
        public string APInventLocationIdMoto { get; set; }
        public List<APFinancialDimension> APFinancialDimensionList { get; set; }


    }
}
