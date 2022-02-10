using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA004.api.Models._001.Request
{
    public class APSalesTableIVTA004001
    {

        public string PurchOrderFormNum { get; set; }
        public string CustAccount { get; set; }
        public string SalesPoolId { get; set; }
        public string SalesOriginId { get; set; }
        public Int64 DeliveryPostalAddress { get; set; }
        public string InventLocationId { get; set; }
        public string WorkSalesResponsibleCode { get; set; }
        public string APPaymModeGeneral { get; set; }
        public string APSalesIdProductAsistenciaFacilita { get; set; }
        
        public string APPromotionalCode { get; set; }
        public string APProductFinancialCECode { get; set; }
        public string APProductFinancialCEDescription { get; set; }
        public string APProductFinancialTCCode { get; set; }
        public string APProductFinancialTCDescription { get; set; }
        public decimal APAmountPayModeCash { get; set; }//real
        public decimal APAmountPayModeCredit { get; set; }//real
        public decimal APAmountPayModeElectronic { get; set; }//real
        public string APProductFinancialTCCode2 { get; set; }
        public string APProductFinancialTCDescription2 { get; set; }
        public string IndependentEntrepreneurId { get; set; }
        public string APStoreId { get; set; }
        // public bool APBillBuyerCheck { get; set; }
        // public decimal APAmountBillBuyer { get; set; }//real
        // public bool APBillBuyerCheckNC { get; set; }
        // public string PostingProfile { get; set; }
        public string PaymFirstShareDate { get; set; }
        public string PaymLastShareDate { get; set; }
        public int ShareNumber { get; set; }
        public int GraceMonths { get; set; }
        //public List<APSalesTableBillBuyerNC> APSalesTableBillBuyerNCList { get; set; }
        public List<APBuyerNumberTickectList> APBuyerNumberTickectList { get; set; }
        public List<APSalesLineIVTA004001> APSalesLineList { get; set; }




    }
}
