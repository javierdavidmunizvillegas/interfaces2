using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ICRE012.api.Models._001.Request
{
    public class APICRE012001MessageRequest
    {
        [Required]
        public string DataAreaId { get; set; }
      
        public string Enviroment { get; set; }
        public string CustAccount { get; set; }
        [Required]
        public int DocumenType { get; set; }
                   
       public string InvoiceIdNC { get; set; }
  
        public string APStoreId { get; set; }
  
        public string BusinnesUnit { get; set; }
       
        public DateTime DateStart { get; set; } //DateTime
      
        public DateTime DateEnd { get; set; } //DateTime
        public string ItemId { get; set; }
        public string APIdentificationList { get; set; }
        public string TransactionType { get; set; }

        public string Vourcher { get; set; }
        public string SalesId { get; set; }
        public string OrdenTrabajo { get; set; }

        public string PaymMode { get; set; }
     
        public string CajaCode { get; set; }
       
  
        public string DocumentNum { get; set; }

        public string SessionId { get; set; }

        /* [Required]
         public string ReasonId { get; set; }

        [Required]
         public string InvoiceIdFC { get; set; }*/

    }
   // public enum DocumentType { Invoice=0, CreditNote =1, JournalAdvance =2}
}
