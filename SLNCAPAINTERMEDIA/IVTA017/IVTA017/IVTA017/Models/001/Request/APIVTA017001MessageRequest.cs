using System;
using System.Collections.Generic;
using System.Text;

namespace IVTA017.Models._001.Request
{
    class APIVTA017001MessageRequest
    {
        public string DataAreaId { get; set; }
        public string Enviroment { get; set; }
        public string SessionId { get; set; }
        public string Address { get; set; }
        public string APStoreId { get; set; }
        public string City { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public string Description { get; set; }
        public string HomeDelivery { get; set; }
        public string ItemId { get; set; }
        public string PurchId { get; set; }
        public decimal PurchPrice { get; set; }
        public decimal PurchQty { get; set; }
        public string SalesId { get; set; }
        public string SalesOriginId { get; set; }
        public string State { get; set; }
        public string VatNum { get; set; }
        public string VendAccount { get; set; }
    }
}
