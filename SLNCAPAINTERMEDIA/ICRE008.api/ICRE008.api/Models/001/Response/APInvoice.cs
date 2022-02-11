using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE008.api.Models._001.Response
{
    public class APInvoice
    {
        public string VatNum { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string phoneNumber { get; set; }
        public string DataAreaId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string invoiceId { get; set; }
        public string Establishment { get; set; }
        public string EmissionPoint { get; set; }
        public DateTime DateAuthSRI { get; set; }
        public string AuthorizationNumber { get; set; }      
       
    }
   
}
