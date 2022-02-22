using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class VTA029Request
    {
        public string SalesId { get; set; }
        public string SalesIdSiac { get; set; }
        public string User { get; set; }
        public string Terminal { get; set; }
        public string Source { get; set; }
        public bool HasCreditNotePurchaserTicket { get; set; }
        public List<VTA029PurchaserTicketList> PurchaserTicketList { get; set; }
        public string InvoiceId { get; set; }
        public string Motive { get; set; }
        public decimal Amount { get; set; }
        public string Cpn { get; set; }
        public string Voucher { get; set; }

        public VTA029Request()
        {
            PurchaserTicketList = new List<VTA029PurchaserTicketList>();
        }

    }

    public class VTA029PurchaserTicketList
    {
        public string PurchaserTicketNumber { get; set; }
        public string PurchaserTicketProvisionId { get; set; }
        public decimal PurchaserTicketAmount { get; set; }
    }
}
