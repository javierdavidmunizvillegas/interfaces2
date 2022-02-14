using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVTA019.api.Models.Request
{
    public class APIVTA019001MessageRequest
    {
        
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }
        public decimal Amount { get; set; }
        public string BusinessUnit { get; set; }
        public string Cpn { get; set; }
        public string CustAccount { get; set; }
        public string CustIdentification { get; set; }
        public Boolean hasCreditNotePurchaserTicket { get; set; }
        public string InvoiceId { get; set; }
        public string Motive { get; set; }       
        
       // public Boolean hasPurchaserTicket { get; set; }        
        public APPurchaserTicketContract[] PurchaserTicketList { get; set; }
        public string SalesId { get; set; }
        public string StatusRegistrationFine { get; set; }
        public string Voucher { get; set; }
    }
}
