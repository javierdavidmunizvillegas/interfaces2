using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiModels
{
    public class GTM004Request
    {
        public string DataAreaId { get; set; }
        public string StoreId { get; set; }
        public string InvoiceId { get; set; }
        public string Id { get; set; }
        public string Names { get; set; }
        public string LastName { get; set; }
        public string ConventionalTelephone { get; set; }
        public string CellPhone { get; set; }
        public string Email { get; set; }
        public string ItemId { get; set; }
        public string ItemDescription { get; set; }
        public string Serie { get; set; }
        public string Chassis { get; set; }
        public string Cpn { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceHour { get; set; }
        public string Establishment { get; set; }
        public string EmissionPoint { get; set; }
        public string Sequential { get; set; }
        public string PaymentForm { get; set; }
        public string UserLogin { get; set; }
        public DateTime AdmissionDate { get; set; }
        public string IncomeTerminal { get; set; }
        public string ProductionYear { get; set; }
        public string Cylinder { get; set; }
        public string Colour { get; set; }
        public string Model { get; set; }
        public string CountryOrigin { get; set; }
        public string Brand { get; set; }
    }
}
