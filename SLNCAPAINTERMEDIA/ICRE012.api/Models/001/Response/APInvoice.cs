using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE012.api.Models._001.Response
{
    public class APInvoice
    {
        public string VatNum { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string phoneNumber { get; set; }
        public string dataAreaId { get; set; }
        public string InvoiceDate { get; set; } //DateTime
        public string InvoiceId { get; set; }
        public string Vourcher { get; set; }
        public string Establishment { get; set; }
        public string EmissionPoint { get; set; }
        public string DateAuthSRI { get; set; } //DateTime
        public string AuthorizationNumber { get; set; }
        public string DocumentApplied { get; set; }
        public string EstablishmentApplied { get; set; }
        public string EmissionPointApplied { get; set; }
        public decimal DocumentAmountBalance { get; set; }
        public string APIdentificationList { get; set; }
        public APSalesTable APSalesTable { get; set; }

        public List<APCreditNoteServicesList> APCreditNoteServices { get; set; }
       // public List<APFinancialDimension> APFinancialDimensionList { get; set; }








    }
}
