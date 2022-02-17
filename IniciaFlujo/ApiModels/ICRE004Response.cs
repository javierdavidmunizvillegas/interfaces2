using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class ICRE004Response:Base
    {
        public string SessionId { get; set; }
        public bool StatusId { get; set; }
        public List<string> ErrorList { get; set; }
        public List<APSalesOrderICRE004001> APSalesOrderList { get; set; }

        public ICRE004Response()
        {
            ErrorList = new List<string>();
            APSalesOrderList = new List<APSalesOrderICRE004001>();
        }
    }
    public partial class APSalesOrderICRE004001
    {
        public APSalesOrderHeader APSalesOrderHeader { get; set; }
        public List<APDiaryJournal> APDiaryJournalList { get; set; }

        public APSalesOrderICRE004001()
        {
            APDiaryJournalList = new List<APDiaryJournal>();
        }
    }

    public partial class APSalesOrderHeader
    {
        public string DataAreaId { get; set; }
        public string SalesId { get; set; }
        public string PurchOrderFormNum { get; set; }
        public string APStoreId { get; set; }
        public string CustAccount { get; set; }
        public string CustName { get; set; }
        public decimal Amount { get; set; }
        public string SalesResponsibleName { get; set; }
        public string SalesResponsiblePersonalNumber { get; set; }
        public string APPaymModeGeneral { get; set; }
        public string APProductFinancialTCCode1 { get; set; }
        public string APProductFinancialCFCode { get; set; }
        public int SalesStatus { get; set; }
        public int DocumentStatus { get; set; }
        public string APPromotionalCode { get; set; }
        public decimal APPaymModeCash { get; set; }
        public decimal APPaymModeCF { get; set; }
        public decimal APPaymModeME { get; set; }
        public string APProductFinancialTCCode2 { get; set; }
        public string IndependentEntrepreneurId { get; set; }
        public bool APBillBuyer { get; set; }
        public decimal APAmountGeneratBillBuyer { get; set; }
        public bool APBillBuyerNC { get; set; }
        public string APBillBuyerProvisionId { get; set; }
        public string APBillBuyerNumber { get; set; }
        public decimal APAmountBillBuyer { get; set; }
        public string PostingProfile { get; set; }
        public string SalesPoolId { get; set; }
        public string SalesOriginId { get; set; }
        public string DeliveryPostalAddressRecId { get; set; }
        public DateTime ConfirmDate { get; set; }
        public List<APSalesLine> APSalesLineList { get; set; }

        public APSalesOrderHeader()
        {
            APSalesLineList = new List<APSalesLine>();
        }
    }

    public partial class APDiaryJournal
    {
        public string Voucher { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public bool AdvanceLiquidatedCheck { get; set; }
    }

    public partial class APSalesLine
    {
        public string APComboPromotionId { get; set; }
        public string TAMFundID { get; set; }
        public decimal APContributionValue { get; set; }
        public string APUserCreateCombo { get; set; }
        public string APItemPrimarySecondary { get; set; }
        public decimal APComboPromotionQtyLimit { get; set; }
        public DateTime APComboPromotionStartDate { get; set; }
        public DateTime APComboPromotionEndDate { get; set; }
        public decimal APPromotionPO { get; set; }
        public bool APHomeDelivery { get; set; }
        public DateTime APInstallationDate { get; set; }
        public decimal APSalesPriceOfert { get; set; }
        public string DataAreaIdStockOwn { get; set; }
        public string InventLocationIdStock { get; set; }
        public string InventLocationDescriptionStock { get; set; }
        public string WmsLocationIdStock { get; set; }
        public string WmsLocationDescriptionStock { get; set; }
        public string APSalesNumberAF { get; set; }
        public string ItemIdAF { get; set; }
        public string APMarginType { get; set; }
        public decimal APMarginAmount { get; set; }
        public string ItemId { get; set; }
        public string InventSerialId { get; set; }
        public string configId { get; set; }
        public string InventLocationId { get; set; }
        public string InventLocationDescription { get; set; }
        public string WmsLocationId { get; set; }
        public string WmsLocationDescription { get; set; }
        public string InventStatusID { get; set; }
        public DateTime ReceiptDateRequested { get; set; }
        public decimal Qty { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal LineDisc { get; set; }

    }
}
