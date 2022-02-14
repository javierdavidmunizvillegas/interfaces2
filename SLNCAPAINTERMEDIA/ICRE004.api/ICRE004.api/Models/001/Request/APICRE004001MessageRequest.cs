using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE004.api.Models._001.Request
{
    public class APICRE004001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }
        public string APPaymModeGeneral { get; set; }     
        public List<string> CustAccountList { get; set; }
        public DateTime DateEnd { get; set; }
        public DateTime DateStart { get; set; }
        public List<DocumentStatus> DocumentStatusList { get; set; }       
        public Boolean PaymentRequest { get; set; }     
        public List<string> PurchOrderFormNumList { get; set; }
        public List<string> SalesIdList { get; set; }              
        public List<string> SalesResponsiblePersonnalNumberList { get; set; }
        public List<EnumPSalesStatus> SalesStatusList { get; set; }
       
        
        
        


    }
    public enum EnumPSalesStatus { None = 0, Backorder = 1, Delivered = 2, Invoiced = 3, Canceled = 4 }
    public enum DocumentStatus { None = 0, Quotation = 1, PurchaseOrder = 2, Confirmation = 3, PickingList = 4, PackingSlip = 5, ReceiptsList = 6, Invoice = 7, Lost = 12, Cancelled = 13 }
}
